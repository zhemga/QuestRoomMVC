using BLL.Filter;
using DAL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IRoomService
    {
        IEnumerable<QuestRoom> GetAllQuestRooms(List<RoomsFilter> filters);
        IEnumerable<DecorationType> GetAllTypes();
        IEnumerable<Company> GetAllCompanies();
        Task AddRoomAsync(QuestRoom room);
        Task AddCompanyAsync(Company company);
        Task AddTypeAsync(DecorationType decorationType);
        Task EditRoomAsync(QuestRoom room);
        Task DeleteRoomAsync(int id);
        IEnumerable<string> GetTypes();
        IEnumerable<string> GetRatings();
        QuestRoom GetRoom(int id);
        DecorationType GetType(int id);
        Company GetCompany(int id);
    }
}
