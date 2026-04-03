using System;
namespace Poly_Cafe.DTO
{
    public class ShiftDTO
    {
        public int SessionId { get; set; }
        public string CashierName { get; set; }
        public int UserId { get; set; }
        public string ShiftName { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public double TotalSales { get; set; }
    }
}