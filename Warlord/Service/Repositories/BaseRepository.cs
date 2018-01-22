using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Warlord.Model;

namespace Warlord.Service.Repositories
{
    /// <summary>
    ///     Base class for repositories managing communication with database.
    /// </summary>
    /// <typeparam name="TEntity">Repository type.</typeparam>
    /// <typeparam name="TContext">Database context.</typeparam>
    public abstract class BaseRepository<TEntity, TContext> : IBaseRepository<TEntity>
        where TEntity : class
        where TContext : DbContext
    {
        #region Fields

        protected readonly TContext Context;

        #endregion

        #region Constructors and Destructors

        protected BaseRepository(TContext context)
        {
            Context = context;
        }

        #endregion

        #region Public Methods and Operators

        public void Add(TEntity model)
        {
            Context.Set<TEntity>().Add(model);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await Context.Set<TEntity>().ToListAsync();
        }

        public virtual async Task<TEntity> GetByIdAsync(int id)
        {
            return await Context.Set<TEntity>().FindAsync(id);
        }

        public bool HasChanges()
        {
            return Context.ChangeTracker.HasChanges();
        }

        public void Remove(TEntity model)
        {
            Context.Set<TEntity>().Remove(model);
        }

        public async Task SaveAsync()
        {
            await Context.SaveChangesAsync();
        }

        public abstract Task ReloadAsync(int id);

        #endregion
    }
}