using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Events;

namespace Warlord.UI.Event
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
