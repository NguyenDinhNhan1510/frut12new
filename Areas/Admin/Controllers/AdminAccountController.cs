using Fruit_N12.Areas.Admin.Models;
using Fruit_N12.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Fruit_N12.Utilities;

namespace Fruit_N12.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminAccountController : Controller
    {
        FruitN12Context _fruitN12Context;

        public AdminAccountController(FruitN12Context fruitN12Context)
        {
            _fruitN12Context = fruitN12Context;
        }

        
        public async Task<IActionResult> Index()
        {
            List<AdminUser> adminUsers = await _fruitN12Context.AdminUsers.ToListAsync();
            return View(adminUsers);
        }

        [HttpGet]
        public IActionResult CreateAdminAccount()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateAdminAccount(AdminUser user)
        {
            if (user == null)
            {
                return NotFound();
            }

            //Kiểm tra sự tồn tại của email trong csdl 
            var check = _fruitN12Context.AdminUsers.Where(m => m.Email == user.Email).FirstOrDefault();
            if (check != null)
            {
                //Hiển thị thong báo có thể làm cách khác 
                Function._MessageEmail = "Duplicate Email!";
                return RedirectToAction("Index", "Register");
            }
            //Nếu không có thì thêm vào csdl 
            //Function._MessageEmail = string.Empty;
            // user.Password = Function.MD5Password(user.Password);
            _fruitN12Context.Add(user);
            _fruitN12Context.SaveChanges();
            return RedirectToAction("Index", "Login");
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var accountadmins = await _fruitN12Context.AdminUsers
                .FirstOrDefaultAsync(m => m.UserID == id);
            if (accountadmins == null)
            {
                return NotFound();
            }

            return View(accountadmins);
        }

        // POST: Admin/Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var accountadmin = await _fruitN12Context.AdminUsers.FindAsync(id);
            if (accountadmin != null)
            {
                _fruitN12Context.AdminUsers.Remove(accountadmin);
            }

            await _fruitN12Context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

      
    }
}
