using System.Threading.Tasks;
using Prism.Commands;
using Prism.Events;
using Warlord.Model;
using Warlord.Service.Message;
using Warlord.Service.Repositories;
using Warlord.Wrappers;

namespace Warlord.ViewModel.Detail
{
    public class VehicleDetailVM : BaseDetailVM
    {
        #region Fields

        private readonly IVehicleModelRepository vehicleModelRepository;
        private readonly IVehicleRepository vehicleRepository;

        private VehicleWrapper vehicle;
        private VehicleModelWrapper vehicleModel;

        #endregion

        #region Constructors and Destructors

        public VehicleDetailVM(IEventAggregator eventAggregator, IMessageService messageService,
            IVehicleRepository vehicleRepository, IVehicleModelRepository vehicleModelRepository)
            : base(eventAggregator, messageService)
        {
            this.vehicleRepository = vehicleRepository;
            this.vehicleModelRepository = vehicleModelRepository;
        }

        #endregion

        #region Public Properties

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
        }

        #endregion

        #region Methods

        private void AfterSaveAction()
        {
            HasChanges = vehicleRepository.HasChanges();
            Id = vehicle.Id;
            RaiseDetailSavedEvent(Vehicle.Id, $"{Title}");
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

                //if (e.PropertyName == nameof(Vehicle.ShortName))
                //{
                //    SetTitle();
                //}
            };

            if (Vehicle.Id == 0)
            {
            }

            SetTitle();
        }

        private void InitializeVehicleModel(VehicleModel model)
        {
            vehicleModel = new VehicleModelWrapper(model);
        }

        private async Task<Vehicle> LoadVehicle(int id)
        {
            var vehicle = await vehicleRepository.GetByIdAsync(id);
            VehicleModelId = vehicle.VehicleModelId;
            return vehicle;
        }

        private void SetTitle()
        {
            Title = $"{vehicleModel.Name} {Vehicle.Price}€";
        }

        #endregion

        #region Event Subscriptions

        protected override async void OnDeleteExecute()
        {
            if (await vehicleRepository.HasOrderAsync(Vehicle.Id))
            {
                MessageService.ShowInfoDialog(
                    $"An order includes {Title} and therefore this entity cannot be deleted.");
                return;
            }

            bool result = MessageService.ShowConfirmDialog(
                $"Do you wish to delete the vehicle {Title}?");
            if (result)
            {
                vehicleRepository.Remove(Vehicle.Model);
                await vehicleRepository.SaveAsync();
                RaiseDetailDeletedEvent(Vehicle.Id);
            }
        }

        protected override bool OnSaveCanExecute()
        {
            return Vehicle != null
                   && !Vehicle.HasErrors
                   && HasChanges;
        }

        protected override async void OnSaveExecute()
        {
            await SaveWithOptimisticConcurrencyAsync(vehicleRepository.SaveAsync);
            AfterSaveAction();
        }

        #endregion
    }
}