namespace Fruit_N12.Models
{
    public partial class Cart
    {
        public string Id { get; set; }
        public int ProductId { get; set; }
        public int? Quantity { get; set; }
        public int AccountId { get; set; }
    }
}
