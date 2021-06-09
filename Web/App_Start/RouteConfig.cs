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
            routes.MapPageRoute("Logout", "account/logout", "~/Account/Logout.aspx");
            routes.MapPageRoute("Register", "account/register", "~/Account/Register.aspx");
            routes.MapPageRoute("ResetPassword", "account/reset-password", "~/Account/ResetPassword.aspx");
            routes.MapPageRoute("Account_NewPassword", "account/new-password/{userId}/{newPasswordToken}", "~/Account/NewPassword.aspx");
            routes.MapPageRoute("Confirm", "account/confirm/{userId}/{confirmToken}/{type}", "~/Account/Confirm.aspx");

            routes.MapPageRoute("Error", "error", "~/Notification/Error.aspx");

            routes.MapPageRoute("Admin_Overview", "admin/overview", "~/Admin/Index.aspx");
            routes.MapPageRoute("Admin_CategoryList", "admin/category/list", "~/Admin/CategoryManagement/CategoryList.aspx");
            routes.MapPageRoute("Admin_CategoryDetail", "admin/category/detail/{id}", "~/Admin/CategoryManagement/CategoryDetail.aspx");
            routes.MapPageRoute("Admin_CreateCategory", "admin/category/create", "~/Admin/CategoryManagement/CreateCategory.aspx");
            routes.MapPageRoute("Admin_UpdateCategory", "admin/category/update/{id}", "~/Admin/CategoryManagement/UpdateCategory.aspx");
            routes.MapPageRoute("Admin_DeleteCategory", "admin/category/delete/{id}", "~/Admin/CategoryManagement/DeleteCategory.aspx");
            routes.MapPageRoute("Admin_RoleList", "admin/role/list", "~/Admin/RoleManagement/RoleList.aspx");
            routes.MapPageRoute("Admin_RoleDetail", "admin/role/detail/{id}", "~/Admin/RoleManagement/RoleDetail.aspx");
            routes.MapPageRoute("Admin_CreateRole", "admin/role/create", "~/Admin/RoleManagement/CreateRole.aspx");
            routes.MapPageRoute("Admin_UpdateRole", "admin/role/update/{id}", "~/Admin/RoleManagement/UpdateRole.aspx");
            routes.MapPageRoute("Admin_DeleteRole", "admin/role/delete/{id}", "~/Admin/RoleManagement/DeleteRole.aspx");
            routes.MapPageRoute("Admin_CountryList", "admin/country/list", "~/Admin/CountryManagement/CountryList.aspx");
            routes.MapPageRoute("Admin_CountryDetail", "admin/country/detail/{id}", "~/Admin/CountryManagement/CountryDetail.aspx");
            routes.MapPageRoute("Admin_CreateCountry", "admin/country/create", "~/Admin/CountryManagement/CreateCountry.aspx");
            routes.MapPageRoute("Admin_UpdateCountry", "admin/country/update/{id}", "~/Admin/CountryManagement/UpdateCountry.aspx");
            routes.MapPageRoute("Admin_DeleteCountry", "admin/country/delete/{id}", "~/Admin/CountryManagement/DeleteCountry.aspx");
            routes.MapPageRoute("Admin_LanguageList", "admin/language/list", "~/Admin/LanguageManagement/LanguageList.aspx");
            routes.MapPageRoute("Admin_LanguageDetail", "admin/language/detail/{id}", "~/Admin/LanguageManagement/LanguageDetail.aspx");
            routes.MapPageRoute("Admin_CreateLanguage", "admin/language/create", "~/Admin/LanguageManagement/CreateLanguage.aspx");
            routes.MapPageRoute("Admin_UpdateLanguage", "admin/language/update/{id}", "~/Admin/LanguageManagement/UpdateLanguage.aspx");
            routes.MapPageRoute("Admin_DeleteLanguage", "admin/language/delete/{id}", "~/Admin/LanguageManagement/DeleteLanguage.aspx");
            routes.MapPageRoute("Admin_DirectorList", "admin/director/list", "~/Admin/DirectorManagement/DirectorList.aspx");
            routes.MapPageRoute("Admin_DirectorDetail", "admin/director/detail/{id}", "~/Admin/DirectorManagement/DirectorDetail.aspx");
            routes.MapPageRoute("Admin_CreateDirector", "admin/director/create", "~/Admin/DirectorManagement/CreateDirector.aspx");
            routes.MapPageRoute("Admin_UpdateDirector", "admin/director/update/{id}", "~/Admin/DirectorManagement/UpdateDirector.aspx");
            routes.MapPageRoute("Admin_DeleteDirector", "admin/director/delete/{id}", "~/Admin/DirectorManagement/DeleteDirector.aspx");
            routes.MapPageRoute("Admin_CastList", "admin/cast/list", "~/Admin/CastManagement/CastList.aspx");
            routes.MapPageRoute("Admin_CastDetail", "admin/cast/detail/{id}", "~/Admin/CastManagement/CastDetail.aspx");
            routes.MapPageRoute("Admin_UpdateCast", "admin/cast/update/{id}", "~/Admin/CastManagement/UpdateCast.aspx");
            routes.MapPageRoute("Admin_DeleteCast", "admin/cast/delete/{id}", "~/Admin/CastManagement/DeleteCast.aspx");
            routes.MapPageRoute("Admin_TagList", "admin/tag/list", "~/Admin/TagManagement/TagList.aspx");
            routes.MapPageRoute("Admin_TagDetail", "admin/tag/detail/{id}", "~/Admin/TagManagement/TagDetail.aspx");
            routes.MapPageRoute("Admin_CreateTag", "admin/tag/create", "~/Admin/TagManagement/CreateTag.aspx");
            routes.MapPageRoute("Admin_UpdateTag", "admin/tag/update/{id}", "~/Admin/TagManagement/UpdateTag.aspx");
            routes.MapPageRoute("Admin_DeleteTag", "admin/tag/delete/{id}", "~/Admin/TagManagement/DeleteTag.aspx");
            routes.MapPageRoute("Admin_UserList", "admin/user/list", "~/Admin/UserManagement/UserList.aspx");
            routes.MapPageRoute("Admin_CreateUser", "admin/user/create", "~/Admin/UserManagement/CreateUser.aspx");


            routes.MapPageRoute("User_Home", "", "~/User/Index.aspx");
            routes.MapPageRoute("User_Category", "category/{id}", "~/User/Category.aspx");
            routes.MapPageRoute("User_FilmDetails", "film-details/{slug}/{id}", "~/User/FilmDetails.aspx");

            routes.MapPageRoute("Notification_Error", "notification/error", "~/Notification/Error.aspx");
        }
    }
}