using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Warlord.DataAccess;

namespace Warlord.UI.Service.Lookups
{
    /// <summary>
    ///     Creates lists that include LookupItems - short summaries of entities meant to be placed in clickable lists that trigger creation of new view including detail for chosen entity.
    /// </summary>
    public class LookupService
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

        public async Task<IEnumerable<LookupItem>> GetOrderLookupAsync()
        {
            using (var ctx = contextCreator())
            {
                return await ctx.Orders.AsNoTracking()
                    .Select(o =>
                        new LookupItem
                        {
                            Id = o.Id,
                            DisplayMember = o.Id + " / " + o.Date + " / " + o.Customer.Name
                        })
                    .ToListAsync();
            }
        }

        public async Task<List<LookupItem>> GetManufacturerLookupAsync()
        {
            using (var ctx = contextCreator())
            {
                var items = await ctx.Manufacturers.AsNoTracking()
                    .Select(m =>
                        new LookupItem
                        {
                            Id = m.Id,
                            DisplayMember = m.ShortName
                        })
                    .ToListAsync();
                return items;
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
                            DisplayMember = v.VehicleModel.Name + " / " + v.Price
                        })
                    .ToListAsync();
            }
        }

        #endregion
    }
}
