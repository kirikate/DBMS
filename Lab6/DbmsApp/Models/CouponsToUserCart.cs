using System;
using System.Collections.Generic;

namespace DbmsApp.Models;

public partial class CouponsToUserCart
{
    public long? UserId { get; set; }

    public long? CouponId { get; set; }

    public int Count { get; set; }

    public virtual Coupon? Coupon { get; set; }

    public virtual User? User { get; set; }
}
