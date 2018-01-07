using System.Threading.Tasks;

namespace Warlord.Service.Message
{
    /// <summary>
    ///     Responsible for displaying messages.
    /// </summary>
    public class MessageService : IMessageService
    {
        #region Public Methods and Operators

        //private MetroWindow MetroWindow => (MetroWindow)App.Current.MainWindow;

        public async void ShowInfoDialogAsync(string text, string title = "Information")
        {
            //await MetroWindow.ShowMessageAsync(title, text);
        }

        public async Task<MessageResult> ShowOkCancelDialogAsync(string text, string title)
        {
            //var result = await MetroWindow.ShowMessageAsync(title, text, MessageDialogStyle.AffirmativeAndNegative);
            //return result == MahApps.Metro.Controls.Dialogs.MessageDialogResult.Affirmative ? MessageResult.OK : MessageResult.Cancel;
            return MessageResult.OK;
        }

        /// <summary>
        ///     Asks the user if he wants to confirm an action or cancel it.
        /// </summary>
        /// <param name="text">Text to display.</param>
        /// <param name="title">Dialog title.</param>
        /// <returns>True when user confirms; false when cancels.</returns>
        public async Task<bool> ShowConfirmDialogAsync(string text, string title)
        {
            //var result = await MetroWindow.ShowMessageAsync(title, text, MessageDialogStyle.AffirmativeAndNegative);
            //return result == MahApps.Metro.Controls.Dialogs.MessageDialogResult.Affirmative;
            return true;
        }

        #endregion
    }

    public enum MessageResult
    {
        OK,
        Cancel
    }
}
