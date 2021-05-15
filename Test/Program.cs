using Data.BLL;
using MSSQL_Lite.Access;
using MSSQL_Lite.Connection;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.OutputEncoding = Encoding.UTF8;

            SqlConnectInfo.DataSource = @"LAPTOP-B78E1G5S\MSSQLSERVER2019";
            SqlConnectInfo.InitialCatalog = "Movie";
            SqlConnectInfo.UserID = "sa";
            SqlConnectInfo.Password = "123456789";
            SqlData.objectReceivingData = ObjectReceivingData.DataSet;

            Run().GetAwaiter().GetResult();

            Console.ReadKey();
            Console.ReadKey();
            Console.ReadKey();
        }

        static async Task Run()
        {

            CategoryBLLForAdmin categoryBLL = new CategoryBLLForAdmin();
            int categoryNumber = await categoryBLL.CountAllAsync();

            Console.WriteLine($"Tổng số thể loại: {categoryNumber}");
        }
    }
}
