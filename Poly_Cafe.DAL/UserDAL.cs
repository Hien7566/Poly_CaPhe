using Poly_Cafe.DTO;
using Poly_Cafe.Utils;
using System;
using System.Data;
using Microsoft.Data.SqlClient;

namespace Poly_Cafe.DAL
{
    public class UserDAL
    {
        // Đổi kiểu trả về thành UserDTO? (thêm dấu ?) để báo hiệu phương thức có thể trả về null
        public UserDTO? CheckLogin(string email, string password)
        {
            string sql = @"SELECT id, email, full_name, phone, address, birthday, 
                                  gender, role, hire_date, salary, active
                           FROM users 
                           WHERE email = @Email AND password = @Password AND active = 1";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Email", email ?? (object)DBNull.Value),
                new SqlParameter("@Password", password ?? (object)DBNull.Value)
            };

            DataTable dt = DBUtil.ExecuteQuery(sql, parameters);

            if (dt != null && dt.Rows.Count > 0)
            {
                return MapRowToUserDTO(dt.Rows[0]);
            }
            return null;
        }

        public UserDTO? GetUserById(int userId)
        {
            string sql = @"SELECT id, email, full_name, phone, address, birthday, 
                                  gender, role, hire_date, salary, active
                           FROM users 
                           WHERE id = @Id";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Id", userId)
            };

            DataTable dt = DBUtil.ExecuteQuery(sql, parameters);

            if (dt != null && dt.Rows.Count > 0)
            {
                return MapRowToUserDTO(dt.Rows[0]);
            }
            return null;
        }

        // Hàm mapping đã được fix lỗi DBNull và ép kiểu an toàn
        private UserDTO MapRowToUserDTO(DataRow row)
        {
            return new UserDTO
            {
                // Sử dụng toán tử kiểm tra DBNull.Value
                Id = row["id"] != DBNull.Value ? (int)row["id"] : 0,

                // Sử dụng ?.ToString() ?? "" để tránh lỗi null reference
                Email = row["email"]?.ToString() ?? "",
                FullName = row["full_name"]?.ToString() ?? "",
                Phone = row["phone"]?.ToString() ?? "",

                Address = row["address"] != DBNull.Value ? row["address"].ToString() : null,

                // Ép kiểu an toàn cho DateTime và các kiểu Nullable
                Birthday = row["birthday"] != DBNull.Value ? Convert.ToDateTime(row["birthday"]) : (DateTime?)null,
                HireDate = row["hire_date"] != DBNull.Value ? Convert.ToDateTime(row["hire_date"]) : (DateTime?)null,

                Gender = row["gender"] != DBNull.Value ? (bool)row["gender"] : (bool?)null,

                // Các trường bắt buộc không được null trong DB
                Role = row["role"] != DBNull.Value ? (bool)row["role"] : false,
                Active = row["active"] != DBNull.Value ? (bool)row["active"] : false,

                Salary = row["salary"] != DBNull.Value ? Convert.ToInt32(row["salary"]) : (int?)null
            };
        }
    }
}