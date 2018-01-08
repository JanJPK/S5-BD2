using System.Threading.Tasks;
using Warlord.Model;

namespace Warlord.Service.Repositories
{
    public interface IManufacturerRepository : IBaseRepository<Manufacturer>
    {
        #region Public Methods and Operators

        Task<bool> HasVehicleModelsAsync(int id);

        #endregion
    }
}