using System;
using System.Collections.ObjectModel;
using System.Data.Entity.Infrastructure;
using System.Linq;
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
    public class OrderDetailVM : BaseDetailVM
    {
        #region Fields

        private readonly ICustomerRepository customerRepository;
        private readonly IOrderRepository orderRepository;

        private readonly IVehicleLookupService vehicleLookupService;
        private readonly IVehicleRepository vehicleRepository;

        private CustomerWrapper customer;
        private OrderWrapper order;

        private BrowseItem selectedVehicleBrowseItem;
        private ObservableCollection<BrowseItem> vehicleBrowseItems;
        private int vehicleToAddId;

        #endregion

        #region Constructors and Destructors

        public OrderDetailVM(IEventAggregator eventAggregator, IMessageService messageService,
            IUserPrivilege userPrivilege, IOrderRepository orderRepository,
            ICustomerRepository customerRepository, IVehicleRepository vehicleRepository,
            IVehicleLookupService vehicleLookupService)
            : base(eventAggregator, messageService, userPrivilege)
        {
            this.orderRepository = orderRepository;
            this.customerRepository = customerRepository;
            this.vehicleLookupService = vehicleLookupService;
            this.vehicleRepository = vehicleRepository;

            EventAggregator.GetEvent<AfterDetailViewSavedEvent>().Subscribe(AfterDetailViewSaved);

            VehicleBrowseItems = new ObservableCollection<BrowseItem>();
            AddVehicleCommand = new DelegateCommand(OnAddVehicleExecute);
            RemoveVehicleCommand = new DelegateCommand(OnRemoveVehicleExecute, OnRemoveVehicleCanExecute);
        }

        #endregion

        #region Public Properties

        public ICommand AddVehicleCommand { get; }

        public CustomerWrapper Customer
        {
            get => customer;
            set
            {
                customer = value;
                OnPropertyChanged();
            }
        }

        public int CustomerId { get; set; }

        public OrderWrapper Order
        {
            get => order;
            set
            {
                order = value;
                OnPropertyChanged();
            }
        }

        public ICommand RemoveVehicleCommand { get; }

        public BrowseItem SelectedVehicleBrowseItem
        {
            get => selectedVehicleBrowseItem;
            set
            {
                selectedVehicleBrowseItem = value;
                OnPropertyChanged();
                ((DelegateCommand) RemoveVehicleCommand).RaiseCanExecuteChanged();
            }
        }

        public ObservableCollection<BrowseItem> VehicleBrowseItems
        {
            get => vehicleBrowseItems;
            set
            {
                vehicleBrowseItems = value;
                OnPropertyChanged();
            }
        }

        public int VehicleToAddId
        {
            get => vehicleToAddId;
            set
            {
                vehicleToAddId = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Public Methods and Operators

        public override async Task LoadAsync(int id)
        {
            var order = id > 0
                ? await LoadOrder(id)
                : CreateNewOrder();

            var customer = await LoadCustomer(id);
            Id = id;

            InitializeCustomer(customer);
            InitializeOrder(order);
        }

        #endregion

        #region Methods

        protected override async void AfterSaveAction()
        {
            HasChanges = orderRepository.HasChanges();
            Id = order.Id;
            await LoadAsync(Id);
            RaiseDetailViewSavedEvent(Order.Id, Title);
        }

        protected override async void OnDeleteExecute()
        {
            if (await orderRepository.HasVehiclesAsync(Order.Id))
            {
                await MessageService.ShowInfoDialog(
                    $"Order {Title} includes vehicles and therefore this entity cannot be deleted.");
                return;
            }

            bool result = await MessageService.ShowConfirmDialog(
                $"Do you wish to delete the order {Title}?");
            if (result)
            {        
                orderRepository.Remove(Order.Model);
                try
                {
                    await orderRepository.SaveAsync();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    throw;
                }
                
                RaiseDetailViewDeletedEvent(Order.Id);
            }
        }

        protected override bool OnSaveCanExecute()
        {
            return Order != null
                   && !Order.HasErrors
                   && HasChanges;
        }

        protected override async void OnSaveExecute()
        {
            //await SaveWithOptimisticConcurrencyOrderAsync();
            await orderRepository.SaveAsync();
            await vehicleRepository.SaveAsync();
            AfterSaveAction();
        }

        protected async Task SaveWithOptimisticConcurrencyOrderAsync()
        {
            try
            {
                // Saving vehicles because they contain the FK.
                await vehicleRepository.SaveAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                var databaseValues = ex.Entries.Single().GetDatabaseValues();
                if (databaseValues == null)
                {
                    await MessageService.ShowInfoDialog(
                        "The entity has been deleted by another user. Reverting changes.");
                    RaiseDetailViewDeletedEvent(Id);
                }
                else
                {
                    await MessageService.ShowInfoDialog(
                        "The entity has been changed in the meantime by another user. Reverting changes.");
                    await ex.Entries.Single().ReloadAsync();
                }
                await orderRepository.ReloadAsync(Id);
                await LoadAsync(Id);
                return;
            }

            // Saving vehicles succeeded; update all the vehicle VMs that are currently loaded - some will be removed after order reload.
            foreach (var vehicle in Order.Model.Vehicles)
            {
                EventAggregator.GetEvent<AfterDetailViewSavedEvent>()
                    .Publish(new AfterDetailViewSavedEventArgs
                    {
                        Id = vehicle.Id,
                        ViewModelName = nameof(VehicleDetailVM)
                    });
            }

            // Save order.            
            try
            {
                await orderRepository.SaveAsync();
                await orderRepository.ReloadAsync(Id);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                var entry = ex.Entries.Single();
                throw;
            }

            AfterSaveAction();
        }

        private async void AfterDetailViewSaved(AfterDetailViewSavedEventArgs args)
        {
            if (args.Id == CustomerId && args.ViewModelName == nameof(CustomerDetailVM))
            {
                await customerRepository.ReloadAsync(CustomerId);
                await LoadAsync(Id);
            }
            if (args.ViewModelName == nameof(VehicleDetailVM))
            {
                var browseItem = vehicleBrowseItems.SingleOrDefault(l => l.Id == args.Id);
                if (browseItem != null)
                    browseItem.DisplayMember = args.DisplayMember;
            }
        }

        private Order CreateNewOrder()
        {
            var order = new Order
            {
                CustomerId = CustomerId,
                Date = DateTime.Today
            };
            orderRepository.Add(order);
            return order;
        }

        private void InitializeCustomer(Customer model)
        {
            customer = new CustomerWrapper(model);
        }

        private void InitializeOrder(Order order)
        {
            Order = new OrderWrapper(order);
            Order.PropertyChanged += (s, e) =>
            {
                if (!HasChanges)
                {
                    HasChanges = orderRepository.HasChanges();
                }

                if (e.PropertyName == nameof(Order.HasErrors))
                {
                    ((DelegateCommand) SaveCommand).RaiseCanExecuteChanged();
                }

                if (e.PropertyName == nameof(Order.Date))
                {
                    SetTitle();
                }
            };

            if (Order.Id == 0)
            {
                orderRepository.SaveAsync();
            }

            SetTitle();
        }

        private async Task<Customer> LoadCustomer(int id)
        {
            return await customerRepository.GetByIdAsync(CustomerId);
        }

        private async Task<Order> LoadOrder(int id)
        {
            var order = await orderRepository.GetByIdAsync(id);
            CustomerId = order.CustomerId;
            LoadVehicles(order.Id);
            return order;
        }

        private async void LoadVehicles(int id)
        {
            VehicleBrowseItems.Clear();
            var lookupItems = await vehicleLookupService.GetVehicleLookupByOrderAsync(id);
            foreach (var item in lookupItems)
            {
                VehicleBrowseItems.Add(new BrowseItem(
                    item.Id,
                    item.DisplayMember,
                    EventAggregator,
                    nameof(VehicleDetailVM)
                ));
            }
        }

        private async void OnAddVehicleExecute()
        {
            var vehicle = await vehicleRepository.GetByIdAsync(VehicleToAddId);
            if (vehicle != null)
            {
                if (vehicle.Order != null)
                {
                    await MessageService.ShowInfoDialog("Vehicle with given ID is in another order.");
                    return;
                }

                if (vehicleBrowseItems.Any(v => v.Id == VehicleToAddId))
                {
                    await MessageService.ShowInfoDialog("Vehicle with given ID is already in this order.");
                    return;
                }

                //Order.Model.Vehicles.Add(vehicle);
                vehicle.OrderId = Order.Id;
                Order.TotalPrice += vehicle.Price;
                HasChanges = orderRepository.HasChanges();
                VehicleBrowseItems.Add(new BrowseItem(
                    vehicle.Id,
                    $"{vehicle.VehicleModel.Name} {vehicle.Price}€",
                    EventAggregator,
                    nameof(VehicleDetailVM)
                ));
                ((DelegateCommand) SaveCommand).RaiseCanExecuteChanged();
            }
            else
            {
                await MessageService.ShowInfoDialog("Vehicle with given ID could not be found.");
            }
        }

        private bool OnRemoveVehicleCanExecute()
        {
            return SelectedVehicleBrowseItem != null;
        }

        private async void OnRemoveVehicleExecute()
        {
            var vehicle = await vehicleRepository.GetByIdAsync(SelectedVehicleBrowseItem.Id);

            //Order.Model.Vehicles.Remove(vehicle);
            vehicle.OrderId = null;
            Order.TotalPrice -= vehicle.Price;
            HasChanges = orderRepository.HasChanges();
            VehicleBrowseItems.Remove(SelectedVehicleBrowseItem);
            ((DelegateCommand) SaveCommand).RaiseCanExecuteChanged();
        }

        private void SetTitle()
        {
            Title = $"{Order.Id}/{Order.Date.Day}-{Order.Date.Month}-{Order.Date.Year} {Customer.Name}";
        }

        #endregion
    }
}