using System;
using System.Collections.Generic;
using System.Data;
using Poly_Cafe.DTO;
using Poly_Cafe.Utils;

namespace Poly_Cafe.DAL
{
    public class RecipeDAL
    {
        public List<RecipeDTO> GetAll()
        {
            List<RecipeDTO> list = new List<RecipeDTO>();
            // Dùng JOIN để lấy tên Đồ uống và tên Nguyên liệu
            string sql = @"SELECT r.*, d.name AS DrinkName, i.name AS IngredientName 
                           FROM recipes r
                           JOIN drinks d ON r.drink_id = d.id
                           JOIN ingredients i ON r.ingredient_id = i.id
                           ORDER BY r.drink_id ASC, r.id ASC";

            DataTable dt = DBUtil.QueryDataTable(sql);

            foreach (DataRow row in dt.Rows)
            {
                list.Add(new RecipeDTO
                {
                    Id = Convert.ToInt32(row["id"]),
                    Drink_Id = Convert.ToInt32(row["drink_id"]),
                    Ingredient_Id = Convert.ToInt32(row["ingredient_id"]),
                    Quantity = Convert.ToDecimal(row["quantity"]),
                    Unit = row["unit"] != DBNull.Value ? row["unit"].ToString() : "",
                    Instructions = row["instructions"] != DBNull.Value ? row["instructions"].ToString() : "",
                    Created_At = row["created_at"] != DBNull.Value ? Convert.ToDateTime(row["created_at"]) : DateTime.Now,
                    DrinkName = row["DrinkName"].ToString(),
                    IngredientName = row["IngredientName"].ToString()
                });
            }
            return list;
        }

        public RecipeDTO GetById(int id)
        {
            string sql = "SELECT * FROM recipes WHERE id = @p0";
            DataTable dt = DBUtil.QueryDataTable(sql, new List<object> { id });

            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                return new RecipeDTO
                {
                    Id = Convert.ToInt32(row["id"]),
                    Drink_Id = Convert.ToInt32(row["drink_id"]),
                    Ingredient_Id = Convert.ToInt32(row["ingredient_id"]),
                    Quantity = Convert.ToDecimal(row["quantity"]),
                    Unit = row["unit"] != DBNull.Value ? row["unit"].ToString() : "",
                    Instructions = row["instructions"] != DBNull.Value ? row["instructions"].ToString() : ""
                };
            }
            return null;
        }

        public bool Insert(RecipeDTO recipe)
        {
            // created_at tự động lấy GETDATE() của SQL
            string sql = @"INSERT INTO recipes (drink_id, ingredient_id, quantity, unit, instructions, created_at) 
                           VALUES (@p0, @p1, @p2, @p3, @p4, GETDATE())";

            return DBUtil.ExecuteNonQuery(sql, new List<object> {
                recipe.Drink_Id,
                recipe.Ingredient_Id,
                recipe.Quantity,
                recipe.Unit ?? "",
                recipe.Instructions ?? ""
            }) > 0;
        }

        public bool Update(RecipeDTO recipe)
        {
            string sql = @"UPDATE recipes 
                           SET drink_id = @p0, ingredient_id = @p1, quantity = @p2, unit = @p3, instructions = @p4 
                           WHERE id = @p5";

            return DBUtil.ExecuteNonQuery(sql, new List<object> {
                recipe.Drink_Id,
                recipe.Ingredient_Id,
                recipe.Quantity,
                recipe.Unit ?? "",
                recipe.Instructions ?? "",
                recipe.Id
            }) > 0;
        }

        public bool Delete(int id)
        {
            string sql = "DELETE FROM recipes WHERE id = @p0";
            return DBUtil.ExecuteNonQuery(sql, new List<object> { id }) > 0;
        }
    }
}