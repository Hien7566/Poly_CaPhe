using System;
using System.Collections.Generic;
using System.Data;
using Poly_Cafe.DTO;
using Poly_Cafe.Utils;

namespace Poly_Cafe.DAL
{
    public class CategoryDAL
    {
        // 1. LẤY TẤT CẢ (Lưu ý: Đã bỏ WHERE active = 1 để lấy ra cả danh mục bị Tạm dừng/Màu đỏ)
        public List<CategoryDTO> GetAll()
        {
            List<CategoryDTO> list = new List<CategoryDTO>();
            // Lấy tất cả, sắp xếp mới nhất lên đầu
            string sql = "SELECT * FROM categories ORDER BY id DESC";
            DataTable dt = DBUtil.QueryDataTable(sql, null);

            foreach (DataRow row in dt.Rows)
            {
                list.Add(new CategoryDTO
                {
                    Id = Convert.ToInt32(row["id"]),
                    Name = row["name"].ToString(),
                    Description = row["description"] != DBNull.Value ? row["description"].ToString() : "",

                    // Lấy trạng thái xanh/đỏ từ SQL ra
                    Active = Convert.ToBoolean(row["active"])
                });
            }
            return list;
        }

        public CategoryDTO GetById(int id)
        {
            string sql = "SELECT * FROM categories WHERE id = @p0";
            DataTable dt = DBUtil.QueryDataTable(sql, new List<object> { id });

            if (dt.Rows.Count == 0) return null;

            DataRow row = dt.Rows[0];
            return new CategoryDTO
            {
                Id = Convert.ToInt32(row["id"]),
                Name = row["name"].ToString(),
                Description = row["description"] != DBNull.Value ? row["description"].ToString() : "",
                Active = Convert.ToBoolean(row["active"])
            };
        }

        // 2. THÊM MỚI (Đã thêm biến Active vào SQL)
        public bool Insert(CategoryDTO category)
        {
            string sql = "INSERT INTO categories (name, description, active) VALUES (@p0, @p1, @p2)";
            return DBUtil.ExecuteNonQuery(sql, new List<object> {
                category.Name,
                category.Description ?? "",
                category.Active // Lưu trạng thái người dùng chọn lúc thêm
            }) > 0;
        }

        // 3. CẬP NHẬT (Quan trọng nhất: Đã thêm active = @p2 vào SQL)
        public bool Update(CategoryDTO category)
        {
            string sql = "UPDATE categories SET name = @p0, description = @p1, active = @p2 WHERE id = @p3";
            return DBUtil.ExecuteNonQuery(sql, new List<object> {
                category.Name,
                category.Description ?? "",
                category.Active, // Cập nhật lại trạng thái mới vào SQL
                category.Id
            }) > 0;
        }

        // 4. XÓA (Xóa vĩnh viễn - Hard Delete)
        public bool Delete(int id)
        {
            // Đổi lệnh từ UPDATE thành DELETE để xóa bay màu khỏi Database
            string sql = "DELETE FROM categories WHERE id = @p0";
            return DBUtil.ExecuteNonQuery(sql, new List<object> { id }) > 0;
        }
    }
}