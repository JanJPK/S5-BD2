using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Events;

namespace Warlord.UI.Event
{
    public class AfterBrowseOpenedEvent : PubSubEvent<AfterBrowseOpenedEventArgs>
    {

    }

    public class AfterBrowseOpenedEventArgs
    {
        public string ViewModelName { get; set; }
    }
}
