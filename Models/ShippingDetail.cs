using System;
using System.Collections.Generic;

namespace Fruit_N12.Models;

public partial class ShippingDetail
{
    public int ShippingId { get; set; }

    public int? OrderId { get; set; }

    public int? ShipperId { get; set; }

    public DateOnly? ShippingDate { get; set; }

    public DateOnly? DeliveryDate { get; set; }

    public decimal? ShippingCost { get; set; }
}
