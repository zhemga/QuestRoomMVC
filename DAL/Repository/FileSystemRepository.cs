using GameStore.DAL.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.DAL.Repository
{
    public class FileSystemRepository<Tentity> //: IGenericRepository<Tentity> 
                                               //  where Tentity: class
    {
        public Task CreateAsync(Tentity entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Tentity Get(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Tentity> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Tentity entity)
        {
            throw new NotImplementedException();
        }
    }
}
