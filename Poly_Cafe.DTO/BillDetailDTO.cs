namespace Poly_Cafe.DTO
{
    public class BillDetailDTO
    {
        public int Id { get; set; }
        public int BillId { get; set; }
        public int DrinkId { get; set; }
        public string DrinkName { get; set; } // Dùng để hiển thị tên trên View
        public string DrinkImage { get; set; } // Dùng để load ảnh từ wwwroot/image
        public int Quantity { get; set; }
        public int Price { get; set; }
        public string Note { get; set; }
    }
}