using System;
using System.Collections.Generic;

namespace DbmsApp.Models;

public partial class Review
{
    public long Id { get; set; }

    public long UserId { get; set; }

    public DateTime Date { get; set; }

    public DateTime? DateOfUpdate { get; set; }

    public string? Text { get; set; }

    public int Grade { get; set; }

    public virtual User User { get; set; } = null!;
}
