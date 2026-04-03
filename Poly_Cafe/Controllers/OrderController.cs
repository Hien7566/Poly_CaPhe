using Microsoft.AspNetCore.Mvc;
using Poly_Cafe.DTO;
using Poly_Cafe.DAL;
using System.Collections.Generic;
using System;
using System.Linq;
using Microsoft.AspNetCore.Http; // Cần cái này để dùng Session

namespace Poly_Cafe.Controllers
{
    public class OrderController : Controller
    {
        OrderDAL _dal = new OrderDAL();

        // 1. Hiển thị danh sách - Lọc theo Tab (Đang bán / Đã hủy)
        public IActionResult Index(string status)
        {
            var list = _dal.GetListOrders();

            if (status == "-1")
            {
                list = list.Where(x => x.Status == "-1").ToList();
            }
            else
            {
                list = list.Where(x => x.Status == "0" || x.Status == "1").ToList();
            }

            ViewBag.CurrentStatus = status;
            return View(list);
        }

        // 2. Lấy chi tiết Bill cho Modal
        [HttpGet]
        public IActionResult GetDetails(int id)
        {
            var details = _dal.GetDetailsByBillId(id);
            var allOrders = _dal.GetListOrders();
            var currentOrder = allOrders.FirstOrDefault(x => x.Id == id);

            if (currentOrder != null)
            {
                ViewBag.OrderCode = currentOrder.Code;
                ViewBag.OrderDate = currentOrder.CreatedAt?.ToString("HH:mm dd/MM/yyyy");
                ViewBag.FinalTotal = currentOrder.FinalTotal;
                ViewBag.Status = currentOrder.Status;
            }

            return PartialView("_OrderDetails", details);
        }

        // 3. Duyệt hoặc Hủy đơn
        [HttpPost]
        public JsonResult UpdateStatus(int id, string status)
        {
            string dbStatus = "0";
            if (status == "completed" || status == "1") dbStatus = "1";
            else if (status == "cancel" || status == "-1") dbStatus = "-1";

            bool res = _dal.UpdateStatus(id, dbStatus);
            return Json(new { success = res });
        }

        // 4. API tạo đơn hàng từ POS - ĐÃ SỬA ĐỂ LẤY ĐÚNG NHÂN VIÊN
        [HttpPost]
        public JsonResult CreateOrder([FromBody] OrderRequest data)
        {
            if (data == null || data.Details == null || data.Details.Count == 0)
                return Json(new { success = false, message = "Đơn hàng trống!" });

            // LẤY ID NHÂN VIÊN TỪ SESSION (Ông kiểm tra tên key "UserId" cho đúng lúc Login nhé)
            int userId = HttpContext.Session.GetInt32("UserId") ?? 1;

            string newCode = "HD" + DateTime.Now.ToString("ddMMHHmm");

            // TRUYỀN THÊM userId vào hàm InsertBill (Khớp với DAL đã sửa)
            string result = _dal.InsertBill(userId, newCode, data.Total, data.PaymentMethod, data.Details);

            return Json(new { success = result != null, orderCode = result });
        }

        public class OrderRequest 
        {
            public string PaymentMethod { get; set; }
            public int Total { get; set; }
            public List<OrderDetailDTO> Details { get; set; }
        }
    }
}