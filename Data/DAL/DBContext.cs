using MSSQL_Lite.Access;
using MSSQL_Lite.Connection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DAL
{
    internal class DBContext : SqlContext
    {
        public DBContext(ConnectionType connectionType)
            : base(connectionType)
        {

        }

        public SqlAccess<Role> Roles { get { return InitSqlAccess<Role>(); } }
        public SqlAccess<User> Users { get { return InitSqlAccess<User>(); } }
        public SqlAccess<PaymentMethod> PaymentMethods { get { return InitSqlAccess<PaymentMethod>(); } }
        public SqlAccess<PaymentInfo> PaymentInfos { get { return InitSqlAccess<PaymentInfo>(); } }
        public SqlAccess<Category> Categories { get { return InitSqlAccess<Category>(); } }
        public SqlAccess<Tag> Tags { get { return InitSqlAccess<Tag>(); } }
        public SqlAccess<Film> Films { get { return InitSqlAccess<Film>(); } }
        public SqlAccess<Language> Languages { get { return InitSqlAccess<Language>(); } }
        public SqlAccess<Country> Countries { get { return InitSqlAccess<Country>(); } }
        public SqlAccess<CategoryDistribution> CategoryDistributons { get { return InitSqlAccess<CategoryDistribution>(); } }
        public SqlAccess<Director> Directors { get { return InitSqlAccess<Director>(); } }
        public SqlAccess<DirectorOfFilm> DirectorOfFilms { get { return InitSqlAccess<DirectorOfFilm>(); } }
    }
}
