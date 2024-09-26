namespace SmartSpend_API.Models
{
    public class Category
    {
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public string ColorCode { get; set; }  // New field for color code

        public int UserID { get; set; }  // Add this to associate the category with a user
    }
}
