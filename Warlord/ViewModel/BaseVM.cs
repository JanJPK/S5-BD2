using System.ComponentModel;
using System.Runtime.CompilerServices;
using Warlord.Annotations;

namespace Warlord.ViewModel
{
    /// <summary>
    ///     Base class for View Models.
    /// </summary>
    public class BaseVM : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged



        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}