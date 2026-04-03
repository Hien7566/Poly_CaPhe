using System;
using System.Collections.Generic;
using System.Data;
using Poly_Cafe.DAL;
using Poly_Cafe.DTO;

namespace Poly_Cafe.BLL
{
    public class BillBLL
    {
        private BillDAL billDAL = new BillDAL();
        private BillDetailDAL billDetailDAL = new BillDetailDAL();

        // Giữ nguyên các hàm GetAllBills, GetBillsByDate...

        public BillDTO GetBill(int id)
        {
            DataTable dt = billDAL.GetBillById(id);
            if (dt == null || dt.Rows.Count == 0) return null;

            DataRow row = dt.Rows[0];
            BillDTO bill = new BillDTO
            {
                Id = Convert.ToInt32(row["id"]),
                UserId = Convert.ToInt32(row["user_id"]),
                UserName = row.Table.Columns.Contains("UserName") ? row["UserName"]?.ToString() : "N/A",
                CustomerId = row["customer_id"] != DBNull.Value ? Convert.ToInt32(row["customer_id"]) : (int?)null,
                CustomerName = row.Table.Columns.Contains("CustomerName") ? row["CustomerName"]?.ToString() : "Guest",
                Code = row["code"].ToString(),
                CreatedAt = Convert.ToDateTime(row["created_at"]),
                // Dùng Convert.ToDecimal trước khi ép sang Int để tránh lỗi nếu DB là kiểu Money
                Total = (int)Convert.ToDecimal(row["total"]),
                Discount = (int)Convert.ToDecimal(row["discount"]),
                FinalTotal = (int)Convert.ToDecimal(row["final_total"]),
                PaymentMethod = row["payment_method"]?.ToString(),
                Status = row["status"].ToString()
            };

            DataTable details = billDetailDAL.GetBillDetails(id);
            bill.BillDetails = new List<BillDetailDTO>();

            foreach (DataRow detailRow in details.Rows)
            {
                bill.BillDetails.Add(new BillDetailDTO
                {
                    Id = Convert.ToInt32(detailRow["id"]),
                    BillId = Convert.ToInt32(detailRow["bill_id"]),
                    DrinkId = Convert.ToInt32(detailRow["drink_id"]),
                    DrinkName = detailRow.Table.Columns.Contains("DrinkName") ? detailRow["DrinkName"].ToString() : "Unknown",
                    DrinkImage = detailRow.Table.Columns.Contains("DrinkImage") && detailRow["DrinkImage"] != DBNull.Value
                        ? "/images/" + detailRow["DrinkImage"].ToString()
                        : "/images/default-coffee.png",
                    Quantity = Convert.ToInt32(detailRow["quantity"]),
                    Price = (int)Convert.ToDecimal(detailRow["price"]),
                    Note = detailRow["note"]?.ToString()
                });
            }

            return bill;
        }

        public bool CreateBill(BillDTO bill, List<BillDetailDTO> details, int userId)
        {
            try
            {
                if (details == null || details.Count == 0) return false;

                bill.UserId = userId;
                bill.Code = billDAL.GenerateBillCode();
                bill.CreatedAt = DateTime.Now;
                bill.Status = "paid";

                // Lưu hóa đơn chính vào database
                int billId = billDAL.CreateBill(bill);

                if (billId > 0)
                {
                    // Cập nhật BillId cho từng chi tiết món ăn
                    foreach (var detail in details)
                    {
                        detail.BillId = billId;
                    }

                    // Lưu danh sách món vào bill_details
                    billDetailDAL.CreateBillDetails(billId, details);

                    // Cập nhật kho (nếu có logic trừ nguyên liệu)
                    billDetailDAL.UpdateStockAfterBill(details);

                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                // Bạn có thể log ex ở đây nếu cần
                return false;
            }
        }

        // Các hàm khác giữ nguyên...
        public DataTable GetAllBills() => billDAL.GetAllBills();
        public DataTable GetBillsByDate(DateTime from, DateTime to) => billDAL.GetBillsByDate(from, to);
    }
}