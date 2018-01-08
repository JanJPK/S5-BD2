using System.Collections.Generic;
using System.Threading.Tasks;

namespace Warlord.Service.Lookups
{
    public interface IVehicleLookupService
    {
        Task<IEnumerable<LookupItem>> GetVehicleLookupAsync();
    }
}