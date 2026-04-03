using Poly_Cafe.DTO;
using Poly_Cafe.Utils;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Poly_Cafe.DAL
{
    public class DashboardDAL
    {
        public DashboardDTO GetDashboardData()
        {
            var dashboard = new DashboardDTO();

            // Total Sales Today
            string sqlTotal = "SELECT ISNULL(SUM(final_total), 0) FROM bills WHERE status = 'paid' AND CAST(created_at AS DATE) = CAST(GETDATE() AS DATE)";
            dashboard.TotalSalesToday = (decimal)DBUtil.ExecuteScalar(sqlTotal, null);

            // Total Orders Today
            string sqlOrders = "SELECT COUNT(*) FROM bills WHERE status = 'paid' AND CAST(created_at AS DATE) = CAST(GETDATE() AS DATE)";
            dashboard.TotalOrders = (int)DBUtil.ExecuteScalar(sqlOrders, null);

            // Low Stock Items (count ingredients below min_stock)
            string sqlLowStock = "SELECT COUNT(*) FROM ingredients WHERE stock_quantity < min_stock";
            dashboard.LowStockItems = (int)DBUtil.ExecuteScalar(sqlLowStock, null);

            // Active Cashiers (users with role=0 who checked in today)
            string sqlCashiers = "SELECT COUNT(DISTINCT user_id) FROM shifts WHERE shift_date = CAST(GETDATE() AS DATE) AND check_in IS NOT NULL";
            dashboard.ActiveCashiers = (int)DBUtil.ExecuteScalar(sqlCashiers, null);

            // Sales Trend (last 7 days)
            dashboard.SalesTrend = GetSalesTrend();

            // Expenses Breakdown
            dashboard.Expenses = GetExpenses();

            // Top Selling Products
            dashboard.TopProducts = GetTopProducts();

            // Low Stock Alerts
            dashboard.LowStockAlerts = GetLowStockAlerts();

            return dashboard;
        }

        private List<SalesTrendData> GetSalesTrend()
        {
            var list = new List<SalesTrendData>();
            string sql = @"
                SELECT 
                    DATENAME(WEEKDAY, created_at) as DayName,
                    COUNT(*) as Orders,
                    ISNULL(SUM(final_total), 0) as Sales
                FROM bills 
                WHERE status = 'paid' AND created_at >= DATEADD(day, -6, CAST(GETDATE() AS DATE))
                GROUP BY DATENAME(WEEKDAY, created_at)
                ORDER BY MIN(created_at)";

            DataTable dt = DBUtil.ExecuteQuery(sql, null);
            foreach (DataRow row in dt.Rows)
            {
                list.Add(new SalesTrendData
                {
                    Day = row["DayName"].ToString(),
                    Orders = (int)row["Orders"],
                    Sales = (decimal)row["Sales"]
                });
            }
            return list;
        }

        private List<ExpenseData> GetExpenses()
        {
            var list = new List<ExpenseData>();
            // Demo data - thay bằng truy vấn thực tế nếu có bảng expenses
            list.Add(new ExpenseData { Name = "Utilities", Percentage = 10 });
            list.Add(new ExpenseData { Name = "Supplies", Percentage = 29 });
            list.Add(new ExpenseData { Name = "Maintenance", Percentage = 7 });
            list.Add(new ExpenseData { Name = "Salaries", Percentage = 54 });
            return list;
        }

        private List<TopProductData> GetTopProducts()
        {
            var list = new List<TopProductData>();
            string sql = @"
                SELECT TOP 5 
                    d.name as ProductName,
                    SUM(bd.quantity) as UnitsSold
                FROM bill_details bd
                JOIN drinks d ON bd.drink_id = d.id
                JOIN bills b ON bd.bill_id = b.id
                WHERE b.status = 'paid'
                GROUP BY d.name
                ORDER BY UnitsSold DESC";

            DataTable dt = DBUtil.ExecuteQuery(sql, null);
            foreach (DataRow row in dt.Rows)
            {
                list.Add(new TopProductData
                {
                    ProductName = row["ProductName"].ToString(),
                    UnitsSold = (int)row["UnitsSold"]
                });
            }
            return list;
        }

        private List<LowStockAlertData> GetLowStockAlerts()
        {
            var list = new List<LowStockAlertData>();
            string sql = @"
                SELECT name as ProductName, 
                       CAST(stock_quantity AS VARCHAR) + ' ' + unit as Stock
                FROM ingredients 
                WHERE stock_quantity < min_stock";

            DataTable dt = DBUtil.ExecuteQuery(sql, null);
            foreach (DataRow row in dt.Rows)
            {
                list.Add(new LowStockAlertData
                {
                    ProductName = row["ProductName"].ToString(),
                    Stock = row["Stock"].ToString()
                });
            }
            return list;
        }
    }
}