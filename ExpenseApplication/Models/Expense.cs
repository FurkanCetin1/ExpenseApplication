using System;
using System.Collections.Generic;

namespace ExpenseApplication.Models;

public partial class Expense
{
    public int ExpenseId { get; set; }

    public DateTime ExpenseDate { get; set; }

    public string? Description { get; set; } = null!;

    public decimal Amount { get; set; }

    public string? ReasonRejection { get; set; }

    public int? ExpenseFormId { get; set; }

    public virtual ExpenseForm? ExpenseForm { get; set; }
}
