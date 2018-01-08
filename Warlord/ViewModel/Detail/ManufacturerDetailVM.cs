using System.Threading.Tasks;
using Prism.Commands;
using Prism.Events;
using Warlord.Model;
using Warlord.Service.Message;
using Warlord.Service.Repositories;
using Warlord.Wrappers;

namespace Warlord.ViewModel.Detail
{
    public class ManufacturerDetailVM : BaseDetailVM
    {
        #region Fields

        private readonly IManufacturerRepository manufacturerRepository;

        private ManufacturerWrapper manufacturer;

        #endregion

        #region Constructors and Destructors

        public ManufacturerDetailVM(IEventAggregator eventAggregator, IMessageService messageService,
            IManufacturerRepository manufacturerRepository)
            : base(eventAggregator, messageService)
        {
            this.manufacturerRepository = manufacturerRepository;
        }

        #endregion

        #region Public Properties

        public ManufacturerWrapper Manufacturer
        {
            get => manufacturer;
            set
            {
                manufacturer = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Public Methods and Operators

        public override async Task LoadAsync(int id)
        {
            var manufacturer = id > 0
                ? await manufacturerRepository.GetByIdAsync(id)
                : CreateNewManufacturer();

            Id = id;

            InitializeManufacturer(manufacturer);
        }

        #endregion

        #region Methods

        private Manufacturer CreateNewManufacturer()
        {
            var manufacturer = new Manufacturer();
            manufacturerRepository.Add(manufacturer);
            return manufacturer;
        }

        private void InitializeManufacturer(Manufacturer manufacturer)
        {
            Manufacturer = new ManufacturerWrapper(manufacturer);
            Manufacturer.PropertyChanged += (s, e) =>
            {
                if (!HasChanges)
                {
                    HasChanges = manufacturerRepository.HasChanges();
                }

                if (e.PropertyName == nameof(Manufacturer.HasErrors))
                {
                    ((DelegateCommand) SaveCommand).RaiseCanExecuteChanged();
                }

                if (e.PropertyName == nameof(Manufacturer.ShortName))
                {
                    SetTitle();
                }
            };

            if (Manufacturer.Id == 0)
            {
                Manufacturer.ShortName = "New manufacturer";
                Manufacturer.FullName = "";
                Manufacturer.Country = "";
            }

            SetTitle();
        }

        private void SetTitle()
        {
            Title = $"{manufacturer.ShortName}";
        }

        #endregion

        #region Event Subscriptions

        protected override async void OnDeleteExecute()
        {
            if (await manufacturerRepository.HasVehicleModelsAsync(Manufacturer.Id))
            {
                MessageService.ShowInfoDialog(
                    $"Vehicle models of {Manufacturer.ShortName} {Manufacturer.FullName} are currently up for sale and therefore this entity cannot be deleted.");
                return;
            }

            bool result = MessageService.ShowConfirmDialog(
                    $"Do you wish to delete the manufacturer {Manufacturer.ShortName} {Manufacturer.FullName}?");
            if (result)
            {
                manufacturerRepository.Remove(Manufacturer.Model);
                await manufacturerRepository.SaveAsync();
                RaiseDetailDeletedEvent(Manufacturer.Id);
            }
        }

        protected override bool OnSaveCanExecute()
        {
            return Manufacturer != null
                   && !Manufacturer.HasErrors
                   && HasChanges;
        }

        protected override async void OnSaveExecute()
        {
            await SaveWithOptimisticConcurrencyAsync(manufacturerRepository.SaveAsync);
            AfterSaveAction();
        }

        private void AfterSaveAction()
        {
            HasChanges = manufacturerRepository.HasChanges();
            Id = manufacturer.Id;
            RaiseDetailSavedEvent(Manufacturer.Id, $"{Title}");
        }

        #endregion
    }
}