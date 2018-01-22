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

            EventAggregator.GetEvent<AfterDetailViewSavedEvent>().Subscribe(AfterDetailSaved);
            EventAggregator.GetEvent<AfterDetailViewDeletedEvent>().Subscribe(AfterDetailDeleted);

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

        public void FilterByDisplayMember()
        {
            if (FilterDisplayMember == "")
            {
                BrowseItemsFiltered = BrowseItems;
                return;
            }

            BrowseItemsFiltered = new ObservableCollection<BrowseItem>(BrowseItems
                .Where(b => b.DisplayMember.ToLower().Contains(FilterDisplayMember.ToLower())).ToList());
        }

        public void FilterById()
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

        public void FilterReset()
        {
            BrowseItemsFiltered = BrowseItems;
        }

        #endregion

        #region Event-related

        protected abstract void AfterDetailDeleted(AfterDetailViewDeletedEventArgs args);

        protected void AfterDetailDeleted(ObservableCollection<BrowseItem> items,
            AfterDetailViewDeletedEventArgs args)
        {
            var item = items.SingleOrDefault(f => f.Id == args.Id);
            if (item != null)
            {
                items.Remove(item);
            }
        }

        protected abstract void AfterDetailSaved(AfterDetailViewSavedEventArgs args);

        protected void AfterDetailSaved(ObservableCollection<BrowseItem> items,
            AfterDetailViewSavedEventArgs args)
        {
            var browseItem = items.SingleOrDefault(l => l.Id == args.Id);
            if (browseItem == null)
            {
                items.Add(new BrowseItem(args.Id, args.DisplayMember, EventAggregator,
                    args.ViewModelName));
            }
            else
            {
                browseItem.DisplayMember = args.DisplayMember;
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

        protected override void AfterSaveAction()
        {
            
        }

        #endregion
    }
}