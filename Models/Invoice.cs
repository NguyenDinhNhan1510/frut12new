using System;
using System.Collections.Generic;

namespace Fruit_N12.Models;

public partial class Invoice
{
    public int InvoiceId { get; set; }

    public int? OrderId { get; set; }

    public int? OrderStatusId { get; set; }

    public DateOnly? InvoiceDate { get; set; }

    public decimal? TotalAmount { get; set; }

    public int? PaymentMethodId { get; set; }

    public virtual PaymentMethod? PaymentMethod { get; set; }
}
