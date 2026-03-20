using Poly_Cafe.DAL;
using Poly_Cafe.DTO;

namespace Poly_Cafe.BLL
{
    public class UserBLL
    {
        private UserDAL userDAL = new UserDAL();

        public UserDTO CheckLogin(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                return null;
            }
            return userDAL.CheckLogin(email, password);
        }

        public UserDTO GetUserById(int userId)
        {
            return userDAL.GetUserById(userId);
        }
    }
}