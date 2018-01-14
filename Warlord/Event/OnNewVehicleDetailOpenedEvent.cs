using Prism.Events;

namespace Warlord.Event
{
    public class OnNewVehicleDetailOpenedEvent : PubSubEvent<OnNewVehicleDetailOpenedEventArgs>
    {
    }

    public class OnNewVehicleDetailOpenedEventArgs : AfterDetailOpenedEventArgs
    {
        #region Public Properties

        public int VehicleModelId { get; set; }

        #endregion
    }
}