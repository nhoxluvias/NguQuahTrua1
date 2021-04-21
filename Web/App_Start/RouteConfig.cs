using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace Web.App_Start
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.MapPageRoute("Login", "account/login", "~/Account/Login.aspx");
            routes.MapPageRoute("Register", "account/register", "~/Account/Login.aspx");
            routes.MapPageRoute("ResetPassword", "account/reset-password", "~/Account/ResetPassword");
            routes.MapPageRoute("Confirm", "account/confirm", "~/Account/Confirm");
        }
    }
}