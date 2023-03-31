using System;
using System.Collections.Generic;

namespace ExpenseApplication.Models;

public partial class Role
{
    public int RoleId { get; set; }

    public string? RoleName { get; set; }

    public virtual ICollection<User> Users { get; } = new List<User>();
}
