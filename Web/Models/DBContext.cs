
using MSSQL_Lite.Access;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Web.Models
{
    public class DBContext:SqlContext
    {
        public DBContext()
            :base()
        {

        }

        public SqlAccess<Role> Roles { get { return new SqlAccess<Role>(); } }
        public SqlAccess<User> Users { get { return new SqlAccess<User>(); } }
        public SqlAccess<PaymentMethod> PaymentMethods { get { return new SqlAccess<PaymentMethod>(); } }
        public SqlAccess<PaymentInfo> PaymentInfos { get { return new SqlAccess<PaymentInfo>(); } }
        public SqlAccess<Category> Categories { get { return new SqlAccess<Category>(); } }
        public SqlAccess<Tag> Tags { get { return new SqlAccess<Tag>(); } }
        public SqlAccess<Film> Films { get { return new SqlAccess<Film>(); } }
    }
}