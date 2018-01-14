using System;
using System.Collections.ObjectModel;
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
using Warlord.Wrappers;

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

        private async void AfterDetailSaved(AfterDetailSavedEventArgs args)
        {
            if (args.ViewModelName == nameof(CustomerDetailVM))
            {
                InitializeCustomer(await LoadCustomer(CustomerId));
                SetTitle();
            }
        }

        private void AfterSaveAction()
        {
            SetTitle();
            HasChanges = orderRepository.HasChanges();
            Id = Order.Id;
            RaiseDetailSavedEvent(Order.Id, Title);
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

                //if (e.PropertyName == nameof(Vehicle.ShortName))
                //{
                //    SetTitle();
                //}
            };

            if (Order.Id == 0)
            {
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

        private void SetTitle()
        {
            Title = $"{Order.Id}/{Order.Date.Day}-{Order.Date.Month}-{Order.Date.Year} {Customer.Name}";
        }

        #endregion

        #region Event-related

        protected override async void OnDeleteExecute()
        {
            if (await orderRepository.HasVehiclesAsync(Order.Id))
            {
                await MessageService.ShowInfoDialog(
                    $"Order {Title} includes vehiles and therefore this entity cannot be deleted.");
                return;
            }

            bool result = await MessageService.ShowConfirmDialog(
                $"Do you wish to delete the order {Title}?");
            if (result)
            {
                orderRepository.Remove(Order.Model);
                await orderRepository.SaveAsync();
                RaiseDetailDeletedEvent(Order.Id);
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
            await SaveWithOptimisticConcurrencyAsync(orderRepository.SaveAsync);
            await SaveWithOptimisticConcurrencyAsync(vehicleRepository.SaveAsync);

            foreach (var vehicle in VehicleBrowseItems)
            {
                // TODO
                // RaiseDetailSavedEvent(vehicle.Id, vehicle.DisplayMember);
            }

            AfterSaveAction();
            await LoadAsync(Order.Id);
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

                if(vehicleBrowseItems.Any(v => v.Id == VehicleToAddId))
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

        #endregion
    }
}