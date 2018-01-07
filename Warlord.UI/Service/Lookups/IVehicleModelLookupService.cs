using System.Collections.Generic;
using System.Threading.Tasks;

namespace Warlord.UI.Service.Lookups
{
    public interface IVehicleModelLookupService
    {
        Task<IEnumerable<LookupItem>> GetVehicleModelLookupAsync();
    }
}