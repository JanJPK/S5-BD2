using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Warlord.DataAccess;

namespace Warlord.UI.Service.Lookups
{  
    public class LookupService : IVehicleModelLookupService
    {
        #region Fields

        private readonly Func<WarlordDbContext> contextCreator;

        #endregion

        #region Constructors and Destructors

        protected LookupService(Func<WarlordDbContext> contextCreator)
        {
            this.contextCreator = contextCreator;
        }

        #endregion

        #region Public Methods and Operators

        public async Task<IEnumerable<LookupItem>> GetVehicleModelLookupAsync()
        {
            using (var ctx = contextCreator())
            {
                return await ctx.VehicleModels.AsNoTracking()
                    .Select(vm =>
                        new LookupItem
                        {
                            Id = vm.Id,
                            DisplayMember = vm.Manufacturer.ShortName + " / " + vm.Name
                        })
                    .ToListAsync();
            }
        }

        #endregion
    }
}