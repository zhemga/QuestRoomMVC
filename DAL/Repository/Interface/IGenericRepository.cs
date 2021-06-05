using System.Collections.Generic;
using System.Threading.Tasks;

namespace GameStore.DAL.Repository.Interface
{
    public interface IGenericRepository<TEntity> where TEntity: class
    {
        Task CreateAsync(TEntity entity);
        void Create(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(int id);
        IEnumerable<TEntity> GetAll();
        TEntity Get(int id);
    }
}
