using System.Threading.Tasks;
using Prism.Events;
using Warlord.Event;
using Warlord.Service.Lookups;
using Warlord.Service.Message;

namespace Warlord.ViewModel.Detail.Browse
{
    public class VehicleBrowseVM : BaseBrowseVM
    {
        #region Fields

        private readonly IVehicleLookupService lookupService;

        #endregion

        #region Constructors and Destructors

        public VehicleBrowseVM(IEventAggregator eventAggregator, IMessageService messageService,
            IVehicleLookupService lookupService)
            : base(eventAggregator, messageService)
        {
            Title = "Vehicles";
            this.lookupService = lookupService;
        }

        #endregion

        #region Public Methods and Operators

        public override async Task LoadAsync(int id)
        {
            Id = id;
            BrowseItems.Clear();

            var lookupItems = await lookupService.GetVehicleLookupAsync();
            foreach (var item in lookupItems)
            {
                BrowseItems.Add(new BrowseItem(
                    item.Id,
                    item.DisplayMember,
                    EventAggregator,
                    nameof(VehicleDetailVM)
                ));
            }
        }

        #endregion

        #region Methods

        protected override void AfterDetailDeleted(AfterDetailDeletedEventArgs args)
        {
            if (args.ViewModelName == nameof(VehicleDetailVM))
            {
                AfterDetailDeleted(BrowseItems, args);
            }
        }

        protected override void AfterDetailSaved(AfterDetailSavedEventArgs args)
        {
            if (args.ViewModelName == nameof(VehicleDetailVM))
            {
                AfterDetailSaved(BrowseItems, args);
            }
        }

        #endregion
    }
}