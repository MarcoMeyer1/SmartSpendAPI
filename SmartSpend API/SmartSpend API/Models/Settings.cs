namespace SmartSpend_API.Models
{
    public class Settings
    {
        public int SettingID { get; set; }
        public int UserID { get; set; }
        public bool AllowNotifications { get; set; }
        public bool AllowSSO { get; set; }  
        public string Language { get; set; }
    }
}
