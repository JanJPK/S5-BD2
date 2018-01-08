using System.Windows;

namespace Warlord.Service.Message
{
    /// <summary>
    ///     Responsible for displaying messages.
    /// </summary>
    public class MessageService : IMessageService
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Asks the user if he wants to confirm an action or cancel it.
        /// </summary>
        /// <param name="text">Text to display.</param>
        /// <param name="title">Dialog title.</param>
        /// <returns>True when user confirms; false when cancels.</returns>
        public bool ShowConfirmDialog(string text, string title = "Question")
        {
            //var result = await MetroWindow.ShowMessageAsync(title, text, MessageDialogStyle.AffirmativeAndNegative);
            //return result == MahApps.Metro.Controls.Dialogs.MessageDialogResult.Affirmative;
            var result = MessageBox.Show(text, title, MessageBoxButton.OKCancel);
            return result == MessageBoxResult.OK;
        }

        /// <summary>
        ///     Displays a message.
        /// </summary>
        /// <param name="text">Text to display.</param>
        /// <param name="title">Dialog title.</param>
        public void ShowInfoDialog(string text, string title = "Information")
        {
            MessageBox.Show(text, title);
        }

        #endregion
    }
}