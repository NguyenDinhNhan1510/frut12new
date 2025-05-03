namespace Fruit_N12.Models.ViewModels
{
	public class InvoiceViewModel
	{
		public int OrderId { get; set; }
        public string? OrderCode { get; set; }
		public DateTime? DateTime { get; set; }
		public string? PaymentMethod { get; set; }
		public string? OrderStatus { get; set; }
		public int? TotalPrice { get; set; }
		public string? PaymentStatus { get; set; }
	}
}
