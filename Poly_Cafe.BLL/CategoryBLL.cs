using System;
using System.Collections.Generic;
using Poly_Cafe.DAL;
using Poly_Cafe.DTO;

namespace Poly_Cafe.BLL
{
    public class CategoryBLL
    {
        private CategoryDAL _dal = new CategoryDAL();

        public List<CategoryDTO> GetAll()
        {
            return _dal.GetAll();
        }

        public CategoryDTO GetById(int id)
        {
            if (id <= 0)
                throw new Exception("ID không hợp lệ");
            return _dal.GetById(id);
        }

        public bool Insert(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new Exception("Tên danh mục không được để trống");

            return _dal.Insert(new CategoryDTO { Name = name });
        }

        public bool Update(int id, string name)
        {
            if (id <= 0)
                throw new Exception("ID không hợp lệ");
            if (string.IsNullOrWhiteSpace(name))
                throw new Exception("Tên danh mục không được để trống");

            return _dal.Update(new CategoryDTO { Id = id, Name = name });
        }

        public bool Delete(int id)
        {
            if (id <= 0)
                throw new Exception("ID không hợp lệ");
            return _dal.Delete(id);
        }
    }
}