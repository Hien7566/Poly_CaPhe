using System;
using System.Collections.Generic;
using System.Data;
using Poly_Cafe.Utils;

namespace Poly_Cafe.DAL
{
    public class ShiftDAL
    {
        public DataTable GetAllShifts()
        {
            string sql = @"SELECT s.*, u.full_name 
                   FROM shifts s 
                   LEFT JOIN users u ON s.user_id = u.id 
                   ORDER BY s.id ASC"; // Đổi DESC thành ASC ở đây
            return DBUtil.QueryDataTable(sql, null);
        }

        public int ExecuteNonQuery(string sql, List<object> parameters = null)
        {
            return DBUtil.ExecuteNonQuery(sql, parameters);
        }

        public DataTable ExecuteQuery(string sql, List<object> parameters = null)
        {
            return DBUtil.QueryDataTable(sql, parameters);
        }

        public object ExecuteScalar(string sql, List<object> parameters = null)
        {
            return DBUtil.ExecuteScalar(sql, parameters);
        }
    }
}