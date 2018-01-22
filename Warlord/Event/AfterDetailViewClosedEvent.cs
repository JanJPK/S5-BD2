using Prism.Events;

namespace Warlord.Event
{
    /// <summary>
    ///     Published when view is closed.
    /// </summary>
    public class AfterDetailViewClosedEvent : PubSubEvent<AfterDetailClosedEventArgs>
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