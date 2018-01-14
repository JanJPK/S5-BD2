using System.Threading.Tasks;
using System.Windows;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace Warlord.Service.Message
{
    /// <summary>
    ///     Responsible for displaying messages.
    /// </summary>
    public class MessageService : IMessageService
    {
        #region Properties

        private MetroWindow MetroWindow => (MetroWindow) Application.Current.MainWindow;

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Asks the user if he wants to confirm an action or cancel it.
        /// </summary>
        /// <param name="text">Text to display.</param>
        /// <param name="title">Dialog title.</param>
        /// <returns>True when user confirms; false when cancels.</returns>
        public async Task<bool> ShowConfirmDialog(string text, string title = "Question")
        {
            var result = await MetroWindow.ShowMessageAsync(title, text, MessageDialogStyle.AffirmativeAndNegative);
            if (result == MessageDialogResult.Affirmative)
                return true;

            return false;
        }

        /// <summary>
        ///     Displays a message.
        /// </summary>
        /// <param name="text">Text to display.</param>
        /// <param name="title">Dialog title.</param>
        public async Task ShowInfoDialog(string text, string title = "Information")
        {
            await MetroWindow.ShowMessageAsync(title, text);
        }

        #endregion
    }
}