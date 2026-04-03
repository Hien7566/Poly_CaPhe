using Poly_Cafe.DTO;
using Poly_Cafe.Utils;
using System;
using System.Collections.Generic;
using System.Data;

namespace Poly_Cafe.DAL
{
    public class BillDAL
    {
        public DataTable GetAllBills()
        {
            string query = @"
                SELECT b.*, u.full_name as UserName, c.full_name as CustomerName
                FROM bills b
                LEFT JOIN users u ON b.user_id = u.id
                LEFT JOIN customers c ON b.customer_id = c.id
                ORDER BY b.created_at DESC";

            return DBUtil.ExecuteQuery(query);
        }

        public DataTable GetBillById(int id)
        {
            string query = @"
                SELECT b.*, u.full_name as UserName, c.full_name as CustomerName
                FROM bills b
                LEFT JOIN users u ON b.user_id = u.id
                LEFT JOIN customers c ON b.customer_id = c.id
                WHERE b.id = @id";

            var parameters = new List<object> { id };
            return DBUtil.QueryDataTable(query, parameters);
        }

        public DataTable GetBillsByDate(DateTime fromDate, DateTime toDate)
        {
            string query = @"
                SELECT b.*, u.full_name as UserName, c.full_name as CustomerName
                FROM bills b
                LEFT JOIN users u ON b.user_id = u.id
                LEFT JOIN customers c ON b.customer_id = c.id
                WHERE CAST(b.created_at AS DATE) BETWEEN @fromDate AND @toDate
                ORDER BY b.created_at DESC";

            var parameters = new List<object> { fromDate.Date, toDate.Date };
            return DBUtil.QueryDataTable(query, parameters);
        }

        public int CreateBill(BillDTO bill)
        {
            string query = @"
                INSERT INTO bills (user_id, customer_id, code, total, discount, final_total, payment_method, status, created_at)
                VALUES (@userId, @customerId, @code, @total, @discount, @finalTotal, @paymentMethod, @status, GETDATE());
                SELECT SCOPE_IDENTITY();";

            var parameters = new List<object> { bill.UserId, bill.CustomerId ?? (object)DBNull.Value, bill.Code, bill.Total, bill.Discount, bill.FinalTotal, bill.PaymentMethod, "paid" };
            return Convert.ToInt32(DBUtil.ExecuteScalar(query, parameters));
        }

        public bool UpdateBillStatus(int billId, string status)
        {
            string query = "UPDATE bills SET status = @status WHERE id = @id";

            var parameters = new List<object> { status, billId };
            return DBUtil.ExecuteNonQuery(query, parameters) > 0;
        }

        public string GenerateBillCode()
        {
            string query = "SELECT COUNT(*) FROM bills WHERE CAST(created_at AS DATE) = CAST(GETDATE() AS DATE)";
            int count = Convert.ToInt32(DBUtil.ExecuteScalar(query, null));
            return $"HD{DateTime.Now:yyyyMMdd}-{count + 1:D3}";
        }

        public DataTable GetTodayRevenue()
        {
            string query = @"
                SELECT 
                    COUNT(*) as TotalOrders,
                    ISNULL(SUM(final_total), 0) as TotalRevenue,
                    ISNULL(AVG(final_total), 0) as AvgOrderValue
                FROM bills 
                WHERE status = 'paid' 
                AND CAST(created_at AS DATE) = CAST(GETDATE() AS DATE)";

            return DBUtil.ExecuteQuery(query);
        }

        public DataTable GetRevenueByDate(DateTime date)
        {
            string query = @"
                SELECT 
                    b.code,
                    b.created_at,
                    c.full_name as CustomerName,
                    u.full_name as StaffName,
                    b.final_total,
                    b.payment_method
                FROM bills b
                LEFT JOIN customers c ON b.customer_id = c.id
                LEFT JOIN users u ON b.user_id = u.id
                WHERE b.status = 'paid' 
                AND CAST(b.created_at AS DATE) = @date
                ORDER BY b.created_at DESC";

            var parameters = new List<object> { date.Date };
            return DBUtil.QueryDataTable(query, parameters);
        }
    }
}