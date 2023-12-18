using System;
using System.Collections.Generic;

namespace DbmsApp.Models;

public partial class CouponsToOrder
{
    public long? OrderId { get; set; }

    public long? CouponId { get; set; }

    public int? Count { get; set; }

    public virtual Coupon? Coupon { get; set; }

    public virtual Order? Order { get; set; }
}
