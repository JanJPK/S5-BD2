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
using Warlord.ViewModel.Detail.Browse;
using Warlord.Wrapper;

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
            IVehicleModelRepository vehicleModelRepository,
            IManufacturerLookupService manufacturerLookupService)
            : base(eventAggregator, messageService, userPrivilege)
        {
            this.vehicleModelRepository = vehicleModelRepository;
            this.manufacturerLookupService = manufacturerLookupService;

            EventAggregator.GetEvent<AfterCollectionSavedEvent>().Subscribe(AfterCollectionSaved);
            EventAggregator.GetEvent<AfterDetailViewSavedEvent>().Subscribe(AfterDetailViewSaved);

            CreateNewVehicleCommand = new DelegateCommand(OnCreateNewVehicleExecute, OnCreateNewVehicleCanExecute);
            OpenBrowseViewWithChildrenCommand =
                new DelegateCommand(OpenBrowseViewWithChildrenExecute, OpenBrowseViewWithChildrenCanExecute);

            Manufacturers = new ObservableCollection<LookupItem>();
        }

        #endregion

        #region Public Properties

        public ICommand CreateNewVehicleCommand { get; }

        public ObservableCollection<LookupItem> Manufacturers { get; }

        public ICommand OpenBrowseViewWithChildrenCommand { get; }

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

        protected override async void OnDeleteExecute()
        {
            if (await vehicleModelRepository.HasVehiclesAsync(VehicleModel.Id))
            {
                await MessageService.ShowInfoDialog(
                    $"Models of {VehicleModel.Name} are currently up for sale and therefore this entity cannot be deleted.");
                return;
            }

            bool result =
                await MessageService.ShowConfirmDialog($"Do you wish to delete the model {VehicleModel.Name}?");
            if (result)
            {
                vehicleModelRepository.Remove(VehicleModel.Model);
                await vehicleModelRepository.SaveAsync();
                RaiseDetailViewDeletedEvent(VehicleModel.Id);
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
        }

        private async void AfterCollectionSaved(AfterCollectionSavedEventArgs args)
        {
            if (args.ViewModelName == nameof(ManufacturerDetailVM))
            {
                await LoadManufacturersAsync();
            }
            SetTitle();
        }

        private async void AfterDetailViewSaved(AfterDetailViewSavedEventArgs args)
        {
            if (args.Id == VehicleModel.ManufacturerId && args.ViewModelName == nameof(ManufacturerDetailVM))
            {
                await vehicleModelRepository.ReloadAsync(Id);
                await LoadAsync(Id);
            }
        }

        protected override async void AfterSaveAction()
        {
            HasChanges = vehicleModelRepository.HasChanges();
            Id = vehicleModel.Id;
            await LoadAsync(Id);
            RaiseDetailViewSavedEvent(VehicleModel.Id, Title);
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
                VehicleModel.EnginePower = 0;
                VehicleModel.Weight = 0;
                VehicleModel.Crew = 0;
                //VehicleModel.ManufacturerId = 0;
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

        private bool OnCreateNewVehicleCanExecute()
        {
            return VehicleModel.Id > 0;
        }

        private void OnCreateNewVehicleExecute()
        {
            EventAggregator.GetEvent<OnNewDependantDetailViewOpenedEvent>().Publish(
                new OnNewDependantDetailViewOpenedEventArgs
                {
                    DependantOnId = vehicleModel.Id,
                    ViewModelName = nameof(VehicleDetailVM)
                });
        }

        private bool OpenBrowseViewWithChildrenCanExecute()
        {
            return VehicleModel.Id > 0 && !HasChanges;
        }

        private void OpenBrowseViewWithChildrenExecute()
        {
            EventAggregator.GetEvent<OnBrowseViewFilteredOpenedEvent>()
                .Publish(new OnBrowseViewFilteredOpenedEventArgs
                {
                    Id = -1,
                    ViewModelName = nameof(VehicleBrowseVM),
                    FilterDisplayMember = VehicleModel.Name
                });
        }

        private void SetTitle()
        {
            Title = VehicleModel.ManufacturerId > 0
                ? $"{Manufacturers[VehicleModel.ManufacturerId - 1].DisplayMember} {VehicleModel.Name}"
                : $"{VehicleModel.Name}";
        }

        #endregion
    }
}