﻿using System.Data.Entity;
using System.Threading.Tasks;
using Warlord.DataAccess;
using Warlord.Model;

namespace Warlord.Service.Repositories
{
    public class VehicleModelRepository : BaseRepository<VehicleModel, WarlordDbContext>, IVehicleModelRepository
    {
        public VehicleModelRepository(WarlordDbContext context) : base(context)
        {
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
    }
}
