using Microsoft.AspNetCore.Mvc;
using Poly_Cafe.BLL;
using Poly_Cafe.DTO;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Poly_Cafe.Controllers
{
    public class ShiftController : Controller
    {
        private readonly ShiftBLL _shiftBll = new ShiftBLL();

        public IActionResult Index()
        {
            var userRoleStr = HttpContext.Session.GetString("UserRole");
            var currentUserId = HttpContext.Session.GetInt32("UserId");

            if (currentUserId == null) return RedirectToAction("Login", "Account");

            // Lấy danh sách toàn bộ ca
            var allSessions = _shiftBll.GetListSessions() ?? new List<ShiftDTO>();

            // THỐNG KÊ cho 3 ô Stat Card (Dùng ViewBag để View hiển thị)
            ViewBag.ActiveShifts = allSessions.Count(x => x.StartTime.HasValue && !x.EndTime.HasValue);
            ViewBag.ClosedCount = allSessions.Count(x => x.EndTime.HasValue);
            ViewBag.TotalRevenue = allSessions.Sum(x => x.TotalSales);

            // PHÂN QUYỀN HIỂN THỊ
            var displayData = allSessions;
            if (userRoleStr != "True")
            {
                // Nhân viên chỉ thấy ca trống hoặc ca của mình
                displayData = allSessions.Where(x => x.StartTime == null || x.UserId == currentUserId).ToList();
            }

            return View(displayData);
        }

        // --- CHỨC NĂNG VẬN HÀNH (VÀO CA / CHỐT CA) ---

        [HttpPost]
        public IActionResult StartNewShift(int sessionId)
        {
            try
            {
                int? currentUserId = HttpContext.Session.GetInt32("UserId");
                if (currentUserId == null) return Json(new { success = false, message = "Hết phiên làm việc!" });

                bool result = _shiftBll.StartNewShift(sessionId, currentUserId.Value);
                return Json(new { success = result });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public IActionResult EndShift(int sessionId)
        {
            try
            {
                double autoSales = _shiftBll.CalculateShiftRevenue(sessionId);
                bool result = _shiftBll.EndShift(sessionId);
                return Json(new { success = result, totalSales = autoSales });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // --- CHỨC NĂNG QUẢN TRỊ (THÊM - XÓA - SỬA) CHO ADMIN ---

        [HttpPost]
        public IActionResult CreateManualShift(string name)
        {
            // Kiểm tra quyền Admin (True)
            if (HttpContext.Session.GetString("UserRole") != "True")
                return Json(new { success = false, message = "Bạn không có quyền!" });

            bool res = _shiftBll.CreateBlankShift(name);
            return Json(new { success = res });
        }

        [HttpPost]
        public IActionResult UpdateShiftName(int id, string name)
        {
            if (HttpContext.Session.GetString("UserRole") != "True")
                return Json(new { success = false, message = "Bạn không có quyền!" });

            bool res = _shiftBll.UpdateShiftName(id, name);
            return Json(new { success = res });
        }

        [HttpPost]
        public IActionResult DeleteShift(int id)
        {
            if (HttpContext.Session.GetString("UserRole") != "True")
                return Json(new { success = false, message = "Bạn không có quyền!" });

            try
            {
                bool res = _shiftBll.DeleteShift(id);
                return Json(new { success = res });
            }
            catch
            {
                return Json(new { success = false, message = "Ca này đã có dữ liệu lịch sử, không thể xóa!" });
            }
        }
    }
}