using System.Threading.Tasks;
using Prism.Events;
using Warlord.Event;
using Warlord.Service;
using Warlord.Service.Lookups;
using Warlord.Service.Message;

namespace Warlord.ViewModel.Detail.Browse
{
    public class ManufacturerBrowseVM : BaseBrowseVM
    {
        #region Fields

        private readonly IManufacturerLookupService lookupService;

        #endregion

        #region Constructors and Destructors

        public ManufacturerBrowseVM(IEventAggregator eventAggregator, IMessageService messageService,
            IUserPrivilege userPrivilege,
            IManufacturerLookupService lookupService)
            : base(eventAggregator, messageService, userPrivilege)
        {
            Title = "Manufacturers";
            this.lookupService = lookupService;
        }

        #endregion

        #region Public Methods and Operators

        public override async Task LoadAsync(int id)
        {
            Id = id;
            BrowseItems.Clear();

            var lookupItems = await lookupService.GetManufacturerLookupAsync();
            foreach (var item in lookupItems)
            {
                BrowseItems.Add(new BrowseItem(
                    item.Id,
                    item.DisplayMember,
                    EventAggregator,
                    nameof(ManufacturerDetailVM)
                ));
            }
        }

        #endregion

        #region Event-related

        protected override void AfterDetailDeleted(AfterDetailViewDeletedEventArgs args)
        {
            if (args.ViewModelName == nameof(ManufacturerDetailVM))
            {
                AfterDetailDeleted(BrowseItems, args);
            }
        }

        protected override void AfterDetailSaved(AfterDetailViewSavedEventArgs args)
        {
            if (args.ViewModelName == nameof(ManufacturerDetailVM))
            {
                AfterDetailSaved(BrowseItems, args);
            }
        }

        #endregion
    }
}