using System.ComponentModel;
using System.Runtime.CompilerServices;
using Warlord.UI.Annotations;

namespace Warlord.UI.ViewModel
{
    /// <summary>
    ///     Base class for View Models.
    /// </summary>
    public class BaseVM : INotifyPropertyChanged
    {
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