using System.Threading.Tasks;

namespace Warlord.ViewModel.Detail
{
    public interface IDetailVM
    {
        #region Public Properties

        bool HasChanges { get; }

        int Id { get; }

        #endregion

        #region Public Methods and Operators

        Task LoadAsync(int id);

        #endregion
    }
}