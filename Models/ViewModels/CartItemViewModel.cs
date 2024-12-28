using Fruit_N12.Models.ViewModels;
using Fruit_N12.Controllers;
namespace Fruit_N12.Models.ViewModels
{
    public class CartItemViewModel
    {
        public List<CartItemModel> CartItems { get; set; }
        public decimal GrandTotal { get; set; }

        public void CartItemAddModel() 
        {

        }

    }
}
