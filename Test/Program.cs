using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using Test.Config;
using Test.DAL;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.OutputEncoding = Encoding.UTF8;

            DatabaseConfig.ManualConfig(@"LAPTOP-B78E1G5S\MSSQLSERVER2019", "Movie", "sa", "123456789");

            Run().GetAwaiter().GetResult();

            Console.ReadKey();
            Console.ReadKey();
            Console.ReadKey();
        }

        static async Task Run()
        {
            //2002
            DBContext db = new DBContext(MSSQL_Lite.Connection.ConnectionType.DisconnectAfterCompletion);
            List<Film> films = await db.Films.ToListAsync();

            foreach(Film film in films)
            {
                Console.WriteLine($"{film.name} -- {film.createAt}");
            }
            //await db.Films.UpdateAsync(new Film { updateAt = DateTime.Now }, f => new { f.updateAt }, f => f.ID == "77f7a818b85480c8f6b7c53a5aab1361");
        }
    }
}
