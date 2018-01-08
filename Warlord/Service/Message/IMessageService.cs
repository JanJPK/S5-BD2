namespace Warlord.Service.Message
{
    public interface IMessageService
    {
        #region Public Methods and Operators

        bool ShowConfirmDialog(string text, string title = "Question");
        void ShowInfoDialog(string text, string title = "Information");

        #endregion
    }
}