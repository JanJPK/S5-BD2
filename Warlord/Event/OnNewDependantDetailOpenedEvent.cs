using Prism.Events;

namespace Warlord.Event
{
    public class OnNewDependantDetailOpenedEvent : PubSubEvent<OnNewDependantDetailOpenedEventArgs>
    {
    }

    public class OnNewDependantDetailOpenedEventArgs : AfterDetailOpenedEventArgs
    {
        #region Public Properties

        public int DependantOnId { get; set; }

        public string DependantOnName { get; set; }

        #endregion
    }
}