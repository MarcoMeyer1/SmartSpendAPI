namespace SmartSpend_API.Models
{
    public class Category
    {
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public string ColorCode { get; set; }
        public int UserID { get; set; }
        public decimal MaxBudget { get; set; } 
    }
}
