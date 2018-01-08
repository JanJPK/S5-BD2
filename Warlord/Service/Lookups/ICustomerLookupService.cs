using System.Collections.Generic;
using System.Threading.Tasks;

namespace Warlord.Service.Lookups
{
    public interface ICustomerLookupService
    {
        Task<IEnumerable<LookupItem>> GetCustomerLookupAsync();
    }
}