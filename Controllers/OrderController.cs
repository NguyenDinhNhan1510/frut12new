using Fruit_N12.Models.ViewModels;
using Fruit_N12.Models;
using Microsoft.AspNetCore.Mvc;

namespace Fruit_N12.Controllers
{
    public class OrderController : Controller
    {
        private readonly FruitN12Context fruitN12Context;

        public OrderController(FruitN12Context _context)
        {
            fruitN12Context = _context;
        }

        public IActionResult Index()
        {
            var userId = User.FindFirst("AccountId")?.Value;

            string codeUserOrder = "DH" + userId;


            var OrderUser = fruitN12Context.TbOrders.FirstOrDefault(x => x.Code.Equals(codeUserOrder));



            //lay don hang
            var itemsList = fruitN12Context.TbOrderDetails.Where(x => x.OrderId == OrderUser.OrderId);


            //lay id product
            var cartItems = (from items in itemsList
                             join product in fruitN12Context.TbProducts on items.ProductId equals product.ProductId
                             select new CartItemModel
                             {
                                 ProductId = product.ProductId,
                                 Title = product.Title,
                                 Price = (int)product.Price,
                                 Quantity = items.Quantity,
                                 Image = product.Image
                             }).ToList();

            CartItemViewModel cartVM = new()
            {
                CartItems = cartItems,
                GrandTotal = cartItems.Sum(x => x.Quantity * x.Price)

            };

            return View(cartVM);
        }
    }
}
