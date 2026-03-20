using System;
using System.Collections.Generic;
using System.Text;

namespace Poly_Cafe.DTO
{
    public class DrinkDTO
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
    }
}
