using Microsoft.AspNetCore.Mvc;
using Fruit_N12.Utilities;
namespace Fruit_N12.Areas.Admin.Controllers
{
    public class FileManagerController : Controller
    {
        [Area("Admin")]
        [Route("/Admin/file-manager")]
        public IActionResult Index()
        {
            if (!Function.IsLogin())
            {
                return RedirectToAction("Index", "Login");
            }
            return View();
        }
    }
}
