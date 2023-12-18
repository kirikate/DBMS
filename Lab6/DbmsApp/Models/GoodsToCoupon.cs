using System;
using System.Collections.Generic;

namespace DbmsApp.Models;

public partial class GoodsToCoupon
{
    public long? CouponId { get; set; }

    public long? ProductId { get; set; }

    public int Count { get; set; }

    public virtual Coupon? Coupon { get; set; }

    public virtual Good? Product { get; set; }
}
