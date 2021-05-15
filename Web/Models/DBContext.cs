
using MSSQL_Lite.Access;
using MSSQL_Lite.Connection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Web.Models
{
    public class DBContext:SqlContext
    {
        public DBContext(ConnectionType connectionType)
            :base(connectionType)
        {

        }

        public SqlAccess<Role> Roles { get { return InitSqlAccess<Role>(); } }
        public SqlAccess<User> Users { get { return InitSqlAccess<User>(); } }
        public SqlAccess<PaymentMethod> PaymentMethods { get { return InitSqlAccess<PaymentMethod>(); } }
        public SqlAccess<PaymentInfo> PaymentInfos { get { return InitSqlAccess<PaymentInfo>(); } }
        public SqlAccess<Category> Categories { get { return InitSqlAccess<Category>(); } }
        public SqlAccess<Tag> Tags { get { return InitSqlAccess<Tag>(); } }
        public SqlAccess<Film> Films { get { return InitSqlAccess<Film>(); } }
    }
}