using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DbmsApp.Models;

public partial class Order
{
    [Key]
    public long Id { get; set; }

    public long UserId { get; set; }

    public DateTime DateOfOrder { get; set; }

    public DateTime? DateOfDelivery { get; set; }

    public int? Price { get; set; }

    public long? AddressId { get; set; }

    public virtual Address? Address { get; set; }

    public virtual User User { get; set; } = null!;
}
