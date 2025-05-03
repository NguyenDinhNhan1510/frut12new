using Fruit_N12.Models;
using Microsoft.AspNetCore.Mvc;

namespace Fruit_N12.Controllers
{
    public class OrderDetailsController : Controller
    {
        private readonly FruitN12Context fruitN12Context;

        public OrderDetailsController(FruitN12Context fruitN12Context)
        {
            this.fruitN12Context = fruitN12Context;
        }

        // GET: OrderDetails
        [HttpGet]
        public IActionResult Index([FromQuery] string OrderId)
        {
            //var userId = User.FindFirst("AccountId")?.Value;

           


            var OrderUser = fruitN12Context.TbOrders.FirstOrDefault(x => x.OrderId == int.Parse(OrderId));

            var orderDetails = fruitN12Context.TbOrderDetails
                .Where(x => x.OrderId == OrderUser.OrderId)
                .ToList();

            var Items = (from cart in orderDetails
                         join product in fruitN12Context.TbProducts on cart.ProductId equals product.ProductId
                         select new CartItemModel
                         {
                             ProductId = product.ProductId,
                             Title = product.Title,
                             Price = (int)product.Price,
                             Quantity = cart.Quantity,
                             Image = product.Image
                         }).ToList();

            ViewBag.TotalPrice = Items.Sum(x => x.Price * x.Quantity);

            return View(Items);
        }
    }
}
