using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Warlord.DataAccess;

namespace Warlord.Service.Lookups
{
    public class LookupService : IVehicleModelLookupService, ICustomerLookupService,
        IManufacturerLookupService, IOrderLookupService, IVehicleLookupService
    {
        #region Fields

        private readonly Func<WarlordDbContext> contextCreator;

        #endregion

        #region Constructors and Destructors

        public LookupService(Func<WarlordDbContext> contextCreator)
        {
            this.contextCreator = contextCreator;
        }

        #endregion

        #region Public Methods and Operators

        public async Task<IEnumerable<LookupItem>> GetCustomerLookupAsync()
        {
            using (var ctx = contextCreator())
            {
                return await ctx.Customers.AsNoTracking()
                    .Select(c =>
                        new LookupItem
                        {
                            Id = c.Id,
                            DisplayMember = c.Name
                        })
                    .ToListAsync();
            }
        }

        public async Task<IEnumerable<LookupItem>> GetManufacturerLookupAsync()
        {
            using (var ctx = contextCreator())
            {
                return await ctx.Manufacturers.AsNoTracking()
                    .Select(m =>
                        new LookupItem
                        {
                            Id = m.Id,
                            DisplayMember = m.ShortName
                        })
                    .ToListAsync();
            }
        }

        public async Task<IEnumerable<LookupItem>> GetOrderLookupAsync()
        {
            using (var ctx = contextCreator())
            {
                return await ctx.Orders.AsNoTracking()
                    .Select(o =>
                        new LookupItem
                        {
                            Id = o.Id,
                            DisplayMember = o.Id + "/" + o.Date + " " + o.Customer.Name
                        })
                    .ToListAsync();
            }
        }

        public async Task<IEnumerable<LookupItem>> GetVehicleLookupAsync()
        {
            using (var ctx = contextCreator())
            {
                return await ctx.Vehicles.AsNoTracking()
                    .Select(v =>
                        new LookupItem
                        {
                            Id = v.Id,
                            DisplayMember = v.VehicleModel.Name + " " + v.Price + "€"
                        })
                    .ToListAsync();
            }
        }

        public async Task<IEnumerable<LookupItem>> GetVehicleModelLookupAsync()
        {
            using (var ctx = contextCreator())
            {
                return await ctx.VehicleModels.AsNoTracking()
                    .Select(vm =>
                        new LookupItem
                        {
                            Id = vm.Id,
                            DisplayMember = vm.Manufacturer.ShortName + " " +  vm.Name
                        })
                    .ToListAsync();
            }
        }

        #endregion
    }
}