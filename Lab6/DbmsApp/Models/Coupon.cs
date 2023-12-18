using System;
using System.Collections.Generic;

namespace DbmsApp.Models;

public partial class Coupon
{
    public long Id { get; set; }

    public int Number { get; set; }

    public decimal Price { get; set; }

    public DateTime DateOfStart { get; set; }

    public DateTime? DateOfExpiration { get; set; }
}
