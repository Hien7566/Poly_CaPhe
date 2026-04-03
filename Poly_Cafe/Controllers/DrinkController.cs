using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Poly_Cafe.BLL;
using Poly_Cafe.DTO;
using System;
using System.IO;

namespace Poly_Cafe.Controllers
{
    public class DrinkController : Controller
    {
        private readonly DrinkBLL _drinkBLL = new DrinkBLL();
        private readonly CategoryBLL _categoryBLL = new CategoryBLL();
        private readonly IWebHostEnvironment _webHostEnvironment;

        public DrinkController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            var list = _drinkBLL.GetAll();
            return View(list);
        }

        public IActionResult Create()
        {
            ViewBag.Categories = _categoryBLL.GetAll();
            return View();
        }

        [HttpPost]
        public IActionResult Create(DrinkDTO drink, IFormFile ImageFile)
        {
            try
            {
                if (ImageFile != null && ImageFile.Length > 0)
                {
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + ImageFile.FileName;
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "image", "drinks");
                    if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        ImageFile.CopyTo(fileStream);
                    }
                    drink.Image = "/image/drinks/" + uniqueFileName;
                }
                _drinkBLL.Insert(drink);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Lỗi thêm món: " + ex.Message;
                ViewBag.Categories = _categoryBLL.GetAll();
                return View(drink);
            }
        }

        // --- 1. HÀM HIỆN FORM CẬP NHẬT (GET) ---
        public IActionResult Edit(int id)
        {
            // Lấy dữ liệu hiện tại từ Database
            var drink = _drinkBLL.GetById(id);
            if (drink == null) return NotFound();

            // Đưa danh mục vào Dropdown
            ViewBag.Categories = _categoryBLL.GetAll();

            // Truyền dữ liệu món ra View để điền vào form
            return View(drink);
        }

        // --- 2. HÀM XỬ LÝ LƯU CẬP NHẬT (POST) ---
        [HttpPost]
        public IActionResult Edit(DrinkDTO drink, IFormFile ImageFile)
        {
            try
            {
                // Lấy món cũ từ DB để so sánh và xóa ảnh cũ
                var oldDrink = _drinkBLL.GetById(drink.Id);

                // A. Nếu người dùng chọn tải lên ảnh mới
                if (ImageFile != null && ImageFile.Length > 0)
                {
                    // 1. Lưu ảnh mới vào thư mục
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + ImageFile.FileName;
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "image", "drinks");
                    if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        ImageFile.CopyTo(fileStream);
                    }
                    // Gán đường dẫn ảnh mới vào DTO
                    drink.Image = "/image/drinks/" + uniqueFileName;

                    // 2. TỰ ĐỘNG XÓA ẢNH CŨ KHỎI THƯ MỤC
                    if (oldDrink != null && !string.IsNullOrEmpty(oldDrink.Image))
                    {
                        // Lấy đường dẫn ảnh cũ thật trên máy chủ
                        string oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, oldDrink.Image.TrimStart('/'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath); // Thực hiện xóa file ảnh cũ
                        }
                    }
                }
                // B. Nếu người dùng KHÔNG chọn ảnh mới
                else
                {
                    // Giữ nguyên đường dẫn ảnh cũ từ DB
                    drink.Image = oldDrink?.Image;
                }

                // 3. Gọi BLL để cập nhật Database
                _drinkBLL.Update(drink);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Lỗi cập nhật: " + ex.Message;
                ViewBag.Categories = _categoryBLL.GetAll();
                return View(drink);
            }
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            _drinkBLL.Delete(id);
            return RedirectToAction("Index");
        }
    }
}