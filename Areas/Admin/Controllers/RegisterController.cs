using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Fruit_N12.Areas.Admin.Models;
using Fruit_N12.Models;
using Fruit_N12.Utilities;
using Fruit_N12.Areas.Admin.Models;

namespace Harmic_0.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RegisterController : Controller
    {
        private readonly FruitN12Context _context;

        public RegisterController(FruitN12Context context)
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

            //Kiểm tra sự tồn tại của email trong csdl 
            var check = _context.AdminUsers.Where(m => m.Email == user.Email).FirstOrDefault();
            if (check != null)
            {
                //Hiển thị thong báo có thể làm cách khác 
                Function._MessageEmail = "Duplicate Email!";
                return RedirectToAction("Index", "Register");
            }
            //Nếu không có thì thêm vào csdl 
            Function._MessageEmail = string.Empty;
           // user.Password = Function.MD5Password(user.Password);
            _context.Add(user);
            _context.SaveChanges();
            return RedirectToAction("Index", "Login");
        }
    }
}
