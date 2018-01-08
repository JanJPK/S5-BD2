using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Warlord.DataAccess;
using Warlord.Model;

namespace Warlord.Service.Repositories
{
    public class VehicleModelRepository : BaseRepository<VehicleModel, WarlordDbContext>, IVehicleModelRepository
    {
        #region Constructors and Destructors

        public VehicleModelRepository(WarlordDbContext context) : base(context)
        {
        }

        #endregion

        #region Public Methods and Operators

        public VehicleModel GetById(int id)
        {
            return Context.VehicleModels
                .Include(v => v.Manufacturer)
                .Single(v => v.Id == id);
        }

        public override async Task<VehicleModel> GetByIdAsync(int id)
        {
            return await Context.VehicleModels
                .Include(v => v.Manufacturer)
                .SingleAsync(v => v.Id == id);
        }

        public async Task<bool> HasVehiclesAsync(int id)
        {
            return await Context.Vehicles.AsNoTracking()
                .AnyAsync(v => v.VehicleModelId == id);
        }

        #endregion
    }
}