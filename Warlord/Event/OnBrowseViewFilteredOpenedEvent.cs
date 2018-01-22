using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Events;

namespace Warlord.Event
{
    public class OnBrowseViewFilteredOpenedEvent : PubSubEvent<OnBrowseViewFilteredOpenedEventArgs>
    {
    }

    public class OnBrowseViewFilteredOpenedEventArgs : OnDetailViewOpenedEventArgs
    {
        public string FilterId { get; set; }
        public string FilterDisplayMember { get; set; }
    }
}
