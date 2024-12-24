using System;
using System.Collections.Generic;

namespace Fruit_N12.Models;

public partial class OrderDiscount
{
    public int OrderDiscountId { get; set; }

    public int? OrderId { get; set; }

    public int? DiscountId { get; set; }

    public virtual Discount? Discount { get; set; }
}
