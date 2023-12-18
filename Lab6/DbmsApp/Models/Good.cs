using System;
using System.Collections.Generic;

namespace DbmsApp.Models;

public partial class Good
{
    public long Id { get; set; }

    public decimal Price { get; set; }

    public string? Size { get; set; }

    public long? ProductId { get; set; }

    public virtual Product? Product { get; set; }
}
