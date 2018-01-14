using System.Threading.Tasks;
using Prism.Events;
using Warlord.Event;
using Warlord.Service;
using Warlord.Service.Lookups;
using Warlord.Service.Message;

namespace Warlord.ViewModel.Detail.Browse
{
    public class OrderBrowseVM : BaseBrowseVM
    {
        #region Fields

        private readonly IOrderLookupService lookupService;

        #endregion

        #region Constructors and Destructors

        public OrderBrowseVM(IEventAggregator eventAggregator, IMessageService messageService,
            IUserPrivilege userPrivilege,
            IOrderLookupService lookupService)
            : base(eventAggregator, messageService, userPrivilege)
        {
            Title = "Orders";
            this.lookupService = lookupService;
        }

        #endregion

        #region Public Methods and Operators

        public override async Task LoadAsync(int id)
        {
            Id = id;
            BrowseItems.Clear();

            var lookupItems = await lookupService.GetOrderLookupAsync();
            foreach (var item in lookupItems)
            {
                BrowseItems.Add(new BrowseItem(
                    item.Id,
                    item.DisplayMember,
                    EventAggregator,
                    nameof(OrderDetailVM)
                ));
            }
        }

        #endregion

        #region Event-related

        protected override void AfterDetailDeleted(AfterDetailDeletedEventArgs args)
        {
            if (args.ViewModelName == nameof(OrderDetailVM))
            {
                AfterDetailDeleted(BrowseItems, args);
            }
        }

        protected override void AfterDetailSaved(AfterDetailSavedEventArgs args)
        {
            if (args.ViewModelName == nameof(OrderDetailVM))
            {
                AfterDetailSaved(BrowseItems, args);
            }
        }

        #endregion
    }
}