using Fruit_N12.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace Fruit_N12.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {

        [Area("Admin")]
        public IActionResult Index()
        {
            if(!Function.IsLogin())
                    {
                return RedirectToAction("Index", "Login");
            }

            return View();
        }
    }
}
