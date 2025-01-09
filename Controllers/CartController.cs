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
             var userId = User.FindFirst("AccountId")?.Value;
 
             int id = int.TryParse(userId, out var parsedId) ? parsedId : 0;


            var cartUser = fruitN12Context._Cart.Where(cart => cart.AccountId == id).ToList();

            //lay id product
            var cartItems = (from cart in cartUser
                             join product in fruitN12Context.TbProducts on cart.ProductId equals product.ProductId
                             select new CartItemModel
                             {
                                 ProductId = product.ProductId,
                                 Title = product.Title,
                                 Price = (int)product.Price,
                                 Quantity = cart.Quantity,
                                 Image = product.Image
                             }).ToList();

            CartItemViewModel cartVM = new()
            {
                CartItems = cartItems,
                GrandTotal = cartItems.Sum(x => x.Quantity * x.Price)

            };



            return View(cartVM);
        }



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
            
            var userId = User.FindFirst("AccountId")?.Value;
            int id = int.TryParse(userId, out var parsedId) ? parsedId : 0;

            var cartUser = fruitN12Context._Cart.Where(cart => cart.AccountId == id).ToList();

            

            for (int i = 0; i < cartUser.Count && i < cartItemViewModel.CartItems.Count; i++)
            {

                    cartUser[i].Quantity = cartItemViewModel.CartItems[i].Quantity;
                    fruitN12Context._Cart.Update(cartUser[i]); // Cập nhật từng mục
                

            }

            fruitN12Context.SaveChanges(); // Lưu thay đổi

            return RedirectToAction("Index", "Cart");
        }

        [Route("Cart/Delete/{productIdString}")]
        public async Task<IActionResult> DeleteItem(string productIdString)
        {
            var userId = User.FindFirst("AccountId")?.Value;
            int id = int.TryParse(userId, out var parsedId) ? parsedId : 0;
            var cartUser = fruitN12Context._Cart.Where(cart => cart.AccountId == id).ToList();

            int productId =  int.Parse(productIdString);

            Cart item = cartUser.FirstOrDefault(cart => cart.ProductId == productId);

            fruitN12Context._Cart.RemoveRange(item);
            fruitN12Context.SaveChanges();

            return RedirectToAction("Index", "Cart");
        }

        [Route("Cart/Add/{productIdString}")]
        public async Task<IActionResult> Add(string productIdString)
        {
            int productId = int.Parse(productIdString);
            var userId = User.FindFirst("AccountId")?.Value;
            var username = User.Identity.Name;

            if(userId == null)
            {
                return RedirectToAction("Index", "Login");
            }

            // Chuyển userId thành kiểu int nếu cần
            int id = int.TryParse(userId, out var parsedId) ? parsedId : 0;

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
            Cart cartItem = await fruitN12Context._Cart.Where(c => c.ProductId == productId).Where(y=>y.AccountId == id).FirstOrDefaultAsync();



            if (cartItem == null)
            {

                Cart orderDetail = new Cart()
                {
                    Id = productId+"_"+ username+"_"+ userId,
                    ProductId = productId,
                    Quantity = 1,
                    AccountId = id
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


            // Chuyển hướng về trang trước đó
            return Redirect(Request.Headers["Referer"].ToString());
        }

        public int? findPrice(int productId)
        {
            var find = fruitN12Context.TbProducts.Find(productId);

            return find.Price;
        }



  
        [Route("Cart/CheckOut/{TotalAmountString}")]
        public async Task<IActionResult> CheckOut(string TotalAmountString)
        {
            var userId = User.FindFirst("AccountId")?.Value;
            int id = int.TryParse(userId, out var parsedId) ? parsedId : 0;
            var cartUser = fruitN12Context._Cart.Where(cart => cart.AccountId == id).ToList();

            //tinh tong san pham mua
            int? totalQuantity = cartUser.Sum(p => p.Quantity);


            //find information customer
            var UserInfor = fruitN12Context.TbAccounts.Find(id);

            //convert int
            int TotalAmount = int.Parse(TotalAmountString);



            if (cartUser != null && userId != null)
            {
                 TbOrder tbOrder = new TbOrder()
                  {
                      Code = "DH"+userId,
                      CustomerName = UserInfor.FullName,
                      Phone = UserInfor.Phone,
                      TotalAmount = TotalAmount,
                      CreatedDate = DateTime.Now,
                      Quanlity = totalQuantity
                      
                  };

                //them vao tbOrder
                fruitN12Context.TbOrders.Add(tbOrder);
                fruitN12Context.SaveChanges();

                for(int i = 0; i < cartUser.Count(); i++)
                {
                    TbOrderDetail tbOrderDetail = new TbOrderDetail()
                    {
                        OrderId = tbOrder.OrderId,
                        ProductId = cartUser[i].ProductId,
                        Price = findPrice(cartUser[i].ProductId),
                        Quantity = cartUser[i].Quantity,

                    };

                    fruitN12Context.TbOrderDetails.Add(tbOrderDetail);
                }

               

                
                //fruitN12Context.TbOrderDetails.Add(tbOrderDetail);
                
               
                //xoa trong shopping cart
                fruitN12Context._Cart.RemoveRange(cartUser);

                fruitN12Context.SaveChanges();

                TempData["OderMesseger"] = "Order Success!";
                

            }

            return RedirectToAction("Index", "Cart");
        }
    }
}
