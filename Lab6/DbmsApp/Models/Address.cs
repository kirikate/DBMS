using System;
using System.Collections.Generic;

namespace DbmsApp.Models;

public partial class Address
{
    public long Id { get; set; }

    public long? UserId { get; set; }

    public string Adress { get; set; } = null!;

    public string? Entrance { get; set; }

    public string? Number { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual User? User { get; set; }
}
