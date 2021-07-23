using Common.Web;
using Data.Config;
using System;
using System.Web.Routing;
using Web.App_Start;

namespace Web
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            DatabaseConfig.CreateConnectionString(@"LAPTOP-B78E1G5S\MSSQLSERVER2019", "Movie", "sa", "123456789");
            DatabaseConfig.OtherSettings();
            MigrationConfig.Migrate();
            EmailConfig.RegisterEmail();
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            PageVisitor.Add();
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
            PageVisitor.Remove();
        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}