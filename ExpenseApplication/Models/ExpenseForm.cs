using System;
using System.Collections.Generic;

namespace ExpenseApplication.Models;

public partial class ExpenseForm
{
    public int ExpenseFormId { get; set; }

    public int UserId { get; set; }

    public decimal TotalAmount { get; set; }

    public string Currency { get; set; } = null!;

    public int? StatusId { get; set; }
    public string? ReviewComment { get; set; }

    public DateTime? CreatedDate { get; set; }

    public virtual Status? Status { get; set; }

    public virtual ICollection<ExpenseHistory> ExpenseHistories { get; } = new List<ExpenseHistory>();

    public virtual ICollection<Expense> Expenses { get; } = new List<Expense>();

    public virtual User? User { get; set; } = null!;
}
