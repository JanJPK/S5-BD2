using System.Threading.Tasks;
using Warlord.Model;

namespace Warlord.Service.Repositories
{
    public interface ICustomerRepository : IBaseRepository<Customer>
    {
        #region Public Methods and Operators

        Task<bool> HasOrdersAsync(int id);

        #endregion
    }
}