using System;
using System.Collections.Generic;

namespace WebApplication2.Models;

public partial class Account
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string Pasword { get; set; } = null!;

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public DateTime? Dob { get; set; }

    public int? RoleId { get; set; }

    public virtual Role? Role { get; set; }
}
