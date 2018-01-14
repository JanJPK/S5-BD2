using System.Threading.Tasks;

namespace Warlord.Service.Message
{
    public interface IMessageService
    {
        #region Public Methods and Operators

        Task<bool> ShowConfirmDialog(string text, string title = "Question");
        Task ShowInfoDialog(string text, string title = "Information");

        #endregion
    }
}