using System;
using Microsoft.AspNetCore.Mvc;
using Poly_Cafe.BLL;
using Poly_Cafe.DTO;

namespace Poly_Cafe.Controllers
{
    public class CustomerController : Controller
    {
        private readonly CustomerBLL _customerBLL = new CustomerBLL();

        public IActionResult Index()
        {
            var list = _customerBLL.GetAll();
            return View(list);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CustomerDTO customer)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Mặc định khách hàng mới sẽ là hạng "Thường" và tổng chi tiêu = 0
                    customer.Total_Spent = 0;
                    if (string.IsNullOrEmpty(customer.Customer_Type))
                        customer.Customer_Type = "Thường";

                    _customerBLL.Insert(customer);
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Lỗi khi thêm khách hàng: " + ex.Message;
            }
            return View(customer);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var customer = _customerBLL.GetById(id);
            if (customer == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy khách hàng này!";
                return RedirectToAction("Index");
            }
            return View(customer);
        }

        [HttpPost]
        public IActionResult Edit(CustomerDTO customer)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Logic tự động cập nhật hạng thành viên theo mức chi tiêu
                    if (customer.Total_Spent >= 3000000) customer.Customer_Type = "VVIP";
                    else if (customer.Total_Spent >= 1000000) customer.Customer_Type = "VIP";
                    else if (customer.Total_Spent >= 500000) customer.Customer_Type = "Thân thiết";
                    else customer.Customer_Type = "Thường";

                    _customerBLL.Update(customer);
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Lỗi khi cập nhật khách hàng: " + ex.Message;
            }
            return View(customer);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            try
            {
                _customerBLL.Delete(id);
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Không thể xóa! Khách hàng này đã có hóa đơn trong hệ thống.";
            }
            return RedirectToAction("Index");
        }
    }
}