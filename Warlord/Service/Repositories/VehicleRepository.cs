using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Warlord.DataAccess;
using Warlord.Model;

namespace Warlord.Service.Repositories
{
    public class VehicleRepository : BaseRepository<Vehicle, WarlordDbContext>, IVehicleRepository
    {
        #region Constructors and Destructors

        public VehicleRepository(WarlordDbContext context) : base(context)
        {
        }

        #endregion

        #region Public Methods and Operators

        public override async Task<Vehicle> GetByIdAsync(int id)
        {
            return await Context.Vehicles
                .Include(v => v.VehicleModel)
                .Include(v => v.Order)
                .SingleOrDefaultAsync(v => v.Id == id);
        }

        public async Task<bool> HasOrderAsync(int id)
        {
            return await Context.Orders.AsNoTracking()
                .AnyAsync(o => o.Vehicles.Any(v => v.Id == id));
        }

        public async Task<IEnumerable<Vehicle>> GetAllByOrderAsync(int id)
        {
            return await Context.Set<Vehicle>().Where(v => v.OrderId == id).ToListAsync();
        }

        #endregion
    }
}