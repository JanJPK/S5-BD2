using System.Threading.Tasks;
using Warlord.Model;

namespace Warlord.Service.Repositories
{
    public interface IOrderRepository : IBaseRepository<Order>
    {
        #region Public Methods and Operators

        Task<bool> HasVehiclesAsync(int id);

        #endregion
    }
}