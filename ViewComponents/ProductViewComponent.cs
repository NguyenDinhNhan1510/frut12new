using Fruit_N12.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fruit.ViewComponents
{
    public class ProductViewComponent : ViewComponent
    {

        private readonly FruitN12Context _context;

        public ProductViewComponent(FruitN12Context context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var items = _context.TbProducts.Include(m => m.CategoryProduct)
                .Where(m => (bool)m.IsActive).Where(m => m.IsNew);
            return await Task.FromResult<IViewComponentResult>
                (View(items.OrderByDescending(m => m.ProductId).ToList()));
        }
    }
}



