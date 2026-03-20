using Poly_Cafe.DTO;
using Poly_Cafe.Utils;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Poly_Cafe.DAL
{
    public class UserDAL
    {
        public UserDTO CheckLogin(string email, string password)
        {
            string sql = @"SELECT id, email, full_name, phone, address, birthday, 
                                  gender, role, hire_date, salary, active
                           FROM users 
                           WHERE email = @Email AND password = @Password AND active = 1";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Email", email),
                new SqlParameter("@Password", password)
            };

            DataTable dt = DBUtil.ExecuteQuery(sql, parameters);

            if (dt.Rows.Count > 0)
            {
                return MapRowToUserDTO(dt.Rows[0]);
            }
            return null;
        }

        // Hàm bổ sung để fix lỗi Build lúc nãy
        public UserDTO GetUserById(int userId)
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

            if (dt.Rows.Count > 0)
            {
                return MapRowToUserDTO(dt.Rows[0]);
            }
            return null;
        }

        // Hàm dùng chung để chuyển dữ liệu từ Row sang Object cho gọn
        private UserDTO MapRowToUserDTO(DataRow row)
        {
            return new UserDTO
            {
                Id = (int)row["id"],
                Email = row["email"].ToString(),
                FullName = row["full_name"].ToString(),
                Phone = row["phone"]?.ToString(),
                Address = row["address"] != DBNull.Value ? row["address"].ToString() : null,
                Birthday = row["birthday"] != DBNull.Value ? Convert.ToDateTime(row["birthday"]) : (DateTime?)null,
                Gender = row["gender"] != DBNull.Value ? (bool)row["gender"] : (bool?)null,
                Role = (bool)row["role"],
                HireDate = row["hire_date"] != DBNull.Value ? Convert.ToDateTime(row["hire_date"]) : (DateTime?)null,
                Salary = row["salary"] != DBNull.Value ? Convert.ToInt32(row["salary"]) : (int?)null,
                Active = (bool)row["active"]
            };
        }
    }
}