using System.Threading.Tasks;
using Warlord.Model;

namespace Warlord.UI.Service.Repositories
{
    public interface IVehicleModelRepository : IBaseRepository<VehicleModel>
    {
        Task<bool> HasVehiclesAsync(int id);
    }
}