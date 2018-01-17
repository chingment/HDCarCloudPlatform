using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                SqlConnection conn = new SqlConnection("server=WIN-9KIJVJBE952\\MSSQLSERVER2;uid=sa;pwd=12345Jsm;database=AppCar;");
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "select * from sys_user";
                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    Console.WriteLine("读到数据库数据");
                }

                dr.Close();
                conn.Close();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.ReadLine();


        }
    }
}
