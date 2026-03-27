using System.Collections.Generic;
using Poly_Cafe.DAL;
using Poly_Cafe.DTO;

namespace Poly_Cafe.BLL
{
    public class IngredientBLL
    {
        private readonly IngredientDAL _ingredientDAL = new IngredientDAL();

        public List<IngredientDTO> GetAll()
        {
            return _ingredientDAL.GetAll();
        }

        public IngredientDTO GetById(int id)
        {
            return _ingredientDAL.GetById(id);
        }

        public bool Insert(IngredientDTO ingredient)
        {
            // Có thể thêm các logic kiểm tra (Validation) ở đây trước khi gọi DAL
            return _ingredientDAL.Insert(ingredient);
        }

        public bool Update(IngredientDTO ingredient)
        {
            return _ingredientDAL.Update(ingredient);
        }

        public bool Delete(int id)
        {
            return _ingredientDAL.Delete(id);
        }
    }
}