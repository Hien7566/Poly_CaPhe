using System;
using System.Collections.Generic;
using System.Data;
using Poly_Cafe.DTO;
using Poly_Cafe.Utils;

namespace Poly_Cafe.DAL
{
    public class CustomerDAL
    {
        public List<CustomerDTO> GetAll()
        {
            List<CustomerDTO> list = new List<CustomerDTO>();
            string sql = "SELECT * FROM customers ORDER BY id DESC";

            DataTable dt = DBUtil.QueryDataTable(sql);

            foreach (DataRow row in dt.Rows)
            {
                list.Add(new CustomerDTO
                {
                    Id = Convert.ToInt32(row["id"]),
                    Full_Name = row["full_name"].ToString(),
                    Phone = row["phone"] != DBNull.Value ? row["phone"].ToString() : "",
                    Email = row["email"] != DBNull.Value ? row["email"].ToString() : "",
                    Address = row["address"] != DBNull.Value ? row["address"].ToString() : "",
                    Birthday = row["birthday"] != DBNull.Value ? Convert.ToDateTime(row["birthday"]) : (DateTime?)null,
                    Gender = row["gender"] != DBNull.Value ? row["gender"].ToString() : "",
                    Total_Spent = Convert.ToDecimal(row["total_spent"]),
                    Customer_Type = row["customer_type"] != DBNull.Value ? row["customer_type"].ToString() : "Thường",
                    Created_At = row["created_at"] != DBNull.Value ? Convert.ToDateTime(row["created_at"]) : DateTime.Now,
                    Active = Convert.ToBoolean(row["active"])
                });
            }
            return list;
        }

        public CustomerDTO GetById(int id)
        {
            string sql = "SELECT * FROM customers WHERE id = @p0";
            DataTable dt = DBUtil.QueryDataTable(sql, new List<object> { id });

            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                return new CustomerDTO
                {
                    Id = Convert.ToInt32(row["id"]),
                    Full_Name = row["full_name"].ToString(),
                    Phone = row["phone"] != DBNull.Value ? row["phone"].ToString() : "",
                    Email = row["email"] != DBNull.Value ? row["email"].ToString() : "",
                    Address = row["address"] != DBNull.Value ? row["address"].ToString() : "",
                    Birthday = row["birthday"] != DBNull.Value ? Convert.ToDateTime(row["birthday"]) : (DateTime?)null,
                    Gender = row["gender"] != DBNull.Value ? row["gender"].ToString() : "",
                    Total_Spent = Convert.ToDecimal(row["total_spent"]),
                    Customer_Type = row["customer_type"] != DBNull.Value ? row["customer_type"].ToString() : "Thường",
                    Created_At = row["created_at"] != DBNull.Value ? Convert.ToDateTime(row["created_at"]) : DateTime.Now,
                    Active = Convert.ToBoolean(row["active"])
                };
            }
            return null;
        }

        public bool Insert(CustomerDTO customer)
        {
            string sql = @"INSERT INTO customers (full_name, phone, email, address, birthday, gender, total_spent, customer_type, created_at, active) 
                           VALUES (@p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, GETDATE(), @p8)";

            return DBUtil.ExecuteNonQuery(sql, new List<object> {
                customer.Full_Name,
                customer.Phone ?? "",
                customer.Email ?? "",
                customer.Address ?? "",
                customer.Birthday.HasValue ? (object)customer.Birthday.Value : DBNull.Value, // Xử lý nếu NULL
                customer.Gender ?? "",
                customer.Total_Spent,
                customer.Customer_Type ?? "Thường",
                customer.Active
            }) > 0;
        }

        public bool Update(CustomerDTO customer)
        {
            string sql = @"UPDATE customers 
                           SET full_name = @p0, phone = @p1, email = @p2, address = @p3, 
                               birthday = @p4, gender = @p5, total_spent = @p6, customer_type = @p7, active = @p8 
                           WHERE id = @p9";

            return DBUtil.ExecuteNonQuery(sql, new List<object> {
                customer.Full_Name,
                customer.Phone ?? "",
                customer.Email ?? "",
                customer.Address ?? "",
                customer.Birthday.HasValue ? (object)customer.Birthday.Value : DBNull.Value,
                customer.Gender ?? "",
                customer.Total_Spent,
                customer.Customer_Type ?? "Thường",
                customer.Active,
                customer.Id
            }) > 0;
        }

        public bool Delete(int id)
        {
            string sql = "DELETE FROM customers WHERE id = @p0";
            return DBUtil.ExecuteNonQuery(sql, new List<object> { id }) > 0;
        }
    }
}