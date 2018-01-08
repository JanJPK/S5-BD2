using System.Data.Entity;
using System.Threading.Tasks;
using Warlord.DataAccess;
using Warlord.Model;

namespace Warlord.Service.Repositories
{
    public class OrderRepository : BaseRepository<Order, WarlordDbContext>, IOrderRepository
    {
        #region Constructors and Destructors

        public OrderRepository(WarlordDbContext context) : base(context)
        {
        }

        #endregion

        #region Public Methods and Operators

        public override async Task<Order> GetByIdAsync(int id)
        {
            return await Context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Vehicles)
                .SingleAsync(o => o.Id == id);
        }

        public async Task<bool> HasVehiclesAsync(int id)
        {
            return await Context.Vehicles.AsNoTracking()
                .AnyAsync(v => v.OrderId == id);
        }

        #endregion
    }
}