using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Poly_Cafe.BLL;
using Poly_Cafe.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Poly_Cafe.Controllers
{
    public class BillController : Controller
    {
        private readonly BillBLL billBLL = new BillBLL();
        private readonly DrinkBLL drinkBLL = new DrinkBLL();

        public IActionResult Index() => Pos();

        public IActionResult Pos()
        {
            var drinks = drinkBLL.GetAll() ?? new List<DrinkDTO>();

            // Tính toán giỏ hàng cho Sidebar
            var cart = GetCartFromSession();
            decimal subtotal = cart.Sum(x => (decimal)x.Price * x.Quantity);

            ViewBag.Subtotal = subtotal;
            ViewBag.Tax = subtotal * 0.08m;
            ViewBag.Total = subtotal * 1.08m;
            ViewBag.CurrentUser = HttpContext.Session.GetString("UserName") ?? "Nhân viên";

            return View(drinks); // Truyền Model sang View
        }

        private List<CartItemDTO> GetCartFromSession()
        {
            var cartJson = HttpContext.Session.GetString("Cart");
            return string.IsNullOrEmpty(cartJson) ? new List<CartItemDTO>() : JsonSerializer.Deserialize<List<CartItemDTO>>(cartJson);
        }

        [HttpPost]
        public IActionResult AddToCart(int drinkId, int quantity = 1)
        {
            var drinks = drinkBLL.GetAll();
            // Tìm kiếm linh hoạt: Kiểm tra tất cả các thuộc tính có chứa chữ "Id"
            var drink = drinks.FirstOrDefault(d => {
                var props = d.GetType().GetProperties();
                var idProp = props.FirstOrDefault(p => p.Name.EndsWith("Id") || p.Name == "Id");
                return idProp != null && Convert.ToInt32(idProp.GetValue(d)) == drinkId;
            });

            if (drink == null) return Json(new { success = false, message = "Không tìm thấy món" });

            var cart = GetCartFromSession();
            var existingItem = cart.FirstOrDefault(x => x.DrinkId == drinkId);

            if (existingItem != null) { existingItem.Quantity += quantity; }
            else
            {
                // Lấy tên và giá linh hoạt
                var props = drink.GetType().GetProperties();
                var name = props.FirstOrDefault(p => p.Name.Contains("Name") || p.Name.Contains("Ten"))?.GetValue(drink)?.ToString() ?? "Sản phẩm";
                var price = Convert.ToInt32(props.FirstOrDefault(p => p.Name.Contains("Price") || p.Name.Contains("Gia"))?.GetValue(drink) ?? 0);

                cart.Add(new CartItemDTO { DrinkId = drinkId, DrinkName = name, Price = price, Quantity = quantity });
            }

            HttpContext.Session.SetString("Cart", JsonSerializer.Serialize(cart));
            return Json(new { success = true });
        }

        [HttpPost]
        public IActionResult Checkout(string paymentMethod)
        {
            var cart = GetCartFromSession();
            if (!cart.Any()) return Json(new { success = false });

            int userId = int.Parse(HttpContext.Session.GetString("UserId") ?? "1");
            decimal subtotal = cart.Sum(x => (decimal)x.Price * x.Quantity);

            var bill = new BillDTO { Total = (int)subtotal, FinalTotal = (int)(subtotal * 1.08m), PaymentMethod = paymentMethod };
            var details = cart.Select(x => new BillDetailDTO { DrinkId = x.DrinkId, Quantity = x.Quantity, Price = x.Price }).ToList();

            if (billBLL.CreateBill(bill, details, userId))
            {
                HttpContext.Session.Remove("Cart");
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
    }
}