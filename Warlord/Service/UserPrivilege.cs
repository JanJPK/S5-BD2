using System.ComponentModel;
using System.Runtime.CompilerServices;
using Prism.Events;
using Warlord.Event;
using Warlord.Properties;

namespace Warlord.Service
{
    public class UserPrivilege : INotifyPropertyChanged, IUserPrivilege
    {
        #region Fields

        private readonly IEventAggregator eventAggregator;
        private bool loggedIn;

        #endregion

        #region Constructors and Destructors

        public UserPrivilege(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
            this.eventAggregator.GetEvent<OnUserLoggedInEvent>().Subscribe(UserLoggedIn);
            this.eventAggregator.GetEvent<OnUserLoggedOutEvent>().Subscribe(UserLoggedOut);
        }

        #endregion

        #region Public Properties

        public bool LoggedIn
        {
            get => loggedIn;
            set
            {
                loggedIn = value;
                //PublishEvent();
                OnPropertyChanged();
            }
        }

        #endregion

        #region Methods

        private void PublishEvent()
        {
            if (LoggedIn)
            {
                eventAggregator.GetEvent<OnUserLoggedInEvent>().Publish();
            }
            else
            {
                eventAggregator.GetEvent<OnUserLoggedOutEvent>().Publish();
            }
        }

        private void UserLoggedIn()
        {
            LoggedIn = true;
        }

        private void UserLoggedOut()
        {
            LoggedIn = false;
        }

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}