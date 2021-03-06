namespace UI.Models
{
    public class OrderContainerViewModel
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public bool NotRegisteredUser { get; set; }
        public string Phone { get; set; }
        public string DateTime { get; set; }
        public bool IsAccepted { get; set; }
    }
}