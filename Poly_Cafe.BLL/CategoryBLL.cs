using System.Collections.Generic;
using Poly_Cafe.DAL;
using Poly_Cafe.DTO;

namespace Poly_Cafe.BLL
{
    public class CategoryBLL
    {
        private readonly CategoryDAL _categoryDAL = new CategoryDAL();

        public List<CategoryDTO> GetAll()
        {
            return _categoryDAL.GetAll();
        }

        public CategoryDTO GetById(int id)
        {
            return _categoryDAL.GetById(id);
        }

        // Đã sửa tham số từ (string name) thành (CategoryDTO category) để khớp với Controller và DAL
        public bool Insert(CategoryDTO category)
        {
            return _categoryDAL.Insert(category);
        }

        // Đã sửa tham số từ (int id, string name) thành (CategoryDTO category)
        public bool Update(CategoryDTO category)
        {
            return _categoryDAL.Update(category);
        }

        public bool Delete(int id)
        {
            return _categoryDAL.Delete(id);
        }
    }
}