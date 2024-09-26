namespace SmartSpend_API.Models
{
    public class DetailedView
    {
        public int DetailedViewID { get; set; }
        public int UserID { get; set; }
        public int CategoryID { get; set; }
        public decimal TotalExpense { get; set; }
        public decimal TotalIncome { get; set; }
        public string MonthYear { get; set; }  // E.g., "August 2024"
    }
}
