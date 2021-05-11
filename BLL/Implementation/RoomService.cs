using Binbin.Linq;
using BLL.Filter;
using BLL.Interfaces;
using DAL.Entities;
using GameStore.DAL.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Implementation
{
    public class RoomService : IRoomService
    {
        private readonly IGenericRepository<QuestRoom> _roomRepository;
        private readonly IGenericRepository<DecorationType> _typeRepository;
        private readonly IGenericRepository<Company> _companyRepository;

        public RoomService(IGenericRepository<QuestRoom> roomRepository, IGenericRepository<DecorationType> typeRepository, IGenericRepository<Company> companyRepository)
        {
            _roomRepository = roomRepository;
            _typeRepository = typeRepository;
            _companyRepository = companyRepository;
        }

        public async Task AddRoomAsync(QuestRoom room)
        {
            await _roomRepository.CreateAsync(room);
        }

        public async Task AddCompanyAsync(Company company)
        {
            await _companyRepository.CreateAsync(company);
        }

        public async Task AddTypeAsync(DecorationType decorationType)
        {
            await _typeRepository.CreateAsync(decorationType);
        }

        public IEnumerable<QuestRoom> GetAllQuestRooms(List<RoomsFilter> filters)
        {
            if (filters == null || filters.Count == 0)
            {
                return _roomRepository.GetAll();
            }

            var builders = new List<Expression<Func<QuestRoom, bool>>>();

            var groupedFilters = filters.GroupBy(x => x.Type);

            foreach (var group in groupedFilters)
            {
                var tmp = PredicateBuilder.Create(group.FirstOrDefault().Predicate);
                for (int i = 1; i < group.ToArray().Length; i++)
                {
                    tmp = tmp.Or(group.ToArray()[i].Predicate);
                }
                builders.Add(tmp);
            }

            var builder = PredicateBuilder.Create(builders.FirstOrDefault());
            for (int i = 1; i < builders.ToArray().Length; i++)
            {
                builder = builder.And(builders[i]);
            }

            return _roomRepository.GetAll().Where(builder.Compile());
        }

        public IEnumerable<DecorationType> GetAllTypes()
        {
            return _typeRepository.GetAll();
        }

        public IEnumerable<string> GetRatings()
        {
            return new string[] { "0", "1", "2", "3", "4", "5" };
        }

        public IEnumerable<string> GetTypes()
        {
            return this.GetAllTypes().Select(x => x.Name);
        }

        public IEnumerable<Company> GetAllCompanies()
        {
            return _companyRepository.GetAll();
        }

        public async Task DeleteRoomAsync(int id)
        {
            await _roomRepository.DeleteAsync(id);
        }

        public async Task EditRoomAsync(QuestRoom room)
        {
            await _roomRepository.UpdateAsync(room);
        }

        public QuestRoom GetRoom(int id)
        {
            return _roomRepository.Get(id);
        }

        public DecorationType GetType(int id)
        {
            return _typeRepository.Get(id);
        }

        public Company GetCompany(int id)
        {
            return _companyRepository.Get(id);
        }

        public IEnumerable<string> GetCompanies()
        {
            return this.GetAllCompanies().Select(x => x.Name);
        }
    }
}
