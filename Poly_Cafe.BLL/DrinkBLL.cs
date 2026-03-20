using System;
using System.Collections.Generic;
using Poly_Cafe.DAL;
using Poly_Cafe.DTO;

namespace Poly_Cafe.BLL
{
    public class DrinkBLL
    {
        private DrinkDAL _dal = new DrinkDAL();

        public List<DrinkDTO> GetAll()
        {
            return _dal.GetAll();
        }

        public List<DrinkDTO> GetByCategory(int categoryId)
        {
            if (categoryId <= 0)
                throw new Exception("ID danh mục không hợp lệ");
            return _dal.GetByCategory(categoryId);
        }

        public DrinkDTO GetById(int id)
        {
            if (id <= 0)
                throw new Exception("ID không hợp lệ");
            return _dal.GetById(id);
        }

        public bool Insert(DrinkDTO drink)
        {
            if (drink.CategoryId <= 0)
                throw new Exception("Phải chọn danh mục");
            if (string.IsNullOrWhiteSpace(drink.Name))
                throw new Exception("Tên đồ uống không được để trống");
            if (drink.Price <= 0)
                throw new Exception("Giá không hợp lệ");

            return _dal.Insert(drink);
        }

        public bool Update(DrinkDTO drink)
        {
            if (drink.Id <= 0)
                throw new Exception("ID không hợp lệ");
            if (drink.CategoryId <= 0)
                throw new Exception("Phải chọn danh mục");
            if (string.IsNullOrWhiteSpace(drink.Name))
                throw new Exception("Tên đồ uống không được để trống");
            if (drink.Price <= 0)
                throw new Exception("Giá không hợp lệ");

            return _dal.Update(drink);
        }

        public bool Delete(int id)
        {
            if (id <= 0)
                throw new Exception("ID không hợp lệ");
            return _dal.Delete(id);
        }
    }
}