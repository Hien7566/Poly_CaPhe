using System;

namespace Poly_Cafe.DTO
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public DateTime? Birthday { get; set; }
        public bool? Gender { get; set; }
        public bool Role { get; set; }
        public DateTime? HireDate { get; set; }
        public int? Salary { get; set; }
        public bool Active { get; set; }
    }
}