using Fruit_N12.Models;
using Microsoft.AspNetCore.Mvc;

namespace Harmic_DN.Views.ViewComponents
{
    public class MenuTopViewComponent : ViewComponent
    {
        private readonly FruitN12Context _context;
        public MenuTopViewComponent(FruitN12Context context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var items = _context.TbMenus.Where(m => (bool)m.IsActive).
                OrderBy(m => m.Position).ToList();
            return await Task.FromResult<IViewComponentResult>(View(items));
        }
    }
}