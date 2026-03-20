using System;
using System.Collections.Generic;
using System.Text;

namespace Poly_Cafe.DTO
{
    public class BillDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Code { get; set; }
        public DateTime CreatedAt { get; set; }
        public int Total { get; set; }
        public string Status { get; set; }
    }
}
