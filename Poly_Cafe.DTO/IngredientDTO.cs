namespace Poly_Cafe.DTO
{
    public class IngredientDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        // Số lượng tồn kho hiện tại 
        public decimal Stock_Quantity { get; set; }

        // Đơn vị tính: Kg, Lon, Lít... 
        public string Unit { get; set; }

        // Mức tồn kho tối thiểu để cảnh báo sắp hết 
        public decimal Min_Stock { get; set; }

        // Giá nhập trên 1 đơn vị 
        public decimal Price_Per_Unit { get; set; }

        // Nhà cung cấp 
        public string Supplier { get; set; }

        // Trạng thái đang sử dụng/Ngừng sử dụng 
        public bool Active { get; set; }
    }
}