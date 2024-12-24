using System;
using System.Collections.Generic;

namespace Fruit_N12.Models;

public partial class Discount
{
    public int DiscountId { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public decimal? DiscountPercent { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public virtual ICollection<OrderDiscount> OrderDiscounts { get; set; } = new List<OrderDiscount>();
}
