using System;
using System.Collections.Generic;

namespace ExpenseApplication.Models;

public partial class ExpenseHistory
{
    public int Id { get; set; }

    public int? ExpenseFormId { get; set; }

    public int? UserId { get; set; }

    public DateTime? ActionDate { get; set; }

    public string? Comment { get; set; }

    public int? StatusId { get; set; }
    public virtual Status? Status { get; set; }
    public virtual ExpenseForm? ExpenseForm { get; set; }

    public virtual User? User { get; set; }
}
