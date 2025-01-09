using Fruit_N12.Models;
using SixLabors.ImageSharp;

namespace Fruit_N12.Controllers
{
    public class CartItemModel
    {
        public int ProductId { get; set; }
        public string Title { get; set; }
        public int? Quantity {  get; set; }
        public int Price { get; set; }

        public decimal?  Total
        {
            get { return Quantity * Price; }
        }
        public string Image { get; set; }

        public CartItemModel()
        { 
            
        }

        public CartItemModel(TbProduct product)
        {
            ProductId = product.ProductId;
            Title = product?.Title ?? "No Title";
            Price = product?.PriceSale ?? 0;
            Quantity = 1;
            Image = product?.Image ?? "no-image.jpg";

        }
    }
}
