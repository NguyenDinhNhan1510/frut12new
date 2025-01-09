using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Fruit_N12.Areas.Admin.Models;
using Fruit_N12.Models;
using Fruit_N12.Utilities;


namespace WebApplication1.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class LoginController : Controller
    {

        private readonly FruitN12Context _context;

        public LoginController(FruitN12Context context)
        {
            _context = context;
        }
        public IActionResult Index()
        {

            return View();
        }
        [HttpPost]
        public IActionResult Index(AdminUser user)
        {
            if (user == null)
            {
                return NotFound();
            }
            var check = _context.AdminUsers.Where(m => (m.UserName == user.UserName) && (m.Password == user.Password)).FirstOrDefault();

            if (check == null)
            {
                Function._Message = "Đăng nhập không thành công";
                return RedirectToAction("Index", "Login");
            }
            // vào trang Admin nếu đúng user và pass
            Function._Message = string.Empty;
            Function._UserID = check.UserID;
            Function._UserName = string.IsNullOrEmpty(check.UserName) ? string.Empty : check.UserName;
            Function._Email = string.IsNullOrEmpty(check.Email) ? string.Empty : check.Email;

            return RedirectToAction("Index", "Home");
        }
    }
}
