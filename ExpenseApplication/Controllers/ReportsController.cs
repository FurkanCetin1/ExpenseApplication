using ExpenseApplication.Data;
using ExpenseApplication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseApplication.Controllers
{
    [Authorize(Roles ="4")]
    public class ReportsController : Controller
    {
        private readonly MasrafAppContext _context;
        public ReportsController(MasrafAppContext context)
        {
            _context = context;
        }
        public IActionResult MonthlyExpenseSummary()
        {
            var monthlyExpenseSummary = _context.Expenses
        .Where(e => e.ExpenseForm.StatusId == 5) 
        .GroupBy(e => new { e.ExpenseDate.Year, e.ExpenseDate.Month })
        .Select(g => new MonthlyExpenseSummaryViewModel
        {
            Year = g.Key.Year,
            Month = g.Key.Month,
            TotalExpense = g.Sum(e => e.Amount)
        })
        .OrderByDescending(m => m.Year)
        .ThenByDescending(m => m.Month)
        .ToList();

            return View(monthlyExpenseSummary);
        }

        public IActionResult UserExpenseSummary()
        {
            const int paidStatusId = 5;

            var userExpenseSummary = _context.Expenses
                .Where(e => e.ExpenseForm.StatusId == paidStatusId)
                .GroupBy(e => new { e.ExpenseForm.UserId, e.ExpenseForm.User.FirstName, e.ExpenseForm.User.LastName })
                .Select(g => new UserExpenseSummaryViewModel
                {
                    UserId = g.Key.UserId,
                    FullName = g.Key.FirstName + " " + g.Key.LastName,
                    TotalExpense = g.Sum(e => e.Amount),
                    AverageExpense = g.Average(e => e.Amount)
                })
                .OrderByDescending(u => u.TotalExpense)
                .ToList();

            return View(userExpenseSummary);
        }


    }
}
