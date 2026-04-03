using System;
using System.Collections.Generic;
using System.Data;
using Poly_Cafe.DAL;
using Poly_Cafe.DTO;

namespace Poly_Cafe.BLL
{
    public class ShiftBLL
    {
        private ShiftDAL _dal = new ShiftDAL();

        // 1. Lấy danh sách ca (Hiển thị bảng và tính toán 3 ô thống kê)
        public List<ShiftDTO> GetListSessions()
        {
            DataTable dt = _dal.GetAllShifts();
            List<ShiftDTO> list = new List<ShiftDTO>();
            if (dt == null) return list;

            foreach (DataRow row in dt.Rows)
            {
                list.Add(new ShiftDTO
                {
                    SessionId = Convert.ToInt32(row["id"]),
                    ShiftName = row["shift_name"] != DBNull.Value ? row["shift_name"].ToString() : "Ca chưa đặt tên",
                    UserId = row["user_id"] != DBNull.Value ? Convert.ToInt32(row["user_id"]) : 0,
                    CashierName = row["full_name"] != DBNull.Value ? row["full_name"].ToString() : "Đang trống",
                    StartTime = row["check_in"] != DBNull.Value ? Convert.ToDateTime(row["check_in"]) : (DateTime?)null,
                    EndTime = row["check_out"] != DBNull.Value ? Convert.ToDateTime(row["check_out"]) : (DateTime?)null,
                    TotalSales = row["total_sales"] != DBNull.Value ? Convert.ToDouble(row["total_sales"]) : 0
                });
            }
            return list;
        }

        // --- CHỨC NĂNG QUẢN TRỊ CHO ADMIN (THÊM - XÓA - SỬA) ---

        // Tạo ca mới trống hoàn toàn
        public bool CreateBlankShift(string name)
        {
            string sql = "INSERT INTO shifts (shift_name, total_sales) VALUES (@p0, 0)";
            return _dal.ExecuteNonQuery(sql, new List<object> { name }) > 0;
        }

        // Sửa tên ca làm việc
        public bool UpdateShiftName(int id, string name)
        {
            string sql = "UPDATE shifts SET shift_name = @p0 WHERE id = @p1";
            return _dal.ExecuteNonQuery(sql, new List<object> { name, id }) > 0;
        }

        // Xóa ca làm việc
        public bool DeleteShift(int id)
        {
            string sql = "DELETE FROM shifts WHERE id = @p0";
            return _dal.ExecuteNonQuery(sql, new List<object> { id }) > 0;
        }

        // --- CHỨC NĂNG VẬN HÀNH CHO NHÂN VIÊN ---

        // Bấm "Nhận ca" -> Cập nhật User và giờ vào
        public bool StartNewShift(int sessionId, int userId)
        {
            string sql = @"UPDATE shifts 
                           SET user_id = @p0, 
                               check_in = GETDATE() 
                           WHERE id = @p1 AND check_in IS NULL";

            return _dal.ExecuteNonQuery(sql, new List<object> { userId, sessionId }) > 0;
        }

        // TỰ TÍNH TIỀN: Lấy tổng final_total từ bảng bills kể từ lúc check_in
        public double CalculateShiftRevenue(int sessionId)
        {
            string sqlInfo = "SELECT user_id, check_in FROM shifts WHERE id = @p0";
            DataTable dt = _dal.ExecuteQuery(sqlInfo, new List<object> { sessionId });

            if (dt == null || dt.Rows.Count == 0 || dt.Rows[0]["check_in"] == DBNull.Value) return 0;

            var userId = dt.Rows[0]["user_id"];
            var checkIn = dt.Rows[0]["check_in"];

            // Truy vấn đúng tên cột: final_total và created_at
            string sqlSum = @"SELECT ISNULL(SUM(final_total), 0) FROM bills 
                              WHERE user_id = @p0 
                              AND created_at >= @p1";

            object result = _dal.ExecuteScalar(sqlSum, new List<object> { userId, checkIn });
            return Convert.ToDouble(result);
        }

        // Bấm "Chốt ca" -> Tự tính tiền và cập nhật giờ ra
        public bool EndShift(int sessionId)
        {
            double totalSales = CalculateShiftRevenue(sessionId);

            string sql = "UPDATE shifts SET check_out = GETDATE(), total_sales = @p0 WHERE id = @p1";
            return _dal.ExecuteNonQuery(sql, new List<object> { totalSales, sessionId }) > 0;
        }
    }
}