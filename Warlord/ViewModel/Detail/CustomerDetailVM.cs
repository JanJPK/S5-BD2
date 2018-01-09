using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Commands;
using Prism.Events;
using Warlord.Event;
using Warlord.Model;
using Warlord.Service.Message;
using Warlord.Service.Repositories;
using Warlord.Wrappers;

namespace Warlord.ViewModel.Detail
{
    public class CustomerDetailVM : BaseDetailVM
    {
        #region Fields

        private readonly ICustomerRepository customerRepository;

        private CustomerWrapper customer;

        #endregion

        #region Constructors and Destructors

        public CustomerDetailVM(IEventAggregator eventAggregator, IMessageService messageService,
            ICustomerRepository customerRepository)
            : base(eventAggregator, messageService)
        {
            this.customerRepository = customerRepository;

            CreateNewOrderCommand = new DelegateCommand(OnCreateNewOrderExecute, OnCreateNewOrderCanExecute);
        }

        #endregion

        #region Public Properties

        public ICommand CreateNewOrderCommand { get; }

        public CustomerWrapper Customer
        {
            get => customer;
            set
            {
                customer = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Public Methods and Operators

        public override async Task LoadAsync(int id)
        {
            var customer = id > 0
                ? await customerRepository.GetByIdAsync(id)
                : CreateNewCustomer();

            Id = id;

            InitializeCustomer(customer);
        }

        #endregion

        #region Methods

        protected override async void OnDeleteExecute()
        {
            if (await customerRepository.HasOrdersAsync(Customer.Id))
            {
                MessageService.ShowInfoDialog(
                    $"Customer {Customer.Name} has orders and therefore this entity cannot be deleted.");
                return;
            }

            bool result = MessageService.ShowConfirmDialog($"Do you wish to delete the customer {Customer.Name}?");
            if (result)
            {
                customerRepository.Remove(Customer.Model);
                await customerRepository.SaveAsync();
                RaiseDetailDeletedEvent(Customer.Id);
            }
        }

        protected override bool OnSaveCanExecute()
        {
            return Customer != null
                   && !Customer.HasErrors
                   && HasChanges;
        }

        protected override async void OnSaveExecute()
        {
            await SaveWithOptimisticConcurrencyAsync(customerRepository.SaveAsync);
            SetTitle();
            AfterSaveAction();
            await LoadAsync(Customer.Id);
        }

        private void AfterSaveAction()
        {
            HasChanges = customerRepository.HasChanges();
            Id = customer.Id;
            RaiseDetailSavedEvent(Customer.Id, Title);
        }

        private Customer CreateNewCustomer()
        {
            var customer = new Customer();
            customerRepository.Add(customer);
            return customer;
        }

        private void InitializeCustomer(Customer customer)
        {
            Customer = new CustomerWrapper(customer);
            Customer.PropertyChanged += (s, e) =>
            {
                if (!HasChanges)
                {
                    HasChanges = customerRepository.HasChanges();
                }

                if (e.PropertyName == nameof(Customer.HasErrors))
                {
                    ((DelegateCommand) SaveCommand).RaiseCanExecuteChanged();
                }

                //if (e.PropertyName == nameof(VehicleModel.Name))
                //{
                //    SetTitle();
                //}
            };

            if (Customer.Id == 0)
            {
                Customer.Name = "New customer";
                Customer.Country = "";
                Customer.PostalCode = "";
                Customer.Address = "";
                Customer.Email = "";
                Customer.Phone = "";
                Customer.City = "";
            }

            SetTitle();
        }

        private bool OnCreateNewOrderCanExecute()
        {
            return Customer.Id > 0;
        }

        private void OnCreateNewOrderExecute()
        {
            EventAggregator.GetEvent<AfterNewOrderDetailOpenedEvent>().Publish(
                new AfterNewOrderDetailOpenedEventArgs
                {
                    CustomerId = Customer.Id,
                    ViewModelName = nameof(VehicleDetailVM)
                });
        }

        private void SetTitle()
        {
            Title = $"{Customer.Name}";
        }

        #endregion
    }
}