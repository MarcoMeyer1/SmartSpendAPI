namespace SmartSpend_API.Models
{
    public class Reminder
    {
        public int ReminderID { get; set; }
        public int UserID { get; set; }
        public string Description { get; set; }
        public DateTime DateDue { get; set; }
        public DateTime? NotificationDate { get; set; }
        public bool IsEnabled { get; set; }
    }
}
