namespace UI.Models
{
    public class OrderDetailsViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int AmountOfHours { get; set; }
        public int PricePerHour { get; set; }
        public int TotalPrice { get; set; }
    }
}