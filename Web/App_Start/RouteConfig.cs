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
            routes.MapPageRoute("Login_WithParam", "account/login/{loginStatus}", "~/Account/Login.aspx");
            routes.MapPageRoute("Logout", "account/logout", "~/Account/Logout.aspx");
            routes.MapPageRoute("Register", "account/register", "~/Account/Register.aspx");
            routes.MapPageRoute("Register_WithParam", "account/register/{registerStatus}", "~/Account/Register.aspx");
            routes.MapPageRoute("ResetPassword", "account/reset-password", "~/Account/ResetPassword.aspx");
            routes.MapPageRoute("Confirm", "account/confirm/{userId}/{confirmToken}/{status}", "~/Account/Confirm.aspx");
            routes.MapPageRoute("Error", "error", "~/Notification/Error.aspx");
            routes.MapPageRoute("Home", "home", "~/User/Home.aspx");
        }
    }
}