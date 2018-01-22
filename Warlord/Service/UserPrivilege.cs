using System.ComponentModel;
using System.Runtime.CompilerServices;
using Prism.Events;
using Warlord.Properties;

namespace Warlord.Service
{
    public class UserPrivilege : INotifyPropertyChanged, IUserPrivilege
    {
        #region Fields

        private bool loggedIn;
        private bool loggedOut;

        #endregion

        #region Constructors and Destructors

        public UserPrivilege()
        {
            loggedOut = true;
        }

        #endregion

        #region Public Properties

        public bool LoggedIn
        {
            get => loggedIn;
            set
            {
                loggedIn = value;                
                OnPropertyChanged();
            }
        }

        public bool LoggedOut
        {
            get => loggedOut;
            set
            {
                loggedOut = value;                
                OnPropertyChanged();
            }
        }

        #endregion

        #region Public Methods and Operators

        public void LogIn()
        {
            LoggedIn = true;
            LoggedOut = false;
        }

        public void LogOut()
        {
            LoggedIn = false;
            LoggedOut = true;
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