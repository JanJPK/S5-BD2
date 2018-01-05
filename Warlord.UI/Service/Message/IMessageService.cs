using System.Threading.Tasks;

namespace Warlord.UI.Service.Message
{
    public interface IMessageService
    {
        void ShowInfoDialogAsync(string text, string title = "Information");
        Task<MessageResult> ShowOkCancelDialogAsync(string text, string title);
        Task<bool> ShowConfirmDialogAsync(string text, string title);
    }
}