using System;
using System.Data;
using System.Data.SqlClient;

namespace Poly_Cafe.Utils
{
    public static class DBUtil
    {
        // SỬA CHUỖI KẾT NỐI THEO MÁY BẠN
        // Sửa: Bỏ dấu cách trong "TrustServerCertificate"
        private static readonly string connectionString =
            "Data Source=HIENTB01233\\SQLEXPRESS01;Initial Catalog=Poly_CaPhe;Integrated Security=True;TrustServerCertificate=True;";
        // Phương thức QueryDataTable (giữ nguyên)
        public static DataTable QueryDataTable(string sql, List<object> parameters)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    if (parameters != null)
                    {
                        for (int i = 0; i < parameters.Count; i++)
                        {
                            string paramName = $"@p{i}";
                            object value = parameters[i] ?? DBNull.Value;

                            if (value is string)
                            {
                                SqlParameter p = new SqlParameter(paramName, SqlDbType.NVarChar);
                                p.Value = value;
                                cmd.Parameters.Add(p);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue(paramName, value);
                            }
                        }
                    }
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(dt);
                }
            }
            return dt;
        }

        // Phương thức ExecuteNonQuery (giữ nguyên)
        public static int ExecuteNonQuery(string sql, List<object> parameters)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    if (parameters != null)
                    {
                        for (int i = 0; i < parameters.Count; i++)
                        {
                            string paramName = $"@p{i}";
                            object value = parameters[i] ?? DBNull.Value;

                            if (value is string)
                            {
                                SqlParameter p = new SqlParameter(paramName, SqlDbType.NVarChar);
                                p.Value = value;
                                cmd.Parameters.Add(p);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue(paramName, value);
                            }
                        }
                    }
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        // Phương thức ExecuteScalar (giữ nguyên)
        public static object ExecuteScalar(string sql, List<object> parameters)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    if (parameters != null)
                    {
                        for (int i = 0; i < parameters.Count; i++)
                        {
                            string paramName = $"@p{i}";
                            object value = parameters[i] ?? DBNull.Value;

                            if (value is string)
                            {
                                SqlParameter p = new SqlParameter(paramName, SqlDbType.NVarChar);
                                p.Value = value;
                                cmd.Parameters.Add(p);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue(paramName, value);
                            }
                        }
                    }
                    return cmd.ExecuteScalar();
                }
            }
        }

        // THÊM PHƯƠNG THỨC ExecuteQuery (để tương thích với UserDAL)
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
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(dt);
                }
            }
            return dt;
        }
    }
}