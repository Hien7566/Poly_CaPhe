using System;
using System.Collections.Generic;
using System.Text;

namespace Poly_Cafe.DTO
{
    public class BillDetailDTO
    {
        public int Id { get; set; }
        public int BillId { get; set; }
        public int DrinkId { get; set; }
        public string DrinkName { get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; }
        public int SubTotal => Quantity * Price;
    }
}
