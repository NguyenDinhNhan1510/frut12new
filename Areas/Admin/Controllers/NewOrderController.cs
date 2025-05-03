using Fruit_N12.Areas.Admin.Models;
using Fruit_N12.Controllers;
using Fruit_N12.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

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
                                 OrderDate = list.CreatedDate,
                                 OrderStatus = _context.TbOrderStatuses.Where(x => x.OrderStatusId == list.OrderStatusId).Select(x=> x.Name).FirstOrDefault(),
                             }).ToList();


            TempData["NumberOrder"] = cartItems.Count();


            return View(cartItems);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var listorder = _context.TbOrders.ToList();
           // listorder = listorder.Where(x => x.OrderId == id).ToList();

           // var listinvoice = _context.Invoices.ToList();

          /*  List<Order> cartItems = (from list in listorder
                                     select new Order
                                     {
                                         OrderId = list.OrderId,
                                         CustomerName = list.CustomerName,
                                         Phone = list.Phone,
                                         Quanlity = list.Quanlity,
                                         Address = list.Address,
                                         TotalAmount = list.TotalAmount,
                                         OrderDate = list.CreatedDate,
                                         OrderStatus = _context.TbOrderStatuses.Where(x => x.OrderStatusId == list.OrderStatusId).Select(x => x.Name).FirstOrDefault(),
                                         OrderStatusId = list.OrderStatusId,
                                        // PaymentMethodId = _context.PaymentMethods.Where(x => x.PaymentMethodId == ),
                                     }).ToList();*/

            var Items = (from order in listorder
                         join invoice in _context.Invoices on order.OrderId equals invoice.OrderId
                         select new Order
                         {
                             OrderId = order.OrderId,
                             CustomerName = order.CustomerName,
                             Phone = order.Phone,
                             Quanlity = order.Quanlity,
                             Address = order.Address,
                             TotalAmount = order.TotalAmount,
                             OrderDate = order.CreatedDate,
                             OrderStatus = _context.TbOrderStatuses.Where(x => x.OrderStatusId == order.OrderStatusId).Select(x => x.Name).FirstOrDefault(),
                             OrderStatusId = order.OrderStatusId,
                             PaymentStatusId = invoice.PaymentStatusId,
                         }).ToList();

            // Find the specific order detail by id
            var orderDetail = Items.FirstOrDefault(x => x.OrderId == id);

           // Order detail = cartItems.Find(x => x.OrderId == id);

            return View(orderDetail);
        }

        [HttpPost]
        public async Task<IActionResult> Details(Order order)
        {
            
                var orderUpdate = await _context.TbOrders.FindAsync(order.OrderId);
                var invoice = _context.Invoices.Where(x => x.OrderId == order.OrderId).FirstOrDefault();

                if (orderUpdate != null)
                {
                    orderUpdate.OrderStatusId = order.OrderStatusId;
                    invoice.PaymentStatusId = order.PaymentStatusId;
                    invoice.OrderStatusId = order.OrderStatusId;

                    _context.Update(orderUpdate);
                    _context.Update(invoice);


                    await _context.SaveChangesAsync();
                }
               
            


            return RedirectToAction("Index");
        }
    }
}
