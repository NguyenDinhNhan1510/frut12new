using Fruit_N12.Models.ViewModels;
using Fruit_N12.Models;
using Microsoft.AspNetCore.Mvc;
using Fruit_N12.Areas.Admin.Models;
using Microsoft.EntityFrameworkCore;

namespace Fruit_N12.Controllers
{
    public class OrderController : Controller
    {
        private readonly FruitN12Context fruitN12Context;

        public OrderController(FruitN12Context _context)
        {
            fruitN12Context = _context;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirst("AccountId")?.Value;

            string codeUserOrder = "DH" + userId;


            var OrderUser = fruitN12Context.TbOrders.Where(x => x.Code.Contains(codeUserOrder));

            

			var Items = (from order in OrderUser
						 join invoice in fruitN12Context.Invoices on order.OrderId equals invoice.OrderId
						  select new InvoiceViewModel
						  {
                              OrderId = order.OrderId,
							  OrderCode = order.Code,
							  DateTime = order.CreatedDate,
							  PaymentMethod = fruitN12Context.PaymentMethods.Where(x => x.PaymentMethodId == invoice.PaymentMethodId).Select(x=>x.MethodName).FirstOrDefault(),
							  OrderStatus = fruitN12Context.TbOrderStatuses.Where(x => x.OrderStatusId == order.OrderStatusId).Select(x => x.Name).FirstOrDefault(),
                              TotalPrice = order.TotalAmount,
							  PaymentStatus = fruitN12Context.PaymentStatus.Where(x => x.PaymentStatusId == invoice.PaymentStatusId).Select(x => x.PaymentName).FirstOrDefault()
						  }).ToList();
						 



			return View(Items);
        }

        public async Task<IActionResult> CancelOrder(string OrderId)
        {
            int orderId = int.Parse(OrderId);

            var order = await fruitN12Context.TbOrders.FindAsync(orderId);

            if (order == null)
            {
                return NotFound();
            }

            fruitN12Context.TbOrders.Remove(order);
            fruitN12Context.TbOrderDetails.RemoveRange(fruitN12Context.TbOrderDetails.Where(x => x.OrderId == orderId));
            fruitN12Context.Invoices.RemoveRange(fruitN12Context.Invoices.Where(x => x.OrderId == orderId));

            return View("Index");
        }
    }
}
