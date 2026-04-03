using Poly_Cafe.DAL;
using Poly_Cafe.DTO;

namespace Poly_Cafe.BLL
{
    public class DashboardBLL
    {
        private DashboardDAL dashboardDAL = new DashboardDAL();

        public DashboardDTO GetDashboardData()
        {
            return dashboardDAL.GetDashboardData();
        }
    }
}