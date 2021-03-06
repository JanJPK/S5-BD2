﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace Warlord.Service.Repositories
{
    public interface IBaseRepository<T>
    {
        #region Public Methods and Operators

        void Add(T model);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        bool HasChanges();
        void Remove(T model);
        Task SaveAsync();
        Task ReloadAsync(int id);

        #endregion
    }
}