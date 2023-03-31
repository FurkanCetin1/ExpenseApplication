namespace ExpenseApplication.Models
{
    public class UserExpenseSummaryViewModel
    {
        public int UserId { get; set; }
        public string FullName { get; set; }
        public decimal TotalExpense { get; set; }
        public decimal AverageExpense { get; set; }
    }

}
