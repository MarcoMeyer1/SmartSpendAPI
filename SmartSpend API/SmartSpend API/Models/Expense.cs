namespace SmartSpend_API.Models
{
    public class Expense
    {
        public int ExpenseID { get; set; }
        public int UserID { get; set; }
        public string ExpenseName { get; set; }
        public int CategoryID { get; set; }
        public decimal Amount { get; set; }
        public DateTime ExpenseDate { get; set; }
    }
}
