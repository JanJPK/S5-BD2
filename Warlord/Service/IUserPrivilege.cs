using System.ComponentModel;

namespace Warlord.Service
{
    public interface IUserPrivilege
    {
        bool LoggedIn { get; set; }
        bool LoggedOut { get; set; }

        event PropertyChangedEventHandler PropertyChanged;

        void LogIn();
        void LogOut();
    }
}