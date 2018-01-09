using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Events;
using Warlord.Event;
using Warlord.Service;
using Warlord.Service.Lookups;
using Warlord.Service.Message;

namespace Warlord.ViewModel.Detail.Browse
{
    public abstract class BaseBrowseVM : BaseDetailVM
    {
        #region Constructors and Destructors

        protected BaseBrowseVM(IEventAggregator eventAggregator, IMessageService messageService, IUserPrivilege userPrivilege)
            : base(eventAggregator, messageService, userPrivilege)
        {
            BrowseItems = new ObservableCollection<BrowseItem>();

            EventAggregator.GetEvent<AfterDetailSavedEvent>().Subscribe(AfterDetailSaved);
            EventAggregator.GetEvent<AfterDetailDeletedEvent>().Subscribe(AfterDetailDeleted);
        }

        #endregion

        #region Public Properties

        public ObservableCollection<BrowseItem> BrowseItems { get; set; }

        #endregion

        #region Public Methods and Operators

        public abstract override Task LoadAsync(int id);

        #endregion

        #region Event Subscriptions

        protected abstract void AfterDetailDeleted(AfterDetailDeletedEventArgs args);

        protected void AfterDetailDeleted(ObservableCollection<BrowseItem> items,
            AfterDetailDeletedEventArgs args)
        {
            var item = items.SingleOrDefault(f => f.Id == args.Id);
            if (item != null)
            {
                items.Remove(item);
            }
        }

        protected abstract void AfterDetailSaved(AfterDetailSavedEventArgs args);

        protected void AfterDetailSaved(ObservableCollection<BrowseItem> items,
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
