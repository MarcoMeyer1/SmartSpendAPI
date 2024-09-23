namespace SmartSpend_API.Models
{
    public class Goal
    {
        public int GoalID { get; set; }
        public int UserID { get; set; }
        public string GoalName { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal SavedAmount { get; set; }
        public DateTime? CompletionDate { get; set; }
    }
}
