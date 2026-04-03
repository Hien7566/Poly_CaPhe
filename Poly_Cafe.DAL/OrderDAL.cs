using System;
using System.Collections.Generic;
using System.Data;
using Poly_Cafe.DTO;
using Poly_Cafe.Utils;

namespace Poly_Cafe.DAL
{
    public class OrderDAL
    {
        public List<BillDTO> GetListOrders()
        {
            List<BillDTO> list = new List<BillDTO>();
            string sql = @"
                SELECT b.*, 
                       u.full_name AS UserName, 
                       c.full_name AS CustomerName 
                FROM dbo.bills b
                LEFT JOIN dbo.users u ON b.user_id = u.id
                LEFT JOIN dbo.customers c ON b.customer_id = c.id
                ORDER BY b.created_at DESC";

            try
            {
                DataTable dt = DBUtil.QueryDataTable(sql, null);
                if (dt != null)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        list.Add(new BillDTO
                        {
                            Id = Convert.ToInt32(dr["id"]),
                            Code = dr["code"]?.ToString(),
                            UserId = dr["user_id"] != DBNull.Value ? Convert.ToInt32(dr["user_id"]) : (int?)null,
                            UserName = dr["UserName"]?.ToString() ?? "Hệ thống",
                            CustomerId = dr["customer_id"] != DBNull.Value ? Convert.ToInt32(dr["customer_id"]) : (int?)null,
                            CustomerName = dr["CustomerName"]?.ToString() ?? "Khách vãng lai",
                            CreatedAt = dr["created_at"] != DBNull.Value ? Convert.ToDateTime(dr["created_at"]) : (DateTime?)null,
                            Total = Convert.ToInt32(dr["total"]),
                            Discount = Convert.ToInt32(dr["discount"]),
                            FinalTotal = Convert.ToInt32(dr["final_total"]),
                            PaymentMethod = dr["payment_method"]?.ToString(),
                            Status = dr["status"]?.ToString()
                        });
                    }
                }
            }
            catch (Exception ex) { System.Diagnostics.Debug.WriteLine("Lỗi GetListOrders: " + ex.Message); }
            return list;
        }

        public List<OrderDetailDTO> GetDetailsByBillId(int billId)
        {
            List<OrderDetailDTO> details = new List<OrderDetailDTO>();
            string sql = @"SELECT d.*, m.name as drink_name 
                           FROM dbo.bill_details d 
                           JOIN dbo.drinks m ON d.drink_id = m.id 
                           WHERE d.bill_id = @p0";
            try
            {
                DataTable dt = DBUtil.QueryDataTable(sql, new List<object> { billId });
                if (dt != null)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        details.Add(new OrderDetailDTO
                        {
                            Id = Convert.ToInt32(row["id"]),
                            BillId = Convert.ToInt32(row["bill_id"]),
                            DrinkId = Convert.ToInt32(row["drink_id"]),
                            DrinkName = row["drink_name"].ToString(),
                            Quantity = Convert.ToInt32(row["quantity"]),
                            Price = Convert.ToInt32(row["price"]),
                            Note = row["note"]?.ToString()
                        });
                    }
                }
            }
            catch (Exception ex) { System.Diagnostics.Debug.WriteLine("Lỗi GetDetailsByBillId: " + ex.Message); }
            return details;
        }

        // --- CHỖ NÀY SỬA LẠI: Nhận userId từ bên ngoài truyền vào ---
        public string InsertBill(int userId, string code, int total, string paymentMethod, List<OrderDetailDTO> details)
        {
            // Thay số 1 bằng @p0
            string sqlBill = @"INSERT INTO dbo.bills (user_id, customer_id, code, total, discount, final_total, payment_method, status, created_at) 
                               VALUES (@p0, NULL, @p1, @p2, 0, @p3, @p4, '0', GETDATE());
                               SELECT SCOPE_IDENTITY();";

            try
            {
                // Thêm userId vào đầu danh sách tham số
                List<object> pBill = new List<object> { userId, code, total, total, paymentMethod };
                object result = DBUtil.ExecuteScalar(sqlBill, pBill);
                if (result == null) return null;

                int billId = Convert.ToInt32(result);

                foreach (var item in details)
                {
                    string sqlDetail = @"INSERT INTO dbo.bill_details (bill_id, drink_id, quantity, price, note) 
                                         VALUES (@p0, @p1, @p2, @p3, @p4)";
                    List<object> pDetail = new List<object> { billId, item.DrinkId, item.Quantity, item.Price, item.Note ?? "" };
                    DBUtil.ExecuteNonQuery(sqlDetail, pDetail);
                }
                return code;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Lỗi InsertBill: " + ex.Message);
                return null;
            }
        }
         
        public bool UpdateStatus(int id, string newStatus)
        {
            try
            {
                string statusValue = (newStatus.ToLower() == "completed" || newStatus == "1") ? "1" : newStatus;
                string sql = "UPDATE dbo.bills SET status = @p0 WHERE id = @p1";
                List<object> parameters = new List<object> { statusValue, id };
                return DBUtil.ExecuteNonQuery(sql, parameters) > 0;
            }
            catch (Exception ex) { System.Diagnostics.Debug.WriteLine("Lỗi UpdateStatus: " + ex.Message); return false; }
        }
    }
}