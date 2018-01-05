using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Prism.Events;
using Warlord.UI.Event;
using Warlord.UI.Service.Lookups;
using Warlord.UI.ViewModel.Detail;

namespace Warlord.UI.ViewModel.Navigation
{
    public class NavigationVM
    {
        #region Fields

        private readonly IEventAggregator eventAggregator;

        private readonly IOrderLookupService orderLookupService;
        private readonly IVehicleLookupService vehicleLookupService;
        private readonly IManufacturerLookupService manufacturerLookupService;

        #endregion

        #region Constructors and Destructors

        public NavigationVM(IOrderLookupService orderLookupService,
            IVehicleLookupService vehicleLookupService, 
            IManufacturerLookupService manufacturerLookupService,
            IEventAggregator eventAggregator)
        {
            this.orderLookupService = orderLookupService;
            this.vehicleLookupService = vehicleLookupService;
            this.manufacturerLookupService = manufacturerLookupService;
            this.eventAggregator = eventAggregator;

            Orders = new ObservableCollection<NavigationItemVM>();
            Vehicles = new ObservableCollection<NavigationItemVM>();

            eventAggregator.GetEvent<AfterDetailSavedEvent>().Subscribe(AfterDetailSaved);
            eventAggregator.GetEvent<AfterDetailDeletedEvent>().Subscribe(AfterDetailDeleted);
        }

        #endregion

        #region Public Properties

        public ObservableCollection<NavigationItemVM> Manufacturers { get; }
        public ObservableCollection<NavigationItemVM> Vehicles { get; }
        public ObservableCollection<NavigationItemVM> Orders { get; }

        #endregion

        #region Public Methods and Operators

        public async Task LoadAsync()
        {

            var lookup = await manufacturerLookupService.GetManufacturerLookupAsync();
            Manufacturers.Clear();
            foreach (var item in lookup)
            {
                Manufacturers.Add(new NavigationItemVM(item.Id, item.DisplayMember, eventAggregator,
                    nameof(ManufacturerDetailVM)));
            }

            lookup = await vehicleLookupService.GetVehicleLookupAsync();
            Vehicles.Clear();
            foreach (var item in lookup)
            {
                Vehicles.Add(new NavigationItemVM(item.Id, item.DisplayMember, eventAggregator,
                    nameof(VehicleDetailVM)));
            }

            lookup = await orderLookupService.GetOrderLookupAsync();
            Orders.Clear();
            foreach (var item in lookup)
            {
                Orders.Add(new NavigationItemVM(item.Id, item.DisplayMember, eventAggregator,
                    nameof(OrderDetailVM)));
            }
        }

        #endregion

        #region Events

        private void AfterDetailDeleted(AfterDetailDeletedEventArgs args)
        {
            switch (args.ViewModelName)
            {
                case nameof(ManufacturerDetailVM):
                {
                    AfterDetailDeleted(Manufacturers, args);
                    break;
                }

                case nameof(VehicleDetailVM):
                {
                    AfterDetailDeleted(Vehicles, args);
                    break;
                }

                case nameof(OrderDetailVM):
                {
                    AfterDetailDeleted(Orders, args);
                    break;
                }
            }
        }

        private void AfterDetailDeleted(ObservableCollection<NavigationItemVM> items,
            AfterDetailDeletedEventArgs args)
        {
            var item = items.SingleOrDefault(f => f.Id == args.Id);
            if (item != null)
            {
                items.Remove(item);
            }
        }

        private void AfterDetailSaved(AfterDetailSavedEventArgs args)
        {
            switch (args.ViewModelName)
            {
                case nameof(ManufacturerDetailVM):
                {
                    AfterDetailSaved(Manufacturers, args);
                    break;
                }

                case nameof(VehicleDetailVM):
                {
                    AfterDetailSaved(Vehicles, args);
                    break;
                }

                case nameof(OrderDetailVM):
                {
                    AfterDetailSaved(Orders, args);
                    break;
                }
            }
        }

        private void AfterDetailSaved(ObservableCollection<NavigationItemVM> items,
            AfterDetailSavedEventArgs args)
        {
            var lookupItem = items.SingleOrDefault(l => l.Id == args.Id);
            if (lookupItem == null)
            {
                items.Add(new NavigationItemVM(args.Id, args.DisplayMember, eventAggregator,
                    args.ViewModelName));
            }
            else
            {
                lookupItem.DisplayMember = args.DisplayMember;
            }
        }

        #endregion
    }
}
