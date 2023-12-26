using System;
using System.Collections.Generic;

namespace DbmsApp.Models;

public partial class User
{
    public long Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public long Password { get; set; }

    public string Email { get; set; } = null!;

    public string Role { get; set; } = null!;

    public string? Phone { get; set; }

    public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
