namespace SmartSpend_API.Models
{
    public class CategoryExpenseTotal
    {
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public string ColorCode { get; set; }
        public decimal MaxBudget { get; set; }
        public decimal TotalSpent { get; set; }
    }
}
