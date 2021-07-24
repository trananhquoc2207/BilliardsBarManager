using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanBida.DAO
{
    public class DataProvider
    {
        private static DataProvider instance;
        public static DataProvider Instance {
            get {  if (instance == null) instance = new DataProvider(); return DataProvider.instance; } 
            private set { DataProvider.instance = value; }
        }
        private DataProvider() { 
        }
        string str = @"Data Source=DESKTOP-TAG6L7K\SQLEXPRESS;Initial Catalog=QuanLyQuanBida;Integrated Security=True";
        public DataTable ExecuteOuery(string query,object[] parameters =null)
        {
            DataTable data = new DataTable();
            using (SqlConnection conn = new SqlConnection(str))
            {
                
                conn.Open();
                SqlCommand command = new SqlCommand(query, conn);
                if (parameters!=null)
                {
                    string[] listPare = query.Split(' ');
                    int i = 0;
                    foreach(string item in listPare)
                    {
                        if (item.Contains("@"))
                        {
                            command.Parameters.AddWithValue(item,parameters[i]);
                            i++;
                        }
                    }

                }

                SqlDataAdapter adapter = new SqlDataAdapter(command);

                adapter.Fill(data);

                conn.Close();
            }
            

            return data; 
        }

        public int ExecuteNonOuery(string query, object[] parameters = null)
        {
            int data = 0;
            using (SqlConnection conn = new SqlConnection(str))
            {
                conn.Open();
                SqlCommand command = new SqlCommand(query, conn);
                if (parameters != null)
                {
                    string[] listPare = query.Split(' ');
                    int i = 0;
                    foreach (string item in listPare)
                    {
                        if (item.Contains("@"))
                        {
                            command.Parameters.AddWithValue(item, parameters[i]);
                            i++;
                        }
                    }
                }
                data = command.ExecuteNonQuery();

                conn.Close();
            }
            return data;
        }

        public object ExecuteScalar(string query, object[] parameters = null)
        {
            object data = 0;
            using (SqlConnection conn = new SqlConnection(str))
            {
                conn.Open();
                SqlCommand command = new SqlCommand(query, conn);
                if (parameters != null)
                {
                    string[] listPare = query.Split(' ');
                    int i = 0;
                    foreach (string item in listPare)
                    {
                        if (item.Contains("@"))
                        {
                            command.Parameters.AddWithValue(item, parameters[i]);
                            i++;
                        }
                    }
                }
                data = command.ExecuteScalar();

                conn.Close();
            }
            return data;
        }

    }
}
