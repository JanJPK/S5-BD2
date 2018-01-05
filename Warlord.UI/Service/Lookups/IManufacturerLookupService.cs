using System.Collections.Generic;
using System.Threading.Tasks;

namespace Warlord.UI.Service.Lookups
{
    public interface IManufacturerLookupService
    {
        #region Public Methods and Operators

        Task<IEnumerable<LookupItem>> GetManufacturerLookupAsync();

        #endregion
    }
}