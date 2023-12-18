using System;
using System.Collections.Generic;

namespace DbmsApp.Models;

public partial class GoodsToOrder
{
    public long? OrderId { get; set; }

    public long? ProductId { get; set; }

    public int Count { get; set; }

    public virtual Order? Order { get; set; }

    public virtual Good? Product { get; set; }
}
