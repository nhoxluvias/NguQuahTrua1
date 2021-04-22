
using MSSQL_Lite.Access;
using MSSQL_Lite.Connection;
using MSSQL_Lite.Query;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Test.Model;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            //object obj = 34;
            //ModelError.IfInvalidType(obj, new string[] { "string", "float" });

            SqlConnectInfo.DataSource = @"LAPTOP-B78E1G5S\MSSQLSERVER2019";
            SqlConnectInfo.InitialCatalog = "AddressTest";
            SqlConnectInfo.UserID = "sa";
            SqlConnectInfo.Password = "123456789";



            Run().GetAwaiter().GetResult();

            Console.ReadKey();
        }

        static async Task Run()
        {
            DBContext db = new DBContext();

            List<District> districts = await db.Districts.ToListAsync();
            //////var item = sqlData.ToDictionaryList();
            Console.OutputEncoding = Encoding.UTF8;
            foreach (District d in districts)
            {
                Console.WriteLine($"ID {d.ID} -- Tên xã: {d.name}");
            }

            //SqlTable table = SqlMapping.GetTable<User>();



            UserInfo userInfo = new UserInfo
            {
                ID = "JFKSDJFLKS",
                userName = "phanxuanchanh",
                surName = "Phan",
                middleName = "Xuân",
                name = "Chánh",
                email = "phanxuanchanh77@gmail.com",
                phoneNumber = "0343583276",
                description = "...",
                createAt = DateTime.Now,
                updateAt = DateTime.Now
            };

            //string str = SqlQuery.Insert<UserInfo>(userInfo);
            //Console.WriteLine(str);

        }
    }
}
