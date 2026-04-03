using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Poly_Cafe.BLL;
using Poly_Cafe.DTO;

namespace Poly_Cafe.Controllers
{
    public class HomeController : Controller
    {
        private UserBLL userBLL = new UserBLL();

        public IActionResult Login()
        {
            // Kiểm tra nếu đã login thì đá vào trong luôn
            if (HttpContext.Session.GetString("UserRole") != null)
            {
                return HttpContext.Session.GetString("UserRole") == "True"
                    ? RedirectToAction("Index", "Dashboard")
                    : RedirectToAction("Index", "Bill");
            }
            return View();
        }

        
        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                TempData["Error"] = "Vui lòng nhập đủ thông tin";
                return View();
            }

            UserDTO user = userBLL.CheckLogin(email, password);

            if (user != null)
            {
                // 1. Lưu ID và Tên hiển thị
                HttpContext.Session.SetInt32("UserId", user.Id);
                HttpContext.Session.SetString("FullName", user.FullName);

                // 2. Lưu cho Layout (Để hiện cái chữ "Quản lý hệ thống")
                HttpContext.Session.SetString("UserRole", user.Role.ToString());

                // 3. QUAN TRỌNG NHẤT: Lưu cho UserController (Để BẤM VÀO ĐƯỢC)
                // Nếu user.Role là true (Admin) thì lưu số 1, ngược lại lưu số 0
                HttpContext.Session.SetInt32("Role", user.Role ? 1 : 0);

                if (user.Role) return RedirectToAction("Index", "Dashboard");
                return RedirectToAction("Index", "Bill");
            }

            TempData["Error"] = "Sai tài khoản hoặc mật khẩu";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}