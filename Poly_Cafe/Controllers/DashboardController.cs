using Microsoft.AspNetCore.Mvc;
using Poly_Cafe.DTO;
using System.Collections.Generic; // Đảm bảo có dòng này để dùng List

namespace Poly_Cafe.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            // 1. Kiểm tra đăng nhập
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("Login", "Home");
            }

            // 2. Khởi tạo dữ liệu đầy đủ để không bị lỗi Null ngoài View
            var data = new DashboardDTO
            {
                // Phần thông tin chung
                TodayDate = DateTime.Now.ToString("dddd, dd/MM/yyyy"),
                TotalSalesToday = 72500000,
                TotalOrders = 156,
                LowStockItems = 8,
                ActiveCashiers = 4,

                // Khởi tạo các danh sách (Bắt buộc phải có để không lỗi giao diện)
                SalesTrend = new List<SalesTrendData>
                {
                    new SalesTrendData { Day = "Thứ 2", Sales = 1500000 },
                    new SalesTrendData { Day = "Thứ 3", Sales = 2400000 },
                    new SalesTrendData { Day = "Thứ 4", Sales = 2100000 }
                },

                Expenses = new List<ExpenseData>
                {
                    new ExpenseData { Name = "Nguyên liệu", Percentage = 45 },
                    new ExpenseData { Name = "Lương NV", Percentage = 30 }
                },

                TopProducts = new List<TopProductData>
                {
                    new TopProductData { ProductName = "Bạc xỉu", UnitsSold = 450 },
                    new TopProductData { ProductName = "Cà phê muối", UnitsSold = 380 }
                },

                LowStockAlerts = new List<LowStockAlertData>
                {
                    new LowStockAlertData { ProductName = "Hạt Arabica", Stock = "2kg" },
                    new LowStockAlertData { ProductName = "Sữa tươi", Stock = "5 lít" }
                }
            };

            // 3. Truyền data sang View
            return View(data);
        }

        public IActionResult SalesReport()
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("Login", "Home");
            }
            return View();
        }
    }
}