using System.Collections.Generic;
using Poly_Cafe.DAL;
using Poly_Cafe.DTO;

namespace Poly_Cafe.BLL
{
    public class CustomerBLL
    {
        private readonly CustomerDAL _customerDAL = new CustomerDAL();

        public List<CustomerDTO> GetAll() => _customerDAL.GetAll();
        public CustomerDTO GetById(int id) => _customerDAL.GetById(id);
        public bool Insert(CustomerDTO customer) => _customerDAL.Insert(customer);
        public bool Update(CustomerDTO customer) => _customerDAL.Update(customer);
        public bool Delete(int id) => _customerDAL.Delete(id);
    }
}