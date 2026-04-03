using System;
using System.Collections.Generic;
using System.Data;
using Poly_Cafe.DTO;
using Poly_Cafe.Utils; // Khai báo đường dẫn tới file DBUtil của bạn

namespace Poly_Cafe.DAL
{
    public class IngredientDAL
    {
        // 1. LẤY TẤT CẢ NGUYÊN LIỆU
        public List<IngredientDTO> GetAll()
        {
            List<IngredientDTO> list = new List<IngredientDTO>();
            string sql = "SELECT * FROM ingredients ORDER BY id DESC";

            // SỬ DỤNG QueryDataTable THAY VÌ ExecuteQuery
            DataTable dt = DBUtil.QueryDataTable(sql);

            foreach (DataRow row in dt.Rows)
            {
                list.Add(new IngredientDTO
                {
                    Id = Convert.ToInt32(row["id"]),
                    Name = row["name"].ToString(),
                    Stock_Quantity = Convert.ToDecimal(row["stock_quantity"]),
                    Unit = row["unit"] != DBNull.Value ? row["unit"].ToString() : "",
                    Min_Stock = Convert.ToDecimal(row["min_stock"]),
                    Price_Per_Unit = Convert.ToDecimal(row["price_per_unit"]),
                    Supplier = row["supplier"] != DBNull.Value ? row["supplier"].ToString() : "",
                    Active = Convert.ToBoolean(row["active"])
                });
            }
            return list;
        }

        // 2. LẤY 1 NGUYÊN LIỆU THEO ID
        public IngredientDTO GetById(int id)
        {
            string sql = "SELECT * FROM ingredients WHERE id = @p0";

            // DÙNG CHUNG List<object> VÀ QueryDataTable CHO ĐỒNG BỘ
            DataTable dt = DBUtil.QueryDataTable(sql, new List<object> { id });

            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                return new IngredientDTO
                {
                    Id = Convert.ToInt32(row["id"]),
                    Name = row["name"].ToString(),
                    Stock_Quantity = Convert.ToDecimal(row["stock_quantity"]),
                    Unit = row["unit"] != DBNull.Value ? row["unit"].ToString() : "",
                    Min_Stock = Convert.ToDecimal(row["min_stock"]),
                    Price_Per_Unit = Convert.ToDecimal(row["price_per_unit"]),
                    Supplier = row["supplier"] != DBNull.Value ? row["supplier"].ToString() : "",
                    Active = Convert.ToBoolean(row["active"])
                };
            }
            return null;
        }

        // 3. THÊM NGUYÊN LIỆU MỚI
        public bool Insert(IngredientDTO ingredient)
        {
            string sql = @"INSERT INTO ingredients (name, stock_quantity, unit, min_stock, price_per_unit, supplier, active) 
                           VALUES (@p0, @p1, @p2, @p3, @p4, @p5, @p6)";

            return DBUtil.ExecuteNonQuery(sql, new List<object> {
                ingredient.Name,
                ingredient.Stock_Quantity,
                ingredient.Unit ?? "",
                ingredient.Min_Stock,
                ingredient.Price_Per_Unit,
                ingredient.Supplier ?? "",
                ingredient.Active
            }) > 0;
        }

        // 4. CẬP NHẬT NGUYÊN LIỆU
        public bool Update(IngredientDTO ingredient)
        {
            string sql = @"UPDATE ingredients 
                           SET name = @p0, stock_quantity = @p1, unit = @p2, 
                               min_stock = @p3, price_per_unit = @p4, supplier = @p5, active = @p6 
                           WHERE id = @p7";

            return DBUtil.ExecuteNonQuery(sql, new List<object> {
                ingredient.Name,
                ingredient.Stock_Quantity,
                ingredient.Unit ?? "",
                ingredient.Min_Stock,
                ingredient.Price_Per_Unit,
                ingredient.Supplier ?? "",
                ingredient.Active,
                ingredient.Id
            }) > 0;
        }

        // 5. XÓA NGUYÊN LIỆU
        public bool Delete(int id)
        {
            string sql = "DELETE FROM ingredients WHERE id = @p0";

            return DBUtil.ExecuteNonQuery(sql, new List<object> { id }) > 0;
        }
    }
}