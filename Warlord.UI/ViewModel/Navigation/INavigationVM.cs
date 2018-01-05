using System.Threading.Tasks;

namespace Warlord.UI.ViewModel.Navigation
{
    public interface INavigationVM
    {
        #region Public Methods and Operators

        Task LoadAsync();

        #endregion
    }
}