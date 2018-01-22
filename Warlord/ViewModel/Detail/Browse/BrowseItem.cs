using System.Windows.Input;
using Prism.Commands;
using Prism.Events;
using Warlord.Event;

namespace Warlord.ViewModel.Detail.Browse
{
    public class BrowseItem : BaseVM
    {
        #region Fields

        private readonly IEventAggregator eventAggregator;
        private string displayMember;

        #endregion

        #region Constructors and Destructors

        public BrowseItem(int id, string displayMember, IEventAggregator eventAggregator,
            string detailViewModelName)
        {
            Id = id;
            this.eventAggregator = eventAggregator;
            this.displayMember = displayMember;
            DetailViewModelName = detailViewModelName;
            OpenDetailViewCommand = new DelegateCommand(OnOpenDetailViewExecute);
        }

        #endregion

        #region Public Properties

        public string DetailViewModelName { get; }

        public string DisplayMember
        {
            get => displayMember;
            set
            {
                displayMember = value;
                OnPropertyChanged();
            }
        }

        public int Id { get; }

        public ICommand OpenDetailViewCommand { get; }

        #endregion

        #region Methods

        private void OnOpenDetailViewExecute()
        {
            eventAggregator.GetEvent<OnDetailViewOpenedEvent>().Publish(
                new OnDetailViewOpenedEventArgs
                {
                    Id = Id,
                    ViewModelName = DetailViewModelName
                });
        }

        #endregion
    }
}