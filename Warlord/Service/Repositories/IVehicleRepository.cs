using System.Threading.Tasks;
using Warlord.Model;

namespace Warlord.Service.Repositories
{
    public interface IVehicleRepository : IBaseRepository<Vehicle>
    {
        #region Public Methods and Operators

        Task<bool> HasOrderAsync(int id);

        #endregion
    }
}