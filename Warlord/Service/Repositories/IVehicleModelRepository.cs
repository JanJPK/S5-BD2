using System.Threading.Tasks;
using Warlord.Model;

namespace Warlord.Service.Repositories
{
    public interface IVehicleModelRepository : IBaseRepository<VehicleModel>
    {
        Task<bool> HasVehiclesAsync(int id);
    }
}