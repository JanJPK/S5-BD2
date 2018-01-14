using System.Threading.Tasks;
using Prism.Events;
using Warlord.Event;
using Warlord.Service;
using Warlord.Service.Lookups;
using Warlord.Service.Message;

namespace Warlord.ViewModel.Detail.Browse
{
    public class VehicleModelBrowseVM : BaseBrowseVM
    {
        #region Fields

        private readonly IVehicleModelLookupService lookupService;

        #endregion

        #region Constructors and Destructors

        public VehicleModelBrowseVM(IEventAggregator eventAggregator, IMessageService messageService,
            IUserPrivilege userPrivilege,
            IVehicleModelLookupService lookupService)
            : base(eventAggregator, messageService, userPrivilege)
        {
            Title = "Vehicle models";
            this.lookupService = lookupService;
        }

        #endregion

        #region Public Methods and Operators

        public override async Task LoadAsync(int id)
        {
            Id = id;
            BrowseItems.Clear();

            var lookupItems = await lookupService.GetVehicleModelLookupAsync();
            foreach (var item in lookupItems)
            {
                BrowseItems.Add(new BrowseItem(
                    item.Id,
                    item.DisplayMember,
                    EventAggregator,
                    nameof(VehicleModelDetailVM)
                ));
            }
        }

        #endregion

        #region Event-related

        protected override void AfterDetailDeleted(AfterDetailDeletedEventArgs args)
        {
            if (args.ViewModelName == nameof(VehicleModelDetailVM))
            {
                AfterDetailDeleted(BrowseItems, args);
            }
        }

        protected override void AfterDetailSaved(AfterDetailSavedEventArgs args)
        {
            if (args.ViewModelName == nameof(VehicleModelDetailVM))
            {
                AfterDetailSaved(BrowseItems, args);
            }
        }

        #endregion
    }
}