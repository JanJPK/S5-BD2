using Prism.Events;

namespace Warlord.UI.Event
{
    /// <summary>
    ///     Published when view is closed.
    /// </summary>
    public class AfterDetailClosedEvent : PubSubEvent<AfterDetailClosedEventArgs>
    {
    }

    public class AfterDetailClosedEventArgs
    {
        #region Public Properties

        public int Id { get; set; }
        public string ViewModelName { get; set; }

        #endregion
    }
}