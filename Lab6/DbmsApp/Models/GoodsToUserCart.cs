using System;
using System.Collections.Generic;

namespace DbmsApp.Models;

public partial class GoodsToUserCart
{
    public long? UserId { get; set; }

    public long? ProductId { get; set; }

    public int Count { get; set; }

    public virtual Good? Product { get; set; }

    public virtual User? User { get; set; }
}
