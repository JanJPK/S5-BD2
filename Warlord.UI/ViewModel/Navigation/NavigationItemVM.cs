using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Commands;
using Prism.Events;
using Warlord.UI.Event;

namespace Warlord.UI.ViewModel.Navigation
{
    public class NavigationItemVM : BaseVM
    {
        #region Fields

        private readonly IEventAggregator eventAggregator;
        private string displayMember;
        private readonly string detailViewModelName;

        #endregion

        #region Constructors and Destructors

        public NavigationItemVM(int id, string displayMember, IEventAggregator eventAggregator,
            string detailViewModelName)
        {
            Id = id;
            this.eventAggregator = eventAggregator;
            this.displayMember = displayMember;
            this.detailViewModelName = detailViewModelName;
            OpenDetailViewCommand = new DelegateCommand(OnOpenDetailViewExecute);
        }

        #endregion

        #region Public Properties

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
            eventAggregator.GetEvent<AfterDetailOpenedEvent>().Publish(
                new AfterDetailOpenedEventArgs
                {
                    Id = Id,
                    ViewModelName = detailViewModelName
                });
        }
        
        #endregion
    }
}
