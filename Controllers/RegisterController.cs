using Fruit_N12.Areas.Admin.Models;
using Fruit_N12.Models;
using Fruit_N12.Models.ViewModels;
using Fruit_N12.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace Fruit_N12.Controllers
{
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
        public IActionResult Index(RegisterModel user)
        {
            if (user == null)
            {
                return NotFound();
            }

            //Kiểm tra sự tồn tại của email trong csdl 
            var check = _context.TbAccounts.Where(m => m.Email == user.Email).FirstOrDefault();
            if (check != null)
            {
                //Hiển thị thong báo có thể làm cách khác 
                Function._MessageEmail = "Duplicate Email!";
                return RedirectToAction("Index", "Register");
            }
            //Nếu không có thì thêm vào csdl 
            Function._MessageEmail = "Register success!";
            // user.Password = Function.MD5Password(user.Password);

            TbAccount newAccount = new TbAccount()
            {
                Username = user.UserName,
                Email = user.Email,
                Password = user.Password,

            };

            _context.TbAccounts.Add(newAccount);
            _context.SaveChanges();
            return RedirectToAction("Index", "Login");
        }
    }
}
