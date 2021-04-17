using DAL.Entities;
using System;
using System.Linq.Expressions;

namespace BLL.Filter
{
    public class RoomsFilter
    {
        public string Type { get; set; }
        public string Name { get; set; }

        public Expression<Func<QuestRoom, bool>> Predicate { get; set; }
    }
}
