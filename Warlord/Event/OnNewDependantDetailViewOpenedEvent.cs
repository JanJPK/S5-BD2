using Prism.Events;

namespace Warlord.Event
{
    public class OnNewDependantDetailViewOpenedEvent : PubSubEvent<OnNewDependantDetailViewOpenedEventArgs>
    {
    }

    public class OnNewDependantDetailViewOpenedEventArgs : OnDetailViewOpenedEventArgs
    {
        #region Public Properties

        public int DependantOnId { get; set; }

        public string DependantOnName { get; set; }

        #endregion
    }
}