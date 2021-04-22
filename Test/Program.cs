using Common.Command;
using Common.FFMPEG;
using MSSQL.Error;
using MSSQL.Reflection;
using MSSQL.Sql;
using MSSQL.Sql.Access;
using MSSQL.Sql.Connection;
using MSSQL.Sql.Execution;
using MSSQL.Sql.Query;
using MSSQL.Sql.Tables;
using MSSQL.String;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
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
            SqlConnectInfo.InitialCatalog = "Movie";
            SqlConnectInfo.UserID = "sa";
            SqlConnectInfo.Password = "123456789";



            Run().GetAwaiter().GetResult();

            Console.ReadKey();
        }

        static async Task Run()
        {
            //string str = "Phường Trúc Bạch";
            //SqlData sqlData = await SqlData.ExecuteReaderAsync($"Select * from Commune");
            //List<District> districts = sqlData.ToList<District>();
            //////var item = sqlData.ToDictionaryList();
            //Console.OutputEncoding = Encoding.UTF8;
            //foreach (District d in districts)
            //{
            //    Console.WriteLine($"ID {d.ID} -- Tên xã: {d.name}");
            //}

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

            string str = SqlQuery.Insert<UserInfo>(userInfo);
            Console.WriteLine(str);

        }
    }
}
