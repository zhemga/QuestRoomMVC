﻿using BLL.Filter;
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
        IEnumerable<string> GetTypes();
        IEnumerable<string> GetRatings();
        QuestRoom GetRoom(int id);
    }
}
