using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Commands;
using Prism.Events;
using Warlord.Event;
using Warlord.Model;
using Warlord.Service;
using Warlord.Service.Lookups;
using Warlord.Service.Message;
using Warlord.Service.Repositories;
using Warlord.Wrappers;

namespace Warlord.ViewModel.Detail
{
    public class VehicleModelDetailVM : BaseDetailVM
    {
        #region Fields

        private readonly IManufacturerLookupService manufacturerLookupService;

        private readonly IVehicleModelRepository vehicleModelRepository;

        private VehicleModelWrapper vehicleModel;

        #endregion

        #region Constructors and Destructors

        public VehicleModelDetailVM(IEventAggregator eventAggregator, IMessageService messageService,
            IUserPrivilege userPrivilege,
            IVehicleModelRepository vehicleModelRepository, IManufacturerLookupService manufacturerLookupService)
            : base(eventAggregator, messageService, userPrivilege)
        {
            this.vehicleModelRepository = vehicleModelRepository;
            this.manufacturerLookupService = manufacturerLookupService;

            eventAggregator.GetEvent<AfterCollectionSavedEvent>().Subscribe(AfterCollectionSaved);

            CreateNewVehicleCommand = new DelegateCommand(OnCreateNewVehicleExecute, OnCreateNewVehicleCanExecute);

            Manufacturers = new ObservableCollection<LookupItem>();
        }

        #endregion

        #region Public Properties

        public ObservableCollection<LookupItem> Manufacturers { get; }

        public VehicleModelWrapper VehicleModel
        {
            get => vehicleModel;
            set
            {
                vehicleModel = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Public Methods and Operators

        public override async Task LoadAsync(int id)
        {
            var vehicleModel = id > 0
                ? await vehicleModelRepository.GetByIdAsync(id)
                : CreateNewVehicleModel();

            Id = id;
            await LoadManufacturersAsync();

            InitializeVehicleModel(vehicleModel);
        }

        #endregion

        #region Methods

        private void AfterSaveAction()
        {
            SetTitle();
            HasChanges = vehicleModelRepository.HasChanges();
            Id = VehicleModel.Id;
            RaiseDetailSavedEvent(VehicleModel.Id, Title);
        }

        private VehicleModel CreateNewVehicleModel()
        {
            var vehicleModel = new VehicleModel();
            vehicleModelRepository.Add(vehicleModel);
            return vehicleModel;
        }

        private void InitializeVehicleModel(VehicleModel vehicleModel)
        {
            VehicleModel = new VehicleModelWrapper(vehicleModel);
            VehicleModel.PropertyChanged += (s, e) =>
            {
                if (!HasChanges)
                {
                    HasChanges = vehicleModelRepository.HasChanges();
                }

                if (e.PropertyName == nameof(VehicleModel.HasErrors))
                {
                    ((DelegateCommand) SaveCommand).RaiseCanExecuteChanged();
                }

                if (e.PropertyName == nameof(VehicleModel.Name))
                {
                    SetTitle();
                }
            };

            if (VehicleModel.Id == 0)
            {
                VehicleModel.Name = "New vehicle model";
                VehicleModel.Engine = "";
                VehicleModel.MainArmament = "";
                VehicleModel.ManufacturerId = 0;
            }

            SetTitle();
        }

        private async Task LoadManufacturersAsync()
        {
            Manufacturers.Clear();
            //Manufacturers.Add(new NullLookupItem {DisplayMember = " - "});

            var lookup = await manufacturerLookupService.GetManufacturerLookupAsync();
            foreach (var lookupItem in lookup)
            {
                Manufacturers.Add(lookupItem);
            }
        }

        private void SetTitle()
        {
            Title = VehicleModel.ManufacturerId > 0
                ? $"{Manufacturers[VehicleModel.ManufacturerId - 1].DisplayMember} {VehicleModel.Name}"
                : $"{VehicleModel.Name}";
        }

        #endregion

        #region Event-related

        public ICommand CreateNewVehicleCommand { get; }

        private bool OnCreateNewVehicleCanExecute()
        {
            return VehicleModel.Id > 0;
        }

        private void OnCreateNewVehicleExecute()
        {
            EventAggregator.GetEvent<OnNewDependantDetailOpenedEvent>().Publish(
                new OnNewDependantDetailOpenedEventArgs
                {
                    DependantOnId = vehicleModel.Id,
                    ViewModelName = nameof(VehicleDetailVM)
                });
        }

        protected override async void OnDeleteExecute()
        {
            if (await vehicleModelRepository.HasVehiclesAsync(VehicleModel.Id))
            {
                await MessageService.ShowInfoDialog(
                    $"Models of {VehicleModel.Name} are currently up for sale and therefore this entity cannot be deleted.");
                return;
            }

            bool result = await MessageService.ShowConfirmDialog($"Do you wish to delete the model {VehicleModel.Name}?");
            if (result)
            {
                vehicleModelRepository.Remove(VehicleModel.Model);
                await vehicleModelRepository.SaveAsync();
                RaiseDetailDeletedEvent(VehicleModel.Id);
            }
        }

        protected override bool OnSaveCanExecute()
        {
            return VehicleModel != null
                   && !VehicleModel.HasErrors
                   && HasChanges;
        }

        protected override async void OnSaveExecute()
        {
            await SaveWithOptimisticConcurrencyAsync(vehicleModelRepository.SaveAsync);
            AfterSaveAction();
            await LoadAsync(VehicleModel.Id);
        }

        private async void AfterCollectionSaved(AfterCollectionSavedEventArgs args)
        {
            if (args.ViewModelName == nameof(ManufacturerDetailVM))
            {
                await LoadManufacturersAsync();
            }
            SetTitle();
        }

        #endregion
    }
}