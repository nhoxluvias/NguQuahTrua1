
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
            Console.ReadKey();
            Console.ReadKey();
        }

        static async Task Run()
        {
            DBContext db = new DBContext();

            //List<District> districts = await db.Districts.ToListAsync();
            ////////var item = sqlData.ToDictionaryList();
            //Console.OutputEncoding = Encoding.UTF8;
            //foreach (District d in districts)
            //{
            //    Console.WriteLine($"ID {d.ID} -- Tên xã: {d.name}");
            //}

            ////SqlTable table = SqlMapping.GetTable<User>();

            //object obj = await db.ExecuteReaderAsync("Select * from Commune", typeof(List<Dictionary<string, object>>));

            //Dictionary<string, object> dict = (Dictionary<string, object>)obj;

            //SqlQuery.GetWhereStatement<UserInfo>(u => u.phoneNumber == "0343583276");

            //string str1 = "chanh";
            //int i = 32767;
            //SqlQuery.GetWhereStatement<UserInfo>(u => u.password == "32767" && u.phoneNumber == "0343583276" && u.userName == "phanxuanchanh");


            //string select = SqlQuery.GetSelectStatement<UserInfo>(u => new { u.ID, u.userName, u.email });

            //Console.WriteLine(select);

            //UserInfo userInfo = new UserInfo
            //{
            //    ID = "JFKSDJFLKS",
            //    userName = "phanxuanchanh",
            //    surName = "Phan",
            //    middleName = "Xuân",
            //    name = "Chánh",
            //    email = "phanxuanchanh77@gmail.com",
            //    phoneNumber = "0343583276",
            //    description = "...",
            //    createAt = DateTime.Now,
            //    updateAt = DateTime.Now
            //};

            //string set = SqlQuery.GetSetStatement<UserInfo>(userInfo, u => new { u.email, u.userName });
            //Console.WriteLine(set);

            //string str = SqlQuery.Insert<UserInfo>(userInfo);
            //Console.WriteLine(str);

            SqlQuery.EnclosedInSquareBrackets = true;

            string select1 = SqlQuery.Select<UserInfo>();
            string select2 = SqlQuery.Select<UserInfo>(5);
            string select3 = SqlQuery.Select<UserInfo>(u => u.userName == "phanxuanchanh");
            string select4 = SqlQuery.Select<UserInfo>(u => new { u.userName, u.email });
            string select5 = SqlQuery.Select<UserInfo>(u => new { u.userName, u.email }, 5);
            string select6 = SqlQuery.Select<UserInfo>(u => u.roleId == "kdi300823jjds", 5);
            string select7 = SqlQuery.Select<UserInfo>(u => new { u.userName, u.email }, u => u.userName == "phanxuanchanh");
            string select8 = SqlQuery.Select<UserInfo>(u => new { u.userName, u.email }, u => u.roleId == "kdi300823jjds", 5);

            string select9 = SqlQuery.Select<UserInfo>(u => u.userName == "phanxuanchanh" && (u.roleId == "jsahf" || u.roleId == "342d"));

            Console.WriteLine(select1);
            Console.WriteLine(select2);
            Console.WriteLine(select3);
            Console.WriteLine(select4);
            Console.WriteLine(select5);
            Console.WriteLine(select6);
            Console.WriteLine(select7);
            Console.WriteLine(select8);

            Console.WriteLine("\n\n");

            Console.WriteLine(select9);

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

            string insert = SqlQuery.Insert<UserInfo>(userInfo);

            Console.WriteLine("\n\n");

            Console.WriteLine(insert);

            string delete1 = SqlQuery.Delete<UserInfo>();
            string delete2 = SqlQuery.Delete<UserInfo>(u => u.userName == "phanxuanchanh");

            Console.WriteLine("\n\n");

            Console.WriteLine(delete1);
            Console.WriteLine(delete2);


            string update1 = SqlQuery.Update<UserInfo>(userInfo, u => new { u.phoneNumber, u.email });
            string update2 = SqlQuery.Update<UserInfo>(userInfo, u => new { u.phoneNumber, u.email }, u => u.userName == "phanxuanchanh");

            Console.WriteLine("\n\n");

            Console.WriteLine(update1);
            Console.WriteLine(update2);

        }
    }
}
