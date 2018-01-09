using System.Collections.Generic;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Events;
using Warlord.Model;
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

        private readonly IOrderRepository orderRepository;
        private readonly IVehicleLookupService vehicleLookupService;

        private OrderWrapper order;

        #endregion

        #region Constructors and Destructors

        public OrderDetailVM(IEventAggregator eventAggregator, IMessageService messageService,
            IOrderRepository orderRepository, IVehicleLookupService vehicleLookupService)
            : base(eventAggregator, messageService)
        {
            this.vehicleLookupService = vehicleLookupService;
            this.orderRepository = orderRepository;
        }

        #endregion

        #region Public Properties

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

        public ICollection<BrowseItem> Vehicles { get; set; }

        #endregion

        #region Public Methods and Operators

        public override async Task LoadAsync(int id)
        {
            var order = id > 0
                ? await LoadOrder(id)
                : CreateNewOrder();

            Id = id;

            InitializeOrder(order);
        }

        #endregion

        #region Methods

        private void AfterSaveAction()
        {
            HasChanges = orderRepository.HasChanges();
            Id = Order.Id;
            RaiseDetailSavedEvent(Order.Id, $"{Title}");
        }

        private Order CreateNewOrder()
        {
            var order = new Order {CustomerId = CustomerId};
            orderRepository.Add(order);
            return order;
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

        private async Task<Order> LoadOrder(int id)
        {
            var order = await orderRepository.GetByIdAsync(id);
            CustomerId = order.CustomerId;
            return order;
        }

        private void SetTitle()
        {
            Title = $"{Order.Id}/{Order.Date} {Order.Customer.Name}";
        }

        #endregion

        #region Event Subscriptions

        protected override async void OnDeleteExecute()
        {
            if (await orderRepository.HasVehiclesAsync(Order.Id))
            {
                MessageService.ShowInfoDialog(
                    $"Order {Title} includes vehiles and therefore this entity cannot be deleted.");
                return;
            }

            bool result = MessageService.ShowConfirmDialog(
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
            AfterSaveAction();
        }

        #endregion
    }
}