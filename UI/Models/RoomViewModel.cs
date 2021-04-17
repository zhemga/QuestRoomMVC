using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UI.Models
{
    public class RoomViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PassingTime { get; set; }
        public int MinPlayers { get; set; }
        public int MinAge { get; set; }
        public string DifficultyLevel { get; set; }
        public string HorrorLevel { get; set; }
        public string Address { get; set; }
        public string CompanyName { get; set; }
        public string CompanyPhone { get; set; }
        public int Price { get; set; }
        public string[] ImagesUrl { get; set; }
        public double Rating { get; set; }
        public string DecorationType { get; set; }
    }
}