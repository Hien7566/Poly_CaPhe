using System;
using System.Collections.Generic;
using System.Data;
using Poly_Cafe.DTO;
using Poly_Cafe.Utils;

namespace Poly_Cafe.DAL
{
    public class CategoryDAL
    {
        public List<CategoryDTO> GetAll()
        {
            List<CategoryDTO> list = new List<CategoryDTO>();
            string sql = "SELECT * FROM categories WHERE active = 1 ORDER BY name";
            DataTable dt = DBUtil.QueryDataTable(sql, null);

            foreach (DataRow row in dt.Rows)
            {
                list.Add(new CategoryDTO
                {
                    Id = Convert.ToInt32(row["id"]),
                    Name = row["name"].ToString(),
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
                Active = Convert.ToBoolean(row["active"])
            };
        }

        public bool Insert(CategoryDTO category)
        {
            string sql = "INSERT INTO categories (name, active) VALUES (@p0, @p1)";
            return DBUtil.ExecuteNonQuery(sql, new List<object> { category.Name, true }) > 0;
        }

        public bool Update(CategoryDTO category)
        {
            string sql = "UPDATE categories SET name = @p0 WHERE id = @p1";
            return DBUtil.ExecuteNonQuery(sql, new List<object> { category.Name, category.Id }) > 0;
        }

        public bool Delete(int id)
        {
            string sql = "UPDATE categories SET active = 0 WHERE id = @p0";
            return DBUtil.ExecuteNonQuery(sql, new List<object> { id }) > 0;
        }
    }
}