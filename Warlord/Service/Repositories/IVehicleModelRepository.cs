﻿using System.Threading.Tasks;
using Warlord.Model;

namespace Warlord.Service.Repositories
{
    public interface IVehicleModelRepository : IBaseRepository<VehicleModel>
    {
        #region Public Methods and Operators

        Task<bool> HasVehiclesAsync(int id);

        VehicleModel GetById(int id);

        #endregion
    }
}