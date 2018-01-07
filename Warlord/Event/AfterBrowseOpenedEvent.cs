using Prism.Events;

namespace Warlord.Event
{
    public class AfterBrowseOpenedEvent : PubSubEvent<AfterBrowseOpenedEventArgs>
    {

    }

    public class AfterBrowseOpenedEventArgs
    {
        public string ViewModelName { get; set; }
    }
}
