using System.Collections.Generic;
using Poly_Cafe.DAL;
using Poly_Cafe.DTO;

namespace Poly_Cafe.BLL
{
    public class RecipeBLL
    {
        private readonly RecipeDAL _recipeDAL = new RecipeDAL();

        public List<RecipeDTO> GetAll() => _recipeDAL.GetAll();
        public RecipeDTO GetById(int id) => _recipeDAL.GetById(id);
        public bool Insert(RecipeDTO recipe) => _recipeDAL.Insert(recipe);
        public bool Update(RecipeDTO recipe) => _recipeDAL.Update(recipe);
        public bool Delete(int id) => _recipeDAL.Delete(id);
    }
}