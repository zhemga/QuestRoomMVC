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
        IEnumerable<OrderContainer> GetAllOrderContainers();
        Task AddRoomAsync(QuestRoom room);
        Task AddCompanyAsync(Company company);
        Task AddTypeAsync(DecorationType decorationType);
        Task AddOrderContainerAsync(OrderContainer orderContainer);
        void AddOrderContainer(OrderContainer orderContainer);
        Task EditRoomAsync(QuestRoom room);
        Task EditOrderContainerAsync(OrderContainer orderContainer);
        Task DeleteRoomAsync(int id);
        Task DeleteCompanyAsync(int id);
        Task DeleteTypeAsync(int id);
        Task DeleteOrderContainerAsync(int id);
        IEnumerable<string> GetTypes();
        IEnumerable<string> GetCompanies();
        IEnumerable<string> GetRatings();
        QuestRoom GetRoom(int id);
        DecorationType GetType(int id);
        Company GetCompany(int id);
        OrderContainer GetOrderContainer(int id);
    }
}
