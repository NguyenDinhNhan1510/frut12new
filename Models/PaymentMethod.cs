using System;
using System.Collections.Generic;

namespace Fruit_N12.Models;

public partial class PaymentMethod
{
    public int PaymentMethodId { get; set; }

    public string? MethodName { get; set; }

    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
}
