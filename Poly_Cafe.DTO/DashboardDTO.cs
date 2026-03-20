using System.Collections.Generic;

namespace Poly_Cafe.DTO
{
    public class DashboardDTO
    {
        public decimal TotalSalesToday { get; set; }
        public int TotalOrders { get; set; }
        public int LowStockItems { get; set; }
        public int ActiveCashiers { get; set; }
        public List<SalesTrendData> SalesTrend { get; set; }
        public List<ExpenseData> Expenses { get; set; }
        public List<TopProductData> TopProducts { get; set; }
        public List<LowStockAlertData> LowStockAlerts { get; set; }
    }

    public class SalesTrendData
    {
        public string Day { get; set; }
        public int Orders { get; set; }
        public decimal Sales { get; set; }
    }

    public class ExpenseData
    {
        public string Name { get; set; }
        public decimal Percentage { get; set; }
    }

    public class TopProductData
    {
        public string ProductName { get; set; }
        public int UnitsSold { get; set; }
    }

    public class LowStockAlertData
    {
        public string ProductName { get; set; }
        public string Stock { get; set; }
    }
}