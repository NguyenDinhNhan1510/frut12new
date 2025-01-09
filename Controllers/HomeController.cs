using System.Diagnostics;
using Fruit_N12.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fruit_N12.Controllers
{
    public class HomeController : Controller
    {
        private readonly FruitN12Context _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(FruitN12Context context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

		
		public IActionResult Index()
        {
            ViewBag.productCategories = _context.TbProductCategories.ToList();
            ViewBag.productNew = _context.TbProducts.Where(m => m.IsNew).ToList();
            ViewBag.productTop = _context.TbProducts.Where(m => m.IsBestSeller).ToList();
            ViewBag.productStar = _context.TbProducts.Where(m => m.Star >=3).ToList();

            var userId = User.FindFirst("AccountId")?.Value;
            int id = int.TryParse(userId, out var parsedId) ? parsedId : 0;

            var cartUser = _context._Cart.Where(cart => cart.AccountId == id).ToList();
           // var cartTotal = cartUser.Sum(item => item.);
           
            TempData["cart"] = cartUser.Count();
           // TempData["total"] = cartUser.


            return View();
        }

		
		public IActionResult Privacy()
        {
            return View();
        }

		
		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
