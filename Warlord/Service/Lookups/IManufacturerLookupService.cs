using System.Collections.Generic;
using System.Threading.Tasks;

namespace Warlord.Service.Lookups
{
    public interface IManufacturerLookupService
    {
        Task<IEnumerable<LookupItem>> GetManufacturerLookupAsync();
    }
}