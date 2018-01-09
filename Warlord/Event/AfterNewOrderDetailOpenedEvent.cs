using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Events;

namespace Warlord.Event
{
    public class AfterNewOrderDetailOpenedEvent : PubSubEvent<AfterNewOrderDetailOpenedEventArgs>
    {
    }

    public class AfterNewOrderDetailOpenedEventArgs : AfterDetailOpenedEventArgs
    {
        public int CustomerId { get; set; }
    }
}
