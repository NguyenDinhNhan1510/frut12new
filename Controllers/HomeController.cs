using System.Diagnostics;
using Fruit_N12.Models;
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
