namespace Poly_Cafe.DTO
{
    public class DrinkDTO
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } // Thêm cái này để hiện tên loại
        public string Name { get; set; }
        public decimal Price { get; set; } // Đảm bảo là chữ P viết hoa
        public string Image { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
    }
}