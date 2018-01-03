using System.Data.Entity;
using Warlord.Model;

namespace Warlord.DataAccess
{
    public class WarlordDbContext : DbContext
    {
        #region Constructors and Destructors

        public WarlordDbContext() : base("WarlordDb")
        {
        }

        #endregion

        #region Public Properties

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Manufacturer> Manufacturers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<VehicleModel> VehicleModels { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }

        #endregion
    }
}