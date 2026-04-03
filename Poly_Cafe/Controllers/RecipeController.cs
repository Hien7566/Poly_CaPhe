using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Poly_Cafe.BLL;
using Poly_Cafe.DAL;
using Poly_Cafe.DTO;

namespace Poly_Cafe.Controllers
{
    public class RecipeController : Controller
    {
        private readonly RecipeBLL _recipeBLL = new RecipeBLL();

        // Cần lấy danh sách Đồ uống và Nguyên liệu để làm menu chọn (Dropdown)
        private readonly DrinkDAL _drinkDAL = new DrinkDAL();
        private readonly IngredientDAL _ingredientDAL = new IngredientDAL();

        private void LoadDropdowns()
        {
            // Gửi danh sách qua ViewBag để hiển thị trên View Create/Edit
            ViewBag.Drinks = new SelectList(_drinkDAL.GetAll(), "Id", "Name");
            ViewBag.Ingredients = new SelectList(_ingredientDAL.GetAll(), "Id", "Name");
        }

        public IActionResult Index()
        {
            var list = _recipeBLL.GetAll();
            return View(list);
        }

        [HttpGet]
        public IActionResult Create()
        {
            LoadDropdowns();
            return View();
        }

        [HttpPost]
        public IActionResult Create(RecipeDTO recipe)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _recipeBLL.Insert(recipe);
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Lỗi khi thêm công thức: " + ex.Message;
            }
            LoadDropdowns();
            return View(recipe);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var recipe = _recipeBLL.GetById(id);
            if (recipe == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy công thức này!";
                return RedirectToAction("Index");
            }
            LoadDropdowns();
            return View(recipe);
        }

        [HttpPost]
        public IActionResult Edit(RecipeDTO recipe)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _recipeBLL.Update(recipe);
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Lỗi khi cập nhật công thức: " + ex.Message;
            }
            LoadDropdowns();
            return View(recipe);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            try
            {
                _recipeBLL.Delete(id);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Không thể xóa công thức này!";
            }
            return RedirectToAction("Index");
        }
    }
}