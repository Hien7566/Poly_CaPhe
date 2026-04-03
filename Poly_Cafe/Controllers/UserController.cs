using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Poly_Cafe.BLL;
using Poly_Cafe.DTO;

namespace Poly_Cafe.Controllers
{
    public class UserController : Controller
    {
        private UserBLL bll = new UserBLL();

        private bool IsAdmin() => HttpContext.Session.GetInt32("Role") == 1;

        public IActionResult Index()
        {
            if (!IsAdmin()) return RedirectToAction("Index", "Dashboard");
            return View(bll.GetListUser());
        }

        [HttpGet]
        public IActionResult Create() => IsAdmin() ? View() : RedirectToAction("Index", "Dashboard");

        [HttpPost]
        public IActionResult Create(UserDTO user)
        {
            if (!IsAdmin()) return RedirectToAction("Index", "Dashboard");

            // Check trùng Email trước khi lưu
            if (bll.IsEmailExists(user.Email))
            {
                ViewBag.Error = "Email này đã tồn tại rồi Nam ơi!";
                return View(user);
            }

            if (bll.CreateUser(user))
            {
                TempData["Success"] = "Thêm thành công!";
                return RedirectToAction("Index");
            }

            ViewBag.Error = "Lỗi hệ thống (Check lại độ dài SĐT trong DB)!";
            return View(user);
        }

        [HttpGet]
        public IActionResult Edit(int id) => IsAdmin() ? View(bll.GetUserById(id)) : RedirectToAction("Index");

        [HttpPost]
        public IActionResult Edit(UserDTO user)
        {
            if (!IsAdmin()) return RedirectToAction("Index");

            if (bll.UpdateUser(user))
            {
                TempData["Success"] = "Cập nhật thành công!";
                return RedirectToAction("Index");
            }
            return View(user);
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            if (!IsAdmin()) return Json(new { success = false });
            return Json(new { success = bll.DeleteUser(id) });
        }
    }
}