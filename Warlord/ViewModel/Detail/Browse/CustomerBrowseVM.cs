using System.Threading.Tasks;
using Prism.Events;
using Warlord.Event;
using Warlord.Service.Lookups;
using Warlord.Service.Message;

namespace Warlord.ViewModel.Detail.Browse
{
    public class CustomerBrowseVM : BaseBrowseVM
    {
        #region Fields

        private readonly ICustomerLookupService lookupService;

        #endregion

        #region Constructors and Destructors

        public CustomerBrowseVM(IEventAggregator eventAggregator, IMessageService messageService,
            ICustomerLookupService lookupService)
            : base(eventAggregator, messageService)
        {
            Title = "Customers";
            this.lookupService = lookupService;
        }

        #endregion

        #region Public Methods and Operators

        public override async Task LoadAsync(int id)
        {
            Id = id;
            BrowseItems.Clear();

            var lookupItems = await lookupService.GetCustomerLookupAsync();
            foreach (var item in lookupItems)
            {
                BrowseItems.Add(new BrowseItem(
                    item.Id,
                    item.DisplayMember,
                    EventAggregator,
                    nameof(CustomerDetailVM)
                ));
            }
        }

        #endregion

        #region Methods

        protected override void AfterDetailDeleted(AfterDetailDeletedEventArgs args)
        {
            if (args.ViewModelName == nameof(CustomerDetailVM))
            {
                AfterDetailDeleted(BrowseItems, args);
            }
        }

        protected override void AfterDetailSaved(AfterDetailSavedEventArgs args)
        {
            if (args.ViewModelName == nameof(CustomerDetailVM))
            {
                AfterDetailSaved(BrowseItems, args);
            }
        }

        #endregion
    }
}