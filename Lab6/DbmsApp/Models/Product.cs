using System;
using System.Collections.Generic;

namespace DbmsApp.Models;

public partial class Product
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public string Type { get; set; } = null!;

    public string? Ingredients { get; set; }

    public virtual ICollection<Good> Goods { get; set; } = new List<Good>();
}
