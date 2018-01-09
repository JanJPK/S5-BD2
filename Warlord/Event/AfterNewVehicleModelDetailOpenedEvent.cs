using Prism.Events;

namespace Warlord.Event
{
    public class AfterNewVehicleModelDetailOpenedEvent : PubSubEvent<AfterNewVehicleModelDetailOpenedEventArgs>
    {
    }

    public class AfterNewVehicleModelDetailOpenedEventArgs : AfterDetailOpenedEventArgs
    {
        #region Public Properties

        public int ManufacturerId { get; set; }

        #endregion
    }
}