using System.Collections.Generic;
using Poly_Cafe.DAL;
using Poly_Cafe.DTO;

namespace Poly_Cafe.BLL
{
    public class OrderBLL
    {
        private OrderDAL dal = new OrderDAL();

        // 1. Lấy danh sách hiển thị
        public List<BillDTO> GetAll() => dal.GetListOrders();

        // 2. Duyệt đơn (Chuyển status sang 1)
        public bool ApproveOrder(int id) => dal.UpdateStatus(id, "1");

        // 3. Hủy đơn (Chuyển status sang -1 để khớp với Tab Hủy)
        // Sửa lại thành "-1" để DAL lưu đúng vào Tab Đã hủy
        public bool CancelOrder(int id) => dal.UpdateStatus(id, "-1");

        // 4. Tạo đơn hàng mới từ trang POS
        // THÊM THAM SỐ int userId VÀO ĐÂY
        public string CreateOrder(int userId, string code, int total, string paymentMethod, List<OrderDetailDTO> details)
        {
            if (details == null || details.Count == 0) return null;

            // TRUYỀN userId XUỐNG DAL
            return dal.InsertBill(userId, code, total, paymentMethod, details);
        }
    }
}