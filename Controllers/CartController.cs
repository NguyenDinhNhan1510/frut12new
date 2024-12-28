using Fruit_N12.Models;
using Fruit_N12.Models.ViewModels;
using Fruit_N12.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

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
            //List<CartItemModel> cartItems = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();

            List<CartItemModel> cartItems = fruitN12Context._Cart
    .Select(c => new
    {
        Cart = c,
        Product = fruitN12Context.TbProducts.FirstOrDefault(p => p.ProductId == c.Id)
    })
    .Where(x => x.Product != null)
    .Select(x => new CartItemModel
    {
        ProductId = x.Product.ProductId,
        Title = x.Product.Title,
        Price = (int)x.Product.Price,
        Quantity = x.Cart.Quantity,
        Image = x.Product.Image
    }).ToList();

            CartItemViewModel cartVM = new()
            {
                CartItems = cartItems,
                GrandTotal = cartItems.Sum(x => x.Quantity * x.Price)

            };



            return View(cartVM);
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

        [HttpPost]
        public async Task<IActionResult> CartChange(CartItemViewModel cartItemViewModel)
        {
            var cartdb = fruitN12Context._Cart.ToList();

            for (int i = 0; i < cartdb.Count && i < cartItemViewModel.CartItems.Count; i++)
            {
                cartdb[i].Quantity = cartItemViewModel.CartItems[i].Quantity;
                fruitN12Context._Cart.Update(cartdb[i]); // Cập nhật từng mục
            }

            fruitN12Context.SaveChanges(); // Lưu thay đổi

            return RedirectToAction("Index", "Cart");
        }

        [Route("Cart/Delete/{productIdString}")]
        public async Task<IActionResult> DeleteItem(string productIdString)
        {
            int productId = int.Parse(productIdString);

            Cart item = await fruitN12Context._Cart.FindAsync(productId);

            fruitN12Context._Cart.RemoveRange(item);
            fruitN12Context.SaveChanges();

            return RedirectToAction("Index", "Cart");
        }

            [Route("Cart/Add/{productIdString}")]
        public async Task<IActionResult> Add(string productIdString)
        {
            int productId = int.Parse(productIdString);

            // Tìm sản phẩm từ database
            TbProduct product = await fruitN12Context.TbProducts.FindAsync(productId);

            // Kiểm tra nếu sản phẩm không tồn tại
            if (product == null)
            {
                TempData["Error"] = "Product not found."; // Lưu thông báo lỗi tạm thời
                return RedirectToAction("Index", "Home"); // Chuyển hướng về trang chủ hoặc trang khác
            }

            // Lấy giỏ hàng từ session, nếu không có thì tạo mới
            //List<CartItemModel> cart = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();


            // Kiểm tra sản phẩm đã có trong giỏ hàng chưa
            Cart cartItem = await fruitN12Context._Cart.Where(c => c.Id == productId).FirstOrDefaultAsync();



            if (cartItem == null)
            {

                Cart orderDetail = new Cart()
                {

                    Id = productId,
                    Quantity = 1
                };

                fruitN12Context._Cart.Add(orderDetail);
                fruitN12Context.SaveChanges();
            }
            else
            {
                // Tăng số lượng sản phẩm nếu đã tồn tại
                cartItem.Quantity += 1;
                fruitN12Context._Cart.Update(cartItem);
                fruitN12Context.SaveChanges();
            }




            // Cập nhật lại session với giỏ hàng mới
            //HttpContext.Session.SetJson("Cart", cart);


            // Chuyển hướng về trang trước đó
            return Redirect(Request.Headers["Referer"].ToString());
        }

    }
}
