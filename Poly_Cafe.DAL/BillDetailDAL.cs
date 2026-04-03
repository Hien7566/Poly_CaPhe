using System.Collections.Generic;
using Poly_Cafe.DTO;
using Poly_Cafe.Utils;

namespace Poly_Cafe.DAL
{
    public class BillDetailDAL
    {
        public bool InsertBillDetail(BillDetailDTO detail)
        {
            string sql = "INSERT INTO bill_details (bill_id, drink_id, quantity, price, note) VALUES (@p0, @p1, @p2, @p3, @p4)";
            List<object> parameters = new List<object> { detail.BillId, detail.DrinkId, detail.Quantity, detail.Price, detail.Note ?? string.Empty };
            return DBUtil.ExecuteNonQuery(sql, parameters) > 0;
        }

        public System.Data.DataTable GetBillDetails(int billId)
        {
            string sql = @"SELECT bd.*, d.name as DrinkName, d.image as DrinkImage FROM bill_details bd LEFT JOIN drinks d ON bd.drink_id = d.id WHERE bd.bill_id = @p0";
            return DBUtil.QueryDataTable(sql, new System.Collections.Generic.List<object> { billId });
        }

        // Create many details at once
        public void CreateBillDetails(int billId, System.Collections.Generic.List<BillDetailDTO> details)
        {
            foreach(var d in details)
            {
                d.BillId = billId;
                InsertBillDetail(d);
            }
        }

        // optional: update stock after bill (not implemented yet)
        public void UpdateStockAfterBill(System.Collections.Generic.List<BillDetailDTO> details)
        {
            // implement stock deduction here if needed
        }
    }
}