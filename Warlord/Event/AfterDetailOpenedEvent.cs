using Prism.Events;

namespace Warlord.Event
{
    /// <summary>
    ///     Published when new detail view is opened.
    /// </summary>
    public class AfterDetailOpenedEvent : PubSubEvent<AfterDetailOpenedEventArgs>
    {
    }

    public class AfterDetailOpenedEventArgs
    {
        public int Id { get; set; }
        public string ViewModelName { get; set; }
    }
}
