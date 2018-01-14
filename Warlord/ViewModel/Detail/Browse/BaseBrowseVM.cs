using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Commands;
using Prism.Events;
using Warlord.Event;
using Warlord.Service;
using Warlord.Service.Message;

namespace Warlord.ViewModel.Detail.Browse
{
    public abstract class BaseBrowseVM : BaseDetailVM
    {
        #region Fields

        private ObservableCollection<BrowseItem> browseItemsFiltered;

        private string filterDisplayMember;
        private string filterId;

        #endregion

        #region Constructors and Destructors

        protected BaseBrowseVM(IEventAggregator eventAggregator, IMessageService messageService,
            IUserPrivilege userPrivilege)
            : base(eventAggregator, messageService, userPrivilege)
        {
            BrowseItems = new ObservableCollection<BrowseItem>();
            BrowseItemsFiltered = BrowseItems;

            EventAggregator.GetEvent<AfterDetailSavedEvent>().Subscribe(AfterDetailSaved);
            EventAggregator.GetEvent<AfterDetailDeletedEvent>().Subscribe(AfterDetailDeleted);

            FilterByDisplayMemberCommand = new DelegateCommand(FilterByDisplayMember);
            FilterByIdCommand = new DelegateCommand(FilterById);
            FilterResetCommand = new DelegateCommand(FilterReset);
        }

        #endregion

        #region Public Properties

        public ObservableCollection<BrowseItem> BrowseItems { get; set; }

        public ObservableCollection<BrowseItem> BrowseItemsFiltered
        {
            get => browseItemsFiltered;
            set
            {
                browseItemsFiltered = value;
                OnPropertyChanged();
            }
        }

        public string FilterDisplayMember
        {
            get => filterDisplayMember;
            set
            {
                filterDisplayMember = value;
                OnPropertyChanged();
            }
        }

        public string FilterId
        {
            get => filterId;
            set
            {
                filterId = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Properties

        public ICommand FilterByDisplayMemberCommand { get; }

        public ICommand FilterByIdCommand { get; }

        public ICommand FilterResetCommand { get; }

        #endregion

        #region Public Methods and Operators

        public abstract override Task LoadAsync(int id);

        #endregion

        #region Methods

        protected void FilterByDisplayMember()
        {
            if (FilterDisplayMember == "")
            {
                BrowseItemsFiltered = BrowseItems;
                return;
            }

            BrowseItemsFiltered = new ObservableCollection<BrowseItem>(BrowseItems
                .Where(b => b.DisplayMember.ToLower().Contains(FilterDisplayMember.ToLower())).ToList());
        }

        protected void FilterById()
        {
            if (FilterId == "")
            {
                BrowseItemsFiltered = BrowseItems;
                return;
            }
                
            int id;
            if (int.TryParse(FilterId, out id))
            {
                BrowseItemsFiltered = new ObservableCollection<BrowseItem>(BrowseItemsFiltered
                    .Where(b => b.Id == id).ToList());
            }
        }

        protected void FilterReset()
        {
            BrowseItemsFiltered = BrowseItems;
        }

        #endregion

        #region Event-related

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