using System.Threading.Tasks;
using Prism.Events;
using Warlord.Event;
using Warlord.Service;
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
            IUserPrivilege userPrivilege,
            IVehicleLookupService lookupService)
            : base(eventAggregator, messageService, userPrivilege)
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

        #region Event-related

        protected override void AfterDetailDeleted(AfterDetailViewDeletedEventArgs args)
        {
            if (args.ViewModelName == nameof(VehicleDetailVM))
            {
                AfterDetailDeleted(BrowseItems, args);
            }
        }

        protected override async void AfterDetailSaved(AfterDetailViewSavedEventArgs args)
        {
            if (args.ViewModelName == nameof(VehicleDetailVM))
            {
                AfterDetailSaved(BrowseItems, args);
            }
            if (args.ViewModelName == nameof(VehicleModelDetailVM))
            {
                await LoadAsync(Id);
            }
        }

        #endregion
    }
}