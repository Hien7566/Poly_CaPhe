using Microsoft.AspNetCore.Mvc;
using Poly_Cafe.BLL;
using Poly_Cafe.DTO;
using System;

namespace Poly_Cafe.Controllers
{
    public class CategoryController : Controller
    {
        private readonly CategoryBLL _categoryBLL = new CategoryBLL();

        // 1. HIỂN THỊ DANH SÁCH
        public IActionResult Index()
        {
            var list = _categoryBLL.GetAll();
            return View(list);
        }

        // 2. HIỆN FORM THÊM MỚI
        public IActionResult Create()
        {
            return View();
        }

        // 3. XỬ LÝ LƯU THÊM MỚI
        [HttpPost]
        public IActionResult Create(CategoryDTO category)
        {
            try
            {
                _categoryBLL.Insert(category);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Lỗi thêm mới: " + ex.Message;
                return View(category);
            }
        }

        // 4. HIỆN FORM CHỈNH SỬA
        public IActionResult Edit(int id)
        {
            var category = _categoryBLL.GetById(id);
            if (category == null) return NotFound();
            return View(category);
        }

        // 5. XỬ LÝ LƯU CẬP NHẬT
        [HttpPost]
        public IActionResult Edit(CategoryDTO category)
        {
            try
            {
                _categoryBLL.Update(category);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Lỗi cập nhật: " + ex.Message;
                return View(category);
            }
        }

        // 6. XỬ LÝ XÓA
        [HttpPost]
        public IActionResult Delete(int id)
        {
            try
            {
                _categoryBLL.Delete(id);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Lỗi xóa: " + ex.Message;
            }
            return RedirectToAction("Index");
        }
    }
}