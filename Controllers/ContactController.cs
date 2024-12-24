using Fruit_N12.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Fruit_N12.Controllers
{
    public class ContactController : Controller
    {

        public readonly FruitN12Context _context;

        public ContactController(FruitN12Context context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(string name, string email, string message)
        {
            try
            {
                TbContact contact = new TbContact();
                contact.Name = name;
                contact.Email = email;
                contact.Message = message;
                _context.TbContacts.Add(contact);
                _context.SaveChanges();
                return Json(new { status = true });
            }
            catch
            {
                return Json(new { status = false });
            }
        }
    }
}
