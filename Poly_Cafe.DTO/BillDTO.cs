using System;
using System.Collections.Generic;

namespace Poly_Cafe.DTO
{
    public class BillDTO
    {
        public int Id { get; set; }
        public int? UserId { get; set; }     // Thêm dấu ? vì SQL cho phép null
        public string UserName { get; set; } // Dùng để Join lấy tên từ bảng Users
        public int? CustomerId { get; set; } // Thêm dấu ?
        public string CustomerName { get; set; }
        public string Code { get; set; }
        public DateTime? CreatedAt { get; set; } // Thêm dấu ?
        public int Total { get; set; }
        public int Discount { get; set; }
        public int FinalTotal { get; set; }
        public string PaymentMethod { get; set; }
        public string Status { get; set; }

        // Danh sách chi tiết món (Dùng cho trang Xem Chi Tiết)
        public List<BillDetailDTO> BillDetails { get; set; } = new List<BillDetailDTO>();
    }

    public class CartItemDTO
    {
        public int DrinkId { get; set; }
        public string DrinkName { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }
        public string Note { get; set; }
        public int Subtotal => Quantity * Price; // Thuộc tính tự tính toán, rất tốt!
    }
}