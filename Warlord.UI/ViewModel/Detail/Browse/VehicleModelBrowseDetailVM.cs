using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Prism.Events;
using Warlord.Model;
using Warlord.UI.Event;
using Warlord.UI.Service.Lookups;
using Warlord.UI.Service.Message;

namespace Warlord.UI.ViewModel.Detail.Browse
{
    public class VehicleModelBrowseDetailVM : BaseDetailVM
    {
        #region Fields

        private readonly IVehicleModelLookupService lookupService;

        #endregion

        #region Constructors and Destructors

        public VehicleModelBrowseDetailVM(IEventAggregator eventAggregator, IMessageService messageService,
            IVehicleModelLookupService lookupService)
            : base(eventAggregator, messageService)
        {
            Title = "Vehicle Models";
            this.lookupService = lookupService;

            BrowseItems = new ObservableCollection<BrowseItem>();

            EventAggregator.GetEvent<AfterDetailSavedEvent>().Subscribe(AfterDetailSaved);
            EventAggregator.GetEvent<AfterDetailDeletedEvent>().Subscribe(AfterDetailDeleted);
        }

        #endregion

        #region Public Properties

        public ObservableCollection<BrowseItem> BrowseItems { get; set; }

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

        #region Event Subscriptions

        private void AfterDetailDeleted(AfterDetailDeletedEventArgs args)
        {
        }

        private void AfterDetailDeleted(ObservableCollection<BrowseItem> items,
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
        }

        private void AfterDetailSaved(ObservableCollection<BrowseItem> items,
            AfterDetailSavedEventArgs args)
        {
            var lookupItem = items.SingleOrDefault(l => l.Id == args.Id);
            if (lookupItem == null)
            {
                items.Add(new BrowseItem(args.Id, args.DisplayMember, EventAggregator,
                    args.ViewModelName));
            }
            else
            {
                lookupItem.DisplayMember = args.DisplayMember;
            }
        }

        #endregion


        #region Unused

        protected override void OnDeleteExecute()
        {
            throw new NotImplementedException();
        }

        protected override bool OnSaveCanExecute()
        {
            throw new NotImplementedException();
        }

        protected override void OnSaveExecute()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}