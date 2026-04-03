using System;
using System.Collections.Generic;
using System.Data;
using Poly_Cafe.DTO;
using Poly_Cafe.Utils;

namespace Poly_Cafe.DAL
{
    public class UserDAL
    {
        // 1. Lấy danh sách (chỉ lấy active = 1)
        public List<UserDTO> GetAll()
        {
            List<UserDTO> list = new List<UserDTO>();
            string sql = "SELECT * FROM users WHERE active = 1";
            DataTable dt = DBUtil.QueryDataTable(sql, null);
            if (dt != null)
            {
                foreach (DataRow row in dt.Rows)
                {
                    list.Add(new UserDTO
                    {
                        Id = Convert.ToInt32(row["id"]),
                        Email = row["email"].ToString(),
                        FullName = row["full_name"].ToString(),
                        Phone = row["phone"]?.ToString(),
                        Role = Convert.ToBoolean(row["role"]),
                        Active = Convert.ToBoolean(row["active"])
                    });
                }
            }
            return list;
        }

        // 2. Kiểm tra trùng Email
        public bool IsEmailExists(string email)
        {
            string sql = "SELECT COUNT(*) FROM users WHERE email = @p0";
            object result = DBUtil.ExecuteScalar(sql, new List<object> { email });
            return Convert.ToInt32(result) > 0;
        }

        // 3. Thêm mới
        public bool Add(UserDTO u)
        {
            string sql = "INSERT INTO users (email, password, full_name, phone, role, active) VALUES (@p0, @p1, @p2, @p3, @p4, 1)";
            List<object> parameters = new List<object> { u.Email, u.Password, u.FullName, u.Phone, u.Role };
            return DBUtil.ExecuteNonQuery(sql, parameters) > 0;
        }

        // 4. Lấy theo ID
        public UserDTO GetById(int id)
        {
            string sql = "SELECT * FROM users WHERE id = @p0";
            DataTable dt = DBUtil.QueryDataTable(sql, new List<object> { id });
            if (dt == null || dt.Rows.Count == 0) return null;
            DataRow row = dt.Rows[0];
            return new UserDTO
            {
                Id = (int)row["id"],
                Email = row["email"].ToString(),
                FullName = row["full_name"].ToString(),
                Phone = row["phone"]?.ToString(),
                Address = row["address"]?.ToString(),
                Role = Convert.ToBoolean(row["role"]),
                Active = Convert.ToBoolean(row["active"])
            };
        }

        // 5. Cập nhật
        public bool Update(UserDTO u)
        {
            string sql = "UPDATE users SET full_name = @p0, phone = @p1, address = @p2, role = @p3 WHERE id = @p4";
            List<object> parameters = new List<object> { u.FullName, u.Phone, u.Address, u.Role, u.Id };
            return DBUtil.ExecuteNonQuery(sql, parameters) > 0;
        }

        // 6. Xóa mềm
        public bool Delete(int id)
        {
            string sql = "UPDATE users SET active = 0 WHERE id = @p0";
            return DBUtil.ExecuteNonQuery(sql, new List<object> { id }) > 0;
        }

        // 7. Đăng nhập
        public UserDTO CheckLogin(string email, string password)
        {
            string sql = "SELECT * FROM users WHERE email = @p0 AND password = @p1 AND active = 1";
            DataTable dt = DBUtil.QueryDataTable(sql, new List<object> { email, password });
            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                return new UserDTO
                {
                    Id = Convert.ToInt32(row["id"]),
                    Email = row["email"].ToString(),
                    FullName = row["full_name"].ToString(),
                    Role = Convert.ToBoolean(row["role"]),
                    Active = Convert.ToBoolean(row["active"])
                };
            }
            return null;
        }
    }
}