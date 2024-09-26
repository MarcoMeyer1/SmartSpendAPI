namespace SmartSpend_API.Models
{
    public class Income
    {
        public int IncomeID { get; set; }
        public int UserID { get; set; }
        public string IncomeReference { get; set; }
        public decimal Amount { get; set; }
        public DateTime IncomeDate { get; set; }
    }
}
