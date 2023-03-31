using System;
using System.Collections.Generic;

namespace ExpenseApplication.Models;

public partial class User
{
    public int UserId { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public int? ManagerId { get; set; }

    public int? RoleId { get; set; }

    public virtual ICollection<ExpenseForm> ExpenseForms { get; } = new List<ExpenseForm>();

    public virtual ICollection<ExpenseHistory> ExpenseHistories { get; } = new List<ExpenseHistory>();

    public virtual ICollection<User> InverseManager { get; } = new List<User>();

    public virtual User? Manager { get; set; }

    public virtual Role? Role { get; set; }
}
