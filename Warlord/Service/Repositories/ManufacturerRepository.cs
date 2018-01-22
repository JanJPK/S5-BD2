using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Warlord.DataAccess;
using Warlord.Model;

namespace Warlord.Service.Repositories
{
    public class ManufacturerRepository : BaseRepository<Manufacturer, WarlordDbContext>, IManufacturerRepository
    {
        #region Constructors and Destructors

        public ManufacturerRepository(WarlordDbContext context) : base(context)
        {
        }

        #endregion

        #region Public Methods and Operators

        public override async Task<Manufacturer> GetByIdAsync(int id)
        {
            return await Context.Manufacturers
                .Include(m => m.VehicleModels)
                .SingleAsync(m => m.Id == id);
        }

        public override async Task ReloadAsync(int id)
        {
            var dbEntityEntry = Context.ChangeTracker.Entries<Manufacturer>()
                .SingleOrDefault(db => db.Entity.Id == id);
            if (dbEntityEntry != null)
            {
                await dbEntityEntry.ReloadAsync();
            }
        }

        public async Task<bool> HasVehicleModelsAsync(int id)
        {
            return await Context.VehicleModels.AsNoTracking()
                .AnyAsync(vm => vm.ManufacturerId == id);
        }

        #endregion
    }
}