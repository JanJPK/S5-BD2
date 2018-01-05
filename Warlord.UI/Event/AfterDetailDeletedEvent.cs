using Prism.Events;

namespace Warlord.UI.Event
{
    /// <summary>
    ///     Published when entity is deleted using view.
    /// </summary>
    public class AfterDetailDeletedEvent : PubSubEvent<AfterDetailDeletedEventArgs>
    {
    }

    public class AfterDetailDeletedEventArgs
    {
        #region Public Properties

        public int Id { get; set; }
        public string ViewModelName { get; set; }

        #endregion
    }
}