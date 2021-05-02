using MSSQL_Lite.Access;
using MSSQL_Lite.Connection;
using System;
using System.Web.Routing;
using Web.App_Start;
using Web.Migrations;

namespace Web
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            SqlConnectInfo.DataSource = @"LAPTOP-B78E1G5S\MSSQLSERVER2019";
            SqlConnectInfo.InitialCatalog = "Movie";
            SqlConnectInfo.UserID = "sa";
            SqlConnectInfo.Password = "123456789";
            SqlData.objectReceivingData = ObjectReceivingData.DataSet;

            RoleMigration roleMigration = new RoleMigration();
            roleMigration.AddDataAndRun();
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}