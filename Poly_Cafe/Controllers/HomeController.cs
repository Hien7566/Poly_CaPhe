using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http; // Bắt buộc để dùng Session
using Poly_Cafe.BLL;
using Poly_Cafe.DTO;

namespace Poly_Cafe.Controllers
{
    public class HomeController : Controller
    {
        private UserBLL userBLL = new UserBLL();

        // [GET] Hiển thị trang đăng nhập
        public IActionResult Login()
        {
            // Nếu đã đăng nhập rồi (UserId tồn tại trong Session) thì chuyển thẳng vào trong
            if (HttpContext.Session.GetInt32("UserId") != null)
            {
                int? role = HttpContext.Session.GetInt32("Role");
                return role == 1 ? RedirectToAction("Index", "Dashboard") : RedirectToAction("Index", "Bill");
            }
            return View();
        }

        // [POST] Xử lý khi bấm nút Login
        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            // 1. Kiểm tra đầu vào
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                TempData["Error"] = "Vui lòng nhập đầy đủ email và mật khẩu";
                return View();
            }

            // 2. Gọi tầng nghiệp vụ (BLL) để kiểm tra Database
            UserDTO user = userBLL.CheckLogin(email, password);

            if (user != null)
            {
                // 3. Lưu thông tin User vào Session
                HttpContext.Session.SetInt32("UserId", user.Id);
                HttpContext.Session.SetString("Email", user.Email);
                HttpContext.Session.SetString("FullName", user.FullName);
                // Lưu Role: 1 cho Admin, 0 cho Nhân viên
                HttpContext.Session.SetInt32("Role", user.Role ? 1 : 0);

                // 4. Phân quyền chuyển hướng
                if (user.Role == true) // Admin
                {
                    return RedirectToAction("Index", "Dashboard");
                }
                else // Nhân viên (Redirect về trang POS/Bill)
                {
                    return RedirectToAction("Index", "Bill");
                }
            }

            // 5. Nếu sai tài khoản/mật khẩu
            TempData["Error"] = "Email hoặc mật khẩu không chính xác";
            return View();
        }

        // [GET] Đăng xuất
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}