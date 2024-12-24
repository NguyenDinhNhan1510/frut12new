using Fruit_N12.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Harmic_0.ViewComponents
{
    public class BlogViewComponent : ViewComponent
    {

        private readonly FruitN12Context _context;

        public BlogViewComponent(FruitN12Context context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var items = _context.TbBlogs.Include(m => m.Category)
                .Where(m => (bool)m.IsActive).Where(m => m.IsActive);
            return await Task.FromResult<IViewComponentResult>
                (View(items.OrderByDescending(m => m.BlogId).ToList()));
        }
    }
}