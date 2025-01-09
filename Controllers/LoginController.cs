
using Fruit_N12.Models;
using Fruit_N12.Models.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Fruit_N12.Controllers
{
    public class LoginController : Controller
    {
        private readonly FruitN12Context fruitN12Context;
		

		public LoginController(FruitN12Context _context)
        {
            fruitN12Context = _context;
           
        }

		[HttpGet]
		public async Task<IActionResult> Index()
        {
            return View();
        }


        [HttpPost]
		public async Task<IActionResult> Index(LoginModel loginModel)
        {
            var user = await fruitN12Context.TbAccounts.FirstOrDefaultAsync(a => a.Username == loginModel.UserName);

            if (user == null)
            {
                ViewBag.Messager = "Not found";
            }
            else
            {
                if (user.Password == loginModel.Password)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.Username),
                        new Claim("AccountId", user.AccountId.ToString())
                     };

                    // Tao ClaimsIdentity
                    var identity = new ClaimsIdentity(claims, "CookieAuth");
                    var principal = new ClaimsPrincipal(identity);

                    // login va luu cookie
                    await HttpContext.SignInAsync("CookieAuth", principal);


					return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.Messager = "Incorrect Password";

                }
            }



            return View();
        }

        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync("CookieAuth");
            return RedirectToAction("Index", "Login");
        }
    }
}
