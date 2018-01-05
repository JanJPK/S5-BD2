using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warlord.UI.Service.Lookups
{
    /// <summary>
    ///     Displayed in the navigation list - displays a short summary of an entity and allows to select one.
    /// </summary>
    public class LookupItem
    {
        #region Public Properties

        public string DisplayMember { get; set; }
        public int Id { get; set; }

        #endregion
    }

    /// <summary>
    ///     Allows to choose the default (nothing) value im ComboBox after selecting something else earlier.
    /// </summary>
    public class NullLookupItem : LookupItem
    {
        public new int? Id => null;
    }
}
