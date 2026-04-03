using System;
using System.Collections.Generic;
using Poly_Cafe.DAL;
using Poly_Cafe.DTO;

namespace Poly_Cafe.BLL
{
    public class UserBLL
    {
        private UserDAL dal = new UserDAL();

        public List<UserDTO> GetListUser() => dal.GetAll();

        public bool IsEmailExists(string email) => dal.IsEmailExists(email);

        public bool CreateUser(UserDTO u)
        {
            u.Active = true;
            return dal.Add(u);
        }

        public UserDTO GetUserById(int id) => dal.GetById(id);

        public bool UpdateUser(UserDTO u) => dal.Update(u);

        public bool DeleteUser(int id) => dal.Delete(id);

        // --- HÀM ĐĂNG NHẬP (FIX LỖI CỦA ÔNG) ---
        public UserDTO CheckLogin(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                return null;

            return dal.CheckLogin(email.Trim(), password);
        }
    }
}