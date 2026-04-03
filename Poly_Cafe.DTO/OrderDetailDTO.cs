using System;
using System.Collections.Generic;
using System.Text;

namespace Poly_Cafe.DTO
{
    public class OrderDetailDTO
    {
        public int Id { get; set; }
        public int BillId { get; set; }
        public int DrinkId { get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; }
        public string Note { get; set; }

        // Dùng để hiển thị tên món trên hóa đơn nếu cần
        public string DrinkName { get; set; }
    }
}
