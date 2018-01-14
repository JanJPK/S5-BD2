using System.Collections.Generic;
using System.Threading.Tasks;
using Warlord.Model;

namespace Warlord.Service.Repositories
{
    public interface IVehicleRepository : IBaseRepository<Vehicle>
    {
        #region Public Methods and Operators

        Task<IEnumerable<Vehicle>> GetAllByOrderAsync(int id);

        Task<bool> HasOrderAsync(int id);

        #endregion
    }
}