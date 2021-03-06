﻿using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Prism.Commands;
using Prism.Events;
using Warlord.Event;
using Warlord.Model;
using Warlord.Service;
using Warlord.Service.Message;
using Warlord.Service.Repositories;
using Warlord.Wrapper;

namespace Warlord.ViewModel.Detail
{
    public class VehicleDetailVM : BaseDetailVM
    {
        #region Fields

        private readonly IVehicleModelRepository vehicleModelRepository;
        private readonly IVehicleRepository vehicleRepository;

        private ImageSource image;

        private VehicleWrapper vehicle;
        private VehicleModelWrapper vehicleModel;

        #endregion

        #region Constructors and Destructors

        public VehicleDetailVM(IEventAggregator eventAggregator, IMessageService messageService,
            IUserPrivilege userPrivilege,
            IVehicleRepository vehicleRepository, IVehicleModelRepository vehicleModelRepository)
            : base(eventAggregator, messageService, userPrivilege)
        {
            this.vehicleRepository = vehicleRepository;
            this.vehicleModelRepository = vehicleModelRepository;
            EventAggregator.GetEvent<AfterDetailViewSavedEvent>().Subscribe(AfterDetailViewSaved);
        }

        #endregion

        #region Public Properties

        public ImageSource Image
        {
            get => image;
            set
            {
                image = value;
                OnPropertyChanged();
            }
        }

        public VehicleWrapper Vehicle
        {
            get => vehicle;
            set
            {
                vehicle = value;
                OnPropertyChanged();
            }
        }

        public VehicleModelWrapper VehicleModel
        {
            get => vehicleModel;
            set
            {
                vehicleModel = value;
                OnPropertyChanged();
            }
        }

        public int VehicleModelId { get; set; }

        #endregion

        #region Public Methods and Operators

        public override async Task LoadAsync(int id)
        {
            var vehicle = id > 0
                ? await LoadVehicle(id)
                : CreateNewVehicle();

            var vehicleModel = await vehicleModelRepository.GetByIdAsync(VehicleModelId);

            Id = id;

            InitializeVehicleModel(vehicleModel);
            InitializeVehicle(vehicle);
            await LoadImage();
        }

        #endregion

        #region Methods

        protected override async void OnDeleteExecute()
        {
            if (await vehicleRepository.HasOrderAsync(Vehicle.Id))
            {
                await MessageService.ShowInfoDialog(
                    $"An order includes {Title} and therefore this entity cannot be deleted.");
                return;
            }

            bool result = await MessageService.ShowConfirmDialog(
                $"Do you wish to delete the vehicle {Title}?");
            if (result)
            {
                vehicleRepository.Remove(Vehicle.Model);
                await vehicleRepository.SaveAsync();
                RaiseDetailViewDeletedEvent(Vehicle.Id);
            }
        }

        protected override bool OnSaveCanExecute()
        {
            return Vehicle != null
                   && Vehicle.Model.Order == null
                   && !Vehicle.HasErrors
                   && HasChanges;
        }

        protected override async void OnSaveExecute()
        {
            await SaveWithOptimisticConcurrencyAsync(vehicleRepository.SaveAsync);
        }

        private async void AfterDetailViewSaved(AfterDetailViewSavedEventArgs args)
        {
            if (args.Id == Id && args.ViewModelName == nameof(VehicleDetailVM))
            {
                await vehicleRepository.ReloadAsync(Id);
                await LoadAsync(Id);
            }

            if (args.Id == VehicleModelId && args.ViewModelName == nameof(VehicleModelDetailVM))
            {
                await vehicleModelRepository.ReloadAsync(VehicleModelId);
                await LoadAsync(Id);
            }
        }

        protected override async void AfterSaveAction()
        {
            HasChanges = vehicleRepository.HasChanges();
            Id = vehicle.Id;
            await LoadAsync(Id);
            RaiseDetailViewSavedEvent(Vehicle.Id, Title);
        }

        private Vehicle CreateNewVehicle()
        {
            var vehicle = new Vehicle {VehicleModelId = VehicleModelId};
            vehicleRepository.Add(vehicle);
            return vehicle;
        }

        private void InitializeVehicle(Vehicle vehicle)
        {
            Vehicle = new VehicleWrapper(vehicle);
            Vehicle.PropertyChanged += (s, e) =>
            {
                if (!HasChanges)
                {
                    HasChanges = vehicleRepository.HasChanges();
                }

                if (e.PropertyName == nameof(Vehicle.HasErrors))
                {
                    ((DelegateCommand) SaveCommand).RaiseCanExecuteChanged();
                }

                if (e.PropertyName == nameof(Vehicle.Price))
                {
                    SetTitle();
                }
            };

            if (Vehicle.Id == 0)
            {
                Vehicle.Color = "";
                Vehicle.Condition = "";
            }

            SetTitle();
        }

        private void InitializeVehicleModel(VehicleModel model)
        {
            VehicleModel = new VehicleModelWrapper(model);
        }

        private async Task LoadImage()
        {
            if (!string.IsNullOrEmpty(Vehicle.Imagepath)
                && File.Exists(Vehicle.Imagepath))
            {
                Uri uri = new Uri(Vehicle.Imagepath);
                BitmapImage bitmapImage = new BitmapImage();

                bitmapImage.BeginInit();
                bitmapImage.UriSource = uri;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();

                Image = bitmapImage;
            }
            else
            {
                await MessageService.ShowInfoDialog("Error during image loading. Default image loaded instead.");
                //Image = new BitmapImage(new Uri("pack://application:,,,/Resources/warlord_logo_light.png"));
            }
        }

        private async Task<Vehicle> LoadVehicle(int id)
        {
            var vehicle = await vehicleRepository.GetByIdAsync(id);
            VehicleModelId = vehicle.VehicleModelId;
            return vehicle;
        }

        private void SetTitle()
        {
            Title = $"{VehicleModel.Name} {Vehicle.Price}€";
        }

        #endregion
    }
}