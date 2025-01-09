using Fruit_N12.Areas.Admin.Models;
using Fruit_N12.Controllers;
using Fruit_N12.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fruit_N12.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class NewOrderController : Controller
    {
        private readonly FruitN12Context _context;

       
        public NewOrderController(FruitN12Context context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var listorder =  _context.TbOrders.ToList();

            List<Order> cartItems = (from list in listorder
                             select new Order
                             {
                                 OrderId = list.OrderId,
                                 CustomerName = list.CustomerName,
                                 Phone = list.Phone,
                                 Quanlity = list.Quanlity,
                                 Address = list.Address,
                                 TotalAmount = list.TotalAmount,
                                 OrderDate = list.CreatedDate
                             }).ToList();


            TempData["NumberOrder"] = cartItems.Count();


            return View(cartItems);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var listorder = _context.TbOrders.ToList();

            List<Order> cartItems = (from list in listorder
                                     select new Order
                                     {
                                         OrderId = list.OrderId,
                                         CustomerName = list.CustomerName,
                                         Phone = list.Phone,
                                         Quanlity = list.Quanlity,
                                         Address = list.Address,
                                         TotalAmount = list.TotalAmount,
                                         OrderDate = list.CreatedDate
                                     }).ToList();

            Order detail = cartItems.Find(x => x.OrderId == id);

            return View(detail);
        }
    }
}
