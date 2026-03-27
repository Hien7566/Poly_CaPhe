using System;

namespace Poly_Cafe.DTO
{
    public class CustomerDTO
    {
        public int Id { get; set; }
        public string Full_Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }

        // Dấu "?" nghĩa là cho phép NULL (Khách không cung cấp ngày sinh)
        public DateTime? Birthday { get; set; }
        public string Gender { get; set; }

        // Tổng tiền đã chi tiêu để xét hạng
        public decimal Total_Spent { get; set; }

        // Hạng thành viên (Thường, Thân thiết, VIP, VVIP)
        public string Customer_Type { get; set; }

        public DateTime Created_At { get; set; }
        public bool Active { get; set; }
    }
}