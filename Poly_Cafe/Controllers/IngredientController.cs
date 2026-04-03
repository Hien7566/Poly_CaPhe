using System;
using Microsoft.AspNetCore.Mvc;
using Poly_Cafe.BLL;
using Poly_Cafe.DTO;

namespace Poly_Cafe.Controllers
{
    public class IngredientController : Controller
    {
        private readonly IngredientBLL _ingredientBLL = new IngredientBLL();

        // 1. HIỂN THỊ DANH SÁCH (Trang Index)
        public IActionResult Index()
        {
            var list = _ingredientBLL.GetAll();
            return View(list);
        }

        // 2. HIỂN THỊ FORM THÊM MỚI (GET)
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // 3. XỬ LÝ THÊM MỚI (POST)
        [HttpPost]
        public IActionResult Create(IngredientDTO ingredient)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _ingredientBLL.Insert(ingredient);
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Lỗi khi thêm nguyên liệu: " + ex.Message;
            }
            return View(ingredient);
        }

        // 4. HIỂN THỊ FORM SỬA (GET)
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var ingredient = _ingredientBLL.GetById(id);
            if (ingredient == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy nguyên liệu này!";
                return RedirectToAction("Index");
            }
            return View(ingredient);
        }

        // 5. XỬ LÝ SỬA (POST)
        [HttpPost]
        public IActionResult Edit(IngredientDTO ingredient)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _ingredientBLL.Update(ingredient);
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Lỗi khi cập nhật nguyên liệu: " + ex.Message;
            }
            return View(ingredient);
        }

        // 6. XỬ LÝ XÓA (POST)
        [HttpPost]
        public IActionResult Delete(int id)
        {
            try
            {
                _ingredientBLL.Delete(id);
            }
            catch (Exception ex)
            {
                // Bắt lỗi khóa ngoại (VD: Nguyên liệu đang được dùng trong Công thức)
                TempData["ErrorMessage"] = "Không thể xóa! Nguyên liệu này đang được sử dụng trong hệ thống.";
            }
            return RedirectToAction("Index");
        }
    }
}