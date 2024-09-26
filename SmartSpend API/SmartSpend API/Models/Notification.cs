namespace SmartSpend_API.Models
{
    public class Notification
    {
        public int NotificationID { get; set; }
        public int UserID { get; set; }
        public string NotificationText { get; set; }
        public DateTime NotificationDate { get; set; }
    }
}
