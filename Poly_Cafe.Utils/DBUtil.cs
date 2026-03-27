using System;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;

namespace Poly_Cafe.Utils
{
    public static class DBUtil
    {
        // ✅ Connection String chuẩn (đã fix TrustServerCertificate)
        private static readonly string connectionString =
            "Data Source=DESKTOP-DFA7S8M\\SQLEXPRESS;Initial Catalog=Poly_CaPhe;Integrated Security=True;Encrypt=True;TrustServerCertificate=True";

        // 🔥 Hàm dùng chung để add parameters (TRÁNH LẶP CODE)
        private static void AddParameters(SqlCommand cmd, List<object> parameters)
        {
            if (parameters == null) return;

            for (int i = 0; i < parameters.Count; i++)
            {
                string paramName = $"@p{i}";
                object value = parameters[i] ?? DBNull.Value;

                if (value is string)
                {
                    SqlParameter p = new SqlParameter(paramName, SqlDbType.NVarChar)
                    {
                        Value = value
                    };
                    cmd.Parameters.Add(p);
                }
                else
                {
                    cmd.Parameters.AddWithValue(paramName, value);
                }
            }
        }

        // ✅ SELECT → DataTable
        public static DataTable QueryDataTable(string sql, List<object> parameters = null)
        {
            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    AddParameters(cmd, parameters);

                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }
                }
            }

            return dt;
        }

        // ✅ INSERT / UPDATE / DELETE
        public static int ExecuteNonQuery(string sql, List<object> parameters = null)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    AddParameters(cmd, parameters);
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        // ✅ SELECT 1 giá trị
        public static object ExecuteScalar(string sql, List<object> parameters = null)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    AddParameters(cmd, parameters);
                    return cmd.ExecuteScalar();
                }
            }
        }

        // ✅ TƯƠNG THÍCH CODE CŨ (UserDAL)
        public static DataTable ExecuteQuery(string sql, SqlParameter[] parameters = null)
        {
            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    if (parameters != null)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }

                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }
                }
            }

            return dt;
        }
    }
}