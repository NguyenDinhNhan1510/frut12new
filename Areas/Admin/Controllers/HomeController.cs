using Fruit_N12.Areas.Admin.Models;
using Fruit_N12.Models;
using Fruit_N12.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Fruit_N12.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        private readonly FruitN12Context _context;

        public HomeController(FruitN12Context context)
        {
            _context = context;
        }

        [Area("Admin")]
        public IActionResult Index()
        {
            if (!Function.IsLogin())   //login
            {
                return RedirectToAction("Index", "Login");
            }

            /*
             * Lay du lieu cho pie char
             */

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

            var groupedProducts = _context.TbOrderDetails
                .GroupBy(o => o.ProductId) // Nhóm theo ProductID
                .Select(g => new
                {
                   ProductID = g.Key,
                   TotalQuantity = g.Sum(o => o.Quantity) // Tính tổng số lượng
                })
                .ToList();

            // Lưu danh sách ProductID và số lượng tương ứng
            List<int?> CartItems = groupedProducts.Select(g => g.ProductID).ToList();
            List<int?> Quantities = groupedProducts.Select(g => g.TotalQuantity).ToList();

            

            // Truy vấn danh sách sản phẩm có trong giỏ hàng
            var productOrders = _context.TbProducts
                .Where(p => CartItems.Contains(p.ProductId)) // Lọc theo danh sách ID trong CartItems
                .Select(p => new
                {
                    // p.ProductId,
                    p.Title,
                    //   p.Price,
                    //Quantity = Quantities[CartItems.IndexOf(p.ProductId)] // Lấy số lượng tương ứng
                })
                .ToList();

            
            /*
             * Lay du lieu cho bar Chart
             */

            var groupedData = _context.TbOrders
                .Where(o => o.CreatedDate.HasValue) // Kiểm tra CreatedDate không null
                .GroupBy(o => new
                {
                  Month = o.CreatedDate.Value.Month,
                  Day = o.CreatedDate.Value.Day,
                  Year = o.CreatedDate.Value.Year
                }) // Nhóm theo tháng và ngày
                .Select(g => new
                {
                  Month = g.Key.Month,
                  Day = g.Key.Day,
                  Year = g.Key.Year,
                  TotalSales = g.Sum(o => (decimal?)o.TotalAmount) ?? 0 // Tính tổng doanh thu
                })
                .OrderBy(g => g.Year) // Sắp xếp theo tháng
                .ThenBy(g => g.Month)
                .ThenBy(g => g.Day)// Sau đó sắp xếp theo ngày
                .ToList();

            // Tách dữ liệu để truyền vào View
            List<string> Labels = groupedData.Select(g => $"{g.Day}/{g.Month}/{g.Year}").ToList();
            List<decimal> Totals = groupedData.Select(g => g.TotalSales).ToList();


            //truyen du lieu bieu do pieChart
            ViewBag.QuanTities = Quantities;
            ViewBag.CartItems = productOrders.Select(g => g.Title).ToList();

            // truyen du lieu bieu cho barChar
            ViewBag.Labels = Labels;
            ViewBag.Totals = Totals;

            //truyen so don dat hang
            TempData["NumberOrder"] = cartItems.Count();

            return View();
        }
    }
}
