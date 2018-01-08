using System.Collections.Generic;
using System.Threading.Tasks;

namespace Warlord.Service.Lookups
{
    public interface IOrderLookupService
    {
        Task<IEnumerable<LookupItem>> GetOrderLookupAsync();
    }
}