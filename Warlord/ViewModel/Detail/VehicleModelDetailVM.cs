using System;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Events;
using Warlord.Event;
using Warlord.Model;
using Warlord.Service.Message;
using Warlord.Service.Repositories;
using Warlord.Wrappers;

namespace Warlord.ViewModel.Detail
{
    public class VehicleModelDetailVM : BaseDetailVM
    {
        #region Fields

        private readonly IVehicleModelRepository vehicleModelRepository;

        private VehicleModelWrapper vehicleModel;

        #endregion

        #region Constructors and Destructors

        public VehicleModelDetailVM(IEventAggregator eventAggregator, IMessageService messageService,
            IVehicleModelRepository vehicleModelRepository)
            : base(eventAggregator, messageService)
        {
            this.vehicleModelRepository = vehicleModelRepository;
        }

        #endregion

        #region Public Properties

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

            InitializeVehicleModel(vehicleModel);
        }

        #endregion

        #region Methods

        protected override async void OnDeleteExecute()
        {
            if (await vehicleModelRepository.HasVehiclesAsync(VehicleModel.Id))
            {
                // TODO: message.
            }

            // TODO: rest of method.
        }

        protected override bool OnSaveCanExecute()
        {
            return VehicleModel != null
                   && !VehicleModel.HasErrors
                   && HasChanges;
        }

        protected override async void OnSaveExecute()
        {
            await SaveWithOptimisticConcurrencyAsync(
                vehicleModelRepository.SaveAsync,
                () =>
                {
                    HasChanges = vehicleModelRepository.HasChanges();
                    Id = vehicleModel.Id;
                    RaiseDetailSavedEvent(VehicleModel.Id, vehicleModel.Name);
                });
        }

        private void AfterCollectionSaved(AfterCollectionSavedEventArgs obj)
        {
            throw new NotImplementedException();
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
                VehicleModel.Name = "";
            }

            SetTitle();
        }

        private void SetTitle()
        {
            Title = $"{vehicleModel.Manufacturer.ShortName} - {vehicleModel.Name}";
        }

        #endregion
    }
}