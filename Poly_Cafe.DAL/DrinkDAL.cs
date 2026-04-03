using System;
using System.Collections.Generic;
using System.Data;
using Poly_Cafe.DTO;
using Poly_Cafe.Utils;

namespace Poly_Cafe.DAL
{
    public class DrinkDAL
    {
        // 1. Lấy danh sách đồ uống (Sắp xếp món mới nhất lên đầu)
        public List<DrinkDTO> GetAll()
        {
            List<DrinkDTO> list = new List<DrinkDTO>();
            string sql = @"SELECT d.*, c.name as CategoryName 
                          FROM drinks d 
                          JOIN categories c ON d.category_id = c.id 
                          WHERE d.active = 1 
                          ORDER BY d.id DESC";

            DataTable dt = DBUtil.QueryDataTable(sql, null);

            foreach (DataRow row in dt.Rows)
            {
                list.Add(new DrinkDTO
                {
                    Id = Convert.ToInt32(row["id"]),
                    CategoryId = Convert.ToInt32(row["category_id"]),
                    CategoryName = row["CategoryName"].ToString(),
                    Name = row["name"].ToString(),
                    Price = Convert.ToDecimal(row["price"]),
                    // CHỈ lấy tên file từ DB, không cộng thêm đường dẫn ở đây để tránh sai lệch với View
                    Image = row["image"] != DBNull.Value ? row["image"].ToString() : string.Empty,
                    Description = row["description"] != DBNull.Value ? row["description"].ToString() : "",
                    Active = Convert.ToBoolean(row["active"])
                });
            }
            return list;
        }

        // 2. Lấy danh sách theo Danh mục
        public List<DrinkDTO> GetByCategory(int categoryId)
        {
            List<DrinkDTO> list = new List<DrinkDTO>();
            // Cập nhật SQL để lấy luôn CategoryName cho đồng bộ
            string sql = @"SELECT d.*, c.name as CategoryName 
                          FROM drinks d 
                          JOIN categories c ON d.category_id = c.id 
                          WHERE d.category_id = @p0 AND d.active = 1";
            DataTable dt = DBUtil.QueryDataTable(sql, new List<object> { categoryId });

            foreach (DataRow row in dt.Rows)
            {
                list.Add(new DrinkDTO
                {
                    Id = Convert.ToInt32(row["id"]),
                    CategoryId = Convert.ToInt32(row["category_id"]),
                    CategoryName = row["CategoryName"].ToString(),
                    Name = row["name"].ToString(),
                    Price = Convert.ToDecimal(row["price"]),
                    Image = row["image"] != DBNull.Value ? row["image"].ToString() : string.Empty,
                    Active = Convert.ToBoolean(row["active"])
                });
            }
            return list;
        }

        // 3. Lấy 1 món đồ uống theo ID (Dùng cho chức năng Sửa)
        public DrinkDTO GetById(int id)
        {
            string sql = "SELECT * FROM drinks WHERE id = @p0 AND active = 1";
            DataTable dt = DBUtil.QueryDataTable(sql, new List<object> { id });

            if (dt.Rows.Count == 0) return null;

            DataRow row = dt.Rows[0];
            return new DrinkDTO
            {
                Id = Convert.ToInt32(row["id"]),
                CategoryId = Convert.ToInt32(row["category_id"]),
                Name = row["name"].ToString(),
                Price = Convert.ToDecimal(row["price"]),
                Image = row["image"] != DBNull.Value ? row["image"].ToString() : string.Empty,
                Description = row["description"] != DBNull.Value ? row["description"].ToString() : "",
                Active = Convert.ToBoolean(row["active"])
            };
        }

        // 4. Thêm mới đồ uống
        public bool Insert(DrinkDTO drink)
        {
            string sql = @"INSERT INTO drinks (category_id, name, price, image, description, active) 
                          VALUES (@p0, @p1, @p2, @p3, @p4, @p5)";

            return DBUtil.ExecuteNonQuery(sql, new List<object> {
                drink.CategoryId,
                drink.Name,
                drink.Price,
                drink.Image ?? "",
                drink.Description ?? "",
                true // Mặc định khi thêm mới là active = 1
            }) > 0;
        }

        // 5. Cập nhật đồ uống
        public bool Update(DrinkDTO drink)
        {
            string sql = @"UPDATE drinks 
                          SET category_id = @p0, name = @p1, price = @p2, image = @p3, description = @p4 
                          WHERE id = @p5";

            return DBUtil.ExecuteNonQuery(sql, new List<object> {
                drink.CategoryId,
                drink.Name,
                drink.Price,
                drink.Image ?? "",
                drink.Description ?? "",
                drink.Id
            }) > 0;
        }

        // 6. Xóa đồ uống (Soft delete - Ẩn khỏi hệ thống)
        public bool Delete(int id)
        {
            string sql = "UPDATE drinks SET active = 0 WHERE id = @p0";
            return DBUtil.ExecuteNonQuery(sql, new List<object> { id }) > 0;
        }
    }
}