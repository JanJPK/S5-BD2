using Prism.Events;

namespace Warlord.Event
{
    public class AfterNewVehicleDetailOpenedEvent : PubSubEvent<AfterNewVehicleDetailOpenedEventArgs>
    {
    }

    public class AfterNewVehicleDetailOpenedEventArgs : AfterDetailOpenedEventArgs
    {
        #region Public Properties

        public int VehicleModelId { get; set; }

        #endregion
    }
}