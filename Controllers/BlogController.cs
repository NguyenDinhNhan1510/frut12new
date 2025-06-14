using Fruit_N12.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fruit_N12.Controllers
{
    public class BlogController : Controller
    {
        private readonly FruitN12Context _context;
        public BlogController(FruitN12Context context)
        {
            _context = context;
        }

        
        public IActionResult Details()
        {
            return View();
        }


        [Route("/blog/{alias}-{id}.html")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TbBlogs == null)
            {
                return NotFound();
            }

            var blog = await _context.TbBlogs.FirstOrDefaultAsync(m => m.BlogId == id);
            if (blog == null)
            {
                return NotFound();
            }
            ViewBag.blogCommet = _context.TbBlogComments.Where(i => i.BlogId == id).ToList();
            return View(blog);

        }
    }
}
