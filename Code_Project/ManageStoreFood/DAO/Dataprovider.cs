using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace ManageStoreFood.DAO
{
    // tạo ra 1 lớp dataprovider 
    public class Dataprovider
    {
        //Singleton đảm bảo chỉ có một instance duy nhất, để tránh lãng phí tài nguyên và môi trường
        private static Dataprovider instance;
        public static Dataprovider Instance
        {
            get { 
                if(instance == null) 
                    instance = new Dataprovider();
                    return Dataprovider.instance;
            }
            private set { Dataprovider.instance = value; }
        }

        private Dataprovider() { }

        private string connectionSTR = "Data Source=HADESSITRINH\\HADESSITRINH;Initial Catalog=QuanLyNhaH;Integrated Security=True";
        // trả ra bảng dữ liệu
        public DataTable ExecuteQuery(string query, object[] parameter = null)
        {
            //giải phóng bộ nhớ
            DataTable dataTable = new DataTable();
            using (SqlConnection connection = new SqlConnection(connectionSTR))
            {
                connection.Open();

                //excute query 
                SqlCommand cmd = new SqlCommand(query, connection);
                //nếu danh sách có nhiều parameter thì tạo ra mảng object chứa parameter để dễ gọi hơn
                if (parameter != null)
                {
                    string[] listPaRa = query.Split(' ');
                    int i = 0;
                    foreach (string item in listPaRa)
                    {
                        if (item.Contains('@'))
                        {
                            string para = item.Replace(",","");//Bổ sung để mỗi lần parameter ko phải nhập dấu cách
                            cmd.Parameters.AddWithValue(para, parameter[i]);
                            i++;
                        }
                    }
                }

                //trung gian chạy câu truy vấn lấy dữ liệu lên 
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                //trả dự liệu cho datatable
                adapter.Fill(dataTable);

                connection.Close();
            }
            return dataTable;
        }

        // trả ra số dòng thành công 
        public int ExecuteNonQuery(string query, object[] parameter = null)
        {
            int data = 0;

            using (SqlConnection connection = new SqlConnection(connectionSTR))
            {
                connection.Open();

                //excute query 
                SqlCommand cmd = new SqlCommand(query, connection);
                //nếu danh sách có nhiều parameter thì tạo ra mảng object chứa parameter để dễ gọi hơn
                if (parameter != null)
                {
                    string[] listPaRa = query.Split(' ');
                    int i = 0;
                    foreach (string item in listPaRa)
                    {
                        if (item.Contains('@'))
                        {
                            cmd.Parameters.AddWithValue(item, parameter[i]);
                            i++;
                        }
                    }
                }

                data = cmd.ExecuteNonQuery();

                connection.Close();
            }
            return data;
        }

        // trả ra số lượng thành công 
        public object ExecuteScalar(string query, object[] parameter = null)
        {
            object data  = 0;

            using (SqlConnection connection = new SqlConnection(connectionSTR))
            {
                connection.Open();

                //excute query 
                SqlCommand cmd = new SqlCommand(query, connection);
                //nếu danh sách có nhiều parameter thì tạo ra mảng object chứa parameter để dễ gọi hơn
                if (parameter != null)
                {
                    string[] listPaRa = query.Split(' ');
                    int i = 0;
                    foreach (string item in listPaRa)
                    {
                        if (item.Contains('@'))
                        {
                            cmd.Parameters.AddWithValue(item, parameter[i]);
                            i++;
                        }
                    }
                }

               data = cmd.ExecuteScalar();

                connection.Close();
            }
            return data;
        }
    }
}
