using DAL.Entities;
using GameStore.DAL.Repository.Interface;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace GameStore.DAL.Repository
{
    // Реалізувати нереалізовані методи в EFRepository
    public class EFRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private readonly DbContext _dbContext;
        private readonly DbSet<TEntity> _set;

        public EFRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
            _set = _dbContext.Set<TEntity>();
        }

        public async Task CreateAsync(TEntity entity)
        {
            _set.Add(entity);
            await SaveAsync();
        }

        private async Task SaveAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = _set.Find(id);
            _set.Remove(entity);
            await SaveAsync();

        }

        public TEntity Get(int id)
        {
            return _set.Find(id);
        }

        // TODO: Resolve includes
        public IEnumerable<TEntity> GetAll()
        {
            var entities = _set.AsQueryable();
            foreach (var item in entities)
            {
                _set.Include(x=>item);
            }

            return _set.AsEnumerable();
        }

        public async Task UpdateAsync(TEntity entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;

            await SaveAsync();
        }
    }
}
