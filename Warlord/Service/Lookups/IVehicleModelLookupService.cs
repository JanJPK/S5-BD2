using System.Collections.Generic;
using System.Threading.Tasks;

namespace Warlord.Service.Lookups
{
    public interface IVehicleModelLookupService
    {
        Task<IEnumerable<LookupItem>> GetVehicleModelLookupAsync();
    }
}