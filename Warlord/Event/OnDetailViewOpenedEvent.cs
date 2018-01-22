using Prism.Events;

namespace Warlord.Event
{
    /// <summary>
    ///     Published when new detail view is opened.
    /// </summary>
    public class OnDetailViewOpenedEvent : PubSubEvent<OnDetailViewOpenedEventArgs>
    {
    }

    public class OnDetailViewOpenedEventArgs
    {
        public int Id { get; set; }
        public string ViewModelName { get; set; }
    }
}
