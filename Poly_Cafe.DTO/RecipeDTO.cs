using System;

namespace Poly_Cafe.DTO
{
    public class RecipeDTO
    {
        public int Id { get; set; }
        public int Drink_Id { get; set; }
        public int Ingredient_Id { get; set; }
        public decimal Quantity { get; set; }
        public string Unit { get; set; }
        public string Instructions { get; set; }
        public DateTime Created_At { get; set; }

        // --- Các thuộc tính bổ sung để hiển thị tên ra View ---
        public string DrinkName { get; set; }
        public string IngredientName { get; set; }
    }
}