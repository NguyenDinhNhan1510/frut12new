namespace Fruit_N12.Areas.Admin.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public string? CustomerName { get; set; }
        public string? Phone {  get; set; }
        public string? Address { get; set; }
        public int? TotalAmount { get; set; }
        public int? Quanlity { get; set; }
        public string? OrderStatus { get; set; }
        public DateTime? OrderDate { get; set; }
    }
}
