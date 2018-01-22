using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Warlord.DataAccess;
using Warlord.Model;

namespace Warlord.Service.Repositories
{
    public class CustomerRepository : BaseRepository<Customer, WarlordDbContext>, ICustomerRepository
    {
        #region Constructors and Destructors

        public CustomerRepository(WarlordDbContext context) : base(context)
        {
        }

        #endregion

        #region Public Methods and Operators

        public override async Task<Customer> GetByIdAsync(int id)
        {
            return await Context.Customers
                .Include(c => c.Orders)
                .SingleAsync(c => c.Id == id);
        }

        public override async Task ReloadAsync(int id)
        {
            var dbEntityEntry = Context.ChangeTracker.Entries<Customer>()
                .SingleOrDefault(db => db.Entity.Id == id);
            if (dbEntityEntry != null)
            {
                await dbEntityEntry.ReloadAsync();
            }
        }

        public async Task<bool> HasOrdersAsync(int id)
        {
            return await Context.Orders.AsNoTracking()
                .AnyAsync(o => o.CustomerId == id);
        }

        #endregion
    }
}