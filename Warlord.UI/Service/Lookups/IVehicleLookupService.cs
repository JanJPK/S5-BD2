using System.Collections.Generic;
using System.Threading.Tasks;

namespace Warlord.UI.Service.Lookups
{
    public interface IVehicleLookupService
    {
        #region Public Methods and Operators

        Task<IEnumerable<LookupItem>> GetVehicleLookupAsync();

        #endregion
    }
}