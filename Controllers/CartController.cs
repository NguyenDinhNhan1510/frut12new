using Fruit_N12.Models;
using Fruit_N12.Models.ViewModels;
using Fruit_N12.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Fruit_N12.Controllers
{
    public class CartController : Controller
    {
        private readonly FruitN12Context fruitN12Context;
        public CartController(FruitN12Context _context)
        {
            fruitN12Context = _context;
        }
        public IActionResult Index()
        {
            List<CartItemModel> cartItems = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();
            CartItemViewModel cartVM = new()
            {
                CartItems = cartItems,
                GrandTotal = cartItems.Sum(x => x.Quantity* x.Price)
                   
            };
            return View();
        }

        // public AcceptedResult CheckOut()
        //{
        //   return View("~/Views/Checkout/Index.cshtml");
        //}

        /*  public async Task<IActionResult> Add(int productId)
          {
              TbProduct product = await fruitN12Context.TbProducts.FindAsync(productId);
              List<CartItemModel> cart = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();
              CartItemModel cartItem = cart.Where(c => c.ProductId == productId).FirstOrDefault();

              if (cartItem == null) 
              {

                  cart.Add(new CartItemModel(product));
              }
              else
              {
                  cartItem.Quantity += 1;
              }

              HttpContext.Session.SetJson("Cart", cart);
              return Redirect(Request.Headers["Referer"].ToString());
          }
        */
        public async Task<IActionResult> Add(int productId)
        {
            // Tìm sản phẩm từ database
            TbProduct product = await fruitN12Context.TbProducts.FindAsync(productId);

            // Kiểm tra nếu sản phẩm không tồn tại
            if (product == null)
            {
                TempData["Error"] = "Product not found."; // Lưu thông báo lỗi tạm thời
                return RedirectToAction("Index", "Home"); // Chuyển hướng về trang chủ hoặc trang khác
            }

            // Lấy giỏ hàng từ session, nếu không có thì tạo mới
            List<CartItemModel> cart = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();

            // Kiểm tra sản phẩm đã có trong giỏ hàng chưa
            CartItemModel cartItem = cart.Where(c => c.ProductId == productId).FirstOrDefault();

            if (cartItem == null)
            {
                // Thêm sản phẩm vào giỏ hàng nếu chưa có
                cart.Add(new CartItemModel(product));
            }
            else
            {
                // Tăng số lượng sản phẩm nếu đã tồn tại
                cartItem.Quantity += 1;
            }

            // Cập nhật lại session với giỏ hàng mới
            HttpContext.Session.SetJson("Cart", cart);

            // Chuyển hướng về trang trước đó
            return Redirect(Request.Headers["Referer"].ToString());
        }

    }
}
