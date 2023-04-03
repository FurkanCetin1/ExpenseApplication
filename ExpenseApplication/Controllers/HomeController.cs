using ExpenseApplication.Data;
using ExpenseApplication.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Transactions;

namespace ExpenseApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly MasrafAppContext _context;

        public HomeController(MasrafAppContext context)
        {
            _context = context;
        }

        [Authorize(Roles ="1,4")]
        public IActionResult Index(bool formSubmitted = false)
        {
            if (formSubmitted)
            {
                ViewData["FormSubmitted"] = true;
            }
            var users = _context.Users.ToList();

            return View(users);
        }
        [Authorize(Roles = "2,4")]
        public async Task<IActionResult> Privacy(DateTime? startDate, DateTime? endDate, string username)
        {
            int currentUserId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "0");

            var expenseForms = _context.ExpenseForms
                .Include(e => e.User)
                .Include(e => e.Expenses)
                .Where(e => e.User.ManagerId == currentUserId && e.StatusId == 1);

            if (startDate.HasValue)
            {
                expenseForms = expenseForms.Where(e => e.CreatedDate >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                expenseForms = expenseForms.Where(e => e.CreatedDate <= endDate.Value);
            }

            if (!string.IsNullOrEmpty(username))
            {
                expenseForms = expenseForms.Where(e => e.User.Username.Contains(username));
            }

            var filteredExpenseForms = await expenseForms.ToListAsync();
            return View("Privacy", filteredExpenseForms);
        }

        [HttpPost]
        public IActionResult SubmitForm(ExpenseForm expenseForm)
        {
            if (ModelState.IsValid)
            {
                int currentUserId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "0");

                expenseForm.UserId = currentUserId;
                expenseForm.CreatedDate = DateTime.UtcNow;
                expenseForm.StatusId = 1; //Onay Bekliyor

                using (TransactionScope scope = new TransactionScope())
                {
                    _context.ExpenseForms.Add(expenseForm);
                    _context.SaveChanges();

                    var expenseHistory = new ExpenseHistory
                    {
                        ExpenseFormId = expenseForm.ExpenseFormId,
                        UserId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "0"),
                        ActionDate = DateTime.UtcNow,
                        StatusId = 1 // Onay Bekliyor
                    };

                    _context.ExpenseHistories.Add(expenseHistory);

                    _context.SaveChanges();

                    scope.Complete();
                }

                    

                return RedirectToAction("Index", new { formSubmitted = true });
            }
            foreach (var modelError in ModelState)
            {
                var fieldName = modelError.Key;
                var errors = modelError.Value.Errors;

                foreach (var error in errors)
                {
                    Console.WriteLine($"Alan: {fieldName}, Hata: {error.ErrorMessage}");
                }
            }

            return View(expenseForm);
        }



        public IActionResult ApproveExpenseForm(int expenseFormId)
        {
            var expenseForm = _context.ExpenseForms.FirstOrDefault(e => e.ExpenseFormId == expenseFormId);
            if (expenseForm == null)
            {
                return NotFound();
            }

            expenseForm.StatusId = 2; // Onaylandı

            var expenseHistory = new ExpenseHistory
            {
                ExpenseFormId = expenseForm.ExpenseFormId,
                UserId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "0"),
                ActionDate = DateTime.UtcNow,
                StatusId = 2 // Onaylandı
            };

            _context.ExpenseHistories.Add(expenseHistory);
            _context.SaveChanges();

            return RedirectToAction("Privacy");
        }



        public IActionResult RejectExpenseForm(int expenseFormId)
        {
            var expenseForm = _context.ExpenseForms.FirstOrDefault(e => e.ExpenseFormId == expenseFormId);
            if (expenseForm == null)
            {
                return NotFound();
            }

            expenseForm.StatusId = 3; // Reddedildi

            var expenseHistory = new ExpenseHistory
            {
                ExpenseFormId = expenseForm.ExpenseFormId,
                UserId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "0"),
                ActionDate = DateTime.UtcNow,
                StatusId = 3 // Reddedildi
            };

            _context.ExpenseHistories.Add(expenseHistory);
            _context.SaveChanges();

            return RedirectToAction("Privacy");
        }


        [HttpPost]
        public async Task<IActionResult> RequestEdit(int expenseFormId, string reviewComment)
        {
            var expenseForm = await _context.ExpenseForms.FindAsync(expenseFormId);
            if (expenseForm == null) { return NotFound(); }
            expenseForm.StatusId = 4; // Düzenle
            expenseForm.ReviewComment = reviewComment; _context.Update(expenseForm);

            var expenseHistory = new ExpenseHistory
            {
                ExpenseFormId = expenseForm.ExpenseFormId,
                UserId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "0"),
                ActionDate = DateTime.UtcNow,
                StatusId = 4, // Düzenle
                Comment = $"Düzenleme istendi: {reviewComment}"
            };

            _context.ExpenseHistories.Add(expenseHistory);

            await _context.SaveChangesAsync();
            return RedirectToAction("Privacy");
        }



        [HttpPost]
        public async Task<IActionResult> SaveChanges(ExpenseForm expenseForm)
        {
            if (ModelState.IsValid)
            {
                var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "0");
                expenseForm.UserId = userId;

                var existingExpenseForm = await _context.ExpenseForms.Include(f => f.Expenses).FirstOrDefaultAsync(f => f.ExpenseFormId == expenseForm.ExpenseFormId);

                if (existingExpenseForm != null)
                {
                    existingExpenseForm.Currency = expenseForm.Currency;
                    existingExpenseForm.StatusId = 1;
                    existingExpenseForm.TotalAmount = expenseForm.Expenses.Sum(e => e.Amount);

                    foreach (var expense in expenseForm.Expenses)
                    {
                        var existingExpense = existingExpenseForm.Expenses.FirstOrDefault(e => e.ExpenseId == expense.ExpenseId);
                        if (existingExpense != null)
                        {
                            existingExpense.ExpenseDate = expense.ExpenseDate;
                            existingExpense.Description = expense.Description;
                            existingExpense.Amount = expense.Amount;
                        }
                        else
                        {
                            existingExpenseForm.Expenses.Add(expense);
                        }
                    }

                    var deletedExpenses = existingExpenseForm.Expenses.Where(e => !expenseForm.Expenses.Any(ef => ef.ExpenseId == e.ExpenseId)).ToList();
                    foreach (var deletedExpense in deletedExpenses)
                    {
                        _context.Expenses.Remove(deletedExpense);
                    }

                    _context.Update(existingExpenseForm);

                    
                    var expenseHistory = new ExpenseHistory
                    {
                        ExpenseFormId = existingExpenseForm.ExpenseFormId,
                        UserId = userId,
                        ActionDate = DateTime.UtcNow,
                        StatusId = 1 // Onay Bekliyor
                    };

                    _context.ExpenseHistories.Add(expenseHistory);

                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(expenseForm);
        }




        [Authorize(Roles = "4")]
        public async Task<IActionResult> Admin(string startDate, string endDate, string username)
        {
            IQueryable<ExpenseApplication.Models.ExpenseHistory> expenseHistories = _context.ExpenseHistories
                .Include(eh => eh.ExpenseForm)
                    .ThenInclude(ef => ef.Expenses)
                .Include(eh => eh.User)
                .Include(eh => eh.Status);

            if (!string.IsNullOrEmpty(startDate) && DateTime.TryParse(startDate, out DateTime sDate))
            {
                expenseHistories = expenseHistories.Where(eh => eh.ActionDate >= sDate);
            }

            if (!string.IsNullOrEmpty(endDate) && DateTime.TryParse(endDate, out DateTime eDate))
            {
                expenseHistories = expenseHistories.Where(eh => eh.ActionDate <= eDate);
            }

            if (!string.IsNullOrEmpty(username))
            {
                expenseHistories = expenseHistories.Where(eh => eh.User.Username.Contains(username));
            }

            var result = await expenseHistories.ToListAsync();

            return View(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public async Task<IActionResult> Manager(DateTime? startDate, DateTime? endDate, string username)
        {
            int currentUserId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "0");

            var expenseForms = _context.ExpenseForms
                .Include(e => e.User)
                .Include(e => e.Expenses)
                .Where(e => e.User.ManagerId == currentUserId && e.StatusId == 1);

            if (startDate.HasValue)
            {
                expenseForms = expenseForms.Where(e => e.CreatedDate >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                expenseForms = expenseForms.Where(e => e.CreatedDate <= endDate.Value);
            }

            if (!string.IsNullOrEmpty(username))
            {
                expenseForms = expenseForms.Where(e => e.User.Username.Contains(username));
            }

            var filteredExpenseForms = await expenseForms.ToListAsync();
            return View("Privacy", filteredExpenseForms);
        }





        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Authorize(Roles = "3,4")]
        public async Task<IActionResult> Accountant(DateTime? startDate, DateTime? endDate, string username)
        {
            var approvedFormsQuery = _context.ExpenseForms
                .Include(ef => ef.Expenses)
                .Include(e => e.User)
                .Where(ef => ef.StatusId == 2);

            if (startDate.HasValue)
            {
                approvedFormsQuery = approvedFormsQuery.Where(ef => ef.CreatedDate >= startDate);
            }

            if (endDate.HasValue)
            {
                approvedFormsQuery = approvedFormsQuery.Where(ef => ef.CreatedDate <= endDate);
            }

            if (!string.IsNullOrEmpty(username))
            {
                approvedFormsQuery = approvedFormsQuery.Where(ef => ef.User.Username.Contains(username));
            }

            var approvedForms = await approvedFormsQuery.ToListAsync();
            return View(approvedForms);
        }


        [HttpPost]
        public async Task<IActionResult> PayExpenseForm(int id)
        {
            var expenseForm = await _context.ExpenseForms.FindAsync(id);


            if (expenseForm == null)
            {
                return NotFound();
            }

            expenseForm.StatusId = 5; // Ödendi

            var expenseHistory = new ExpenseHistory
            {
                ExpenseFormId = expenseForm.ExpenseFormId,
                UserId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "0"),
                ActionDate = DateTime.UtcNow,
                StatusId = 5 // Ödendi
            };

            _context.ExpenseHistories.Add(expenseHistory);
            _context.Entry(expenseForm).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExpenseFormExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Json(new { success = true });
        }


        private bool ExpenseFormExists(int id)
        {
            return _context.ExpenseForms.Any(e => e.ExpenseFormId == id);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            int userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "0");
            var expenseForm = await _context.ExpenseForms.Include(f => f.Expenses).FirstOrDefaultAsync(f => f.ExpenseFormId == id && f.StatusId == 4 && f.UserId == userId);

            if (expenseForm == null)
            {
                return NotFound();
            }

            return View(expenseForm);
        }

        [Authorize(Roles = "1,2")]
        public async Task<IActionResult> PendingExpenses()
        {
            int userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "0");
            var pendingExpenses = await _context.ExpenseForms
                .Include(f => f.Expenses)
                .Where(f => f.StatusId == 4 && f.UserId == userId)
                .ToListAsync();

            return View(pendingExpenses);
        }

        public async Task<IActionResult> PendingForms(DateTime? startDate, DateTime? endDate, string currency)
        {
            int userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "0");

            var query = _context.ExpenseForms.Where(e => e.StatusId == 1 && e.UserId == userId);

            if (startDate.HasValue)
            {
                query = query.Where(e => e.CreatedDate >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(e => e.CreatedDate < endDate.Value.AddDays(1));
            }

            if (!string.IsNullOrEmpty(currency))
            {
                query = query.Where(e => e.Currency == currency);
            }

            return View(await query.ToListAsync());
        }

        public IActionResult ExpenseDetails(int id)
        {
            var expenseForm = _context.ExpenseForms.Include(e => e.Expenses).SingleOrDefault(e => e.ExpenseFormId == id);

            if (expenseForm == null)
            {
                return NotFound();
            }
            var expenses = expenseForm.Expenses;

            return View("ExpenseDetails", expenses);
        }


        public async Task<IActionResult> EditExpense(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var expense = await _context.Expenses.FindAsync(id);
            if (expense == null)
            {
                return NotFound();
            }

            return View(expense);
        }

        [HttpPost]
        public async Task<IActionResult> EditExpense(int id, Expense updatedExpense)
        {
            if (id != updatedExpense.ExpenseId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var originalExpense = await _context.Expenses.FindAsync(updatedExpense.ExpenseId);
                    if (originalExpense == null)
                    {
                        return NotFound();
                    }

                    originalExpense.ExpenseDate = updatedExpense.ExpenseDate;
                    originalExpense.Description = updatedExpense.Description;
                    originalExpense.Amount = updatedExpense.Amount;
                    originalExpense.ExpenseFormId = updatedExpense.ExpenseFormId;

                    _context.Update(originalExpense);

                    var expenseForm = await _context.ExpenseForms.FindAsync(updatedExpense.ExpenseFormId);
                    if (expenseForm != null)
                    {
                        var expenses = await _context.Expenses.Where(e => e.ExpenseFormId == updatedExpense.ExpenseFormId).ToListAsync();
                        expenseForm.TotalAmount = expenses.Sum(e => e.Amount);
                        _context.Update(expenseForm);
                    }

                    await _context.SaveChangesAsync();

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExpenseExists(updatedExpense.ExpenseId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(PendingForms));
            }

            return View(updatedExpense);
        }

        private bool ExpenseExists(int id)
        {
            return _context.Expenses.Any(e => e.ExpenseId == id);
        }





        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }

    }
}