using MSSQL_Lite.Connection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web.Models;
using Web.Models.DTO;

namespace Web.User
{
    public partial class Category : System.Web.UI.Page
    {
        private DBContext db;
        protected string categoryName;
        protected List<FilmInfo> films;

        protected async void Page_Load(object sender, EventArgs e)
        {
            db = new DBContext(ConnectionType.ManuallyDisconnect);
            await GetCategoryId();
        }

        private async Task GetCategoryId()
        {
            object obj = Page.RouteData.Values["id"];
            if(obj == null)
            {

            }
            else
            {
                int categoryId = int.Parse((string)obj);
                SqlCommand sqlCommand = new SqlCommand();
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.CommandText = "Select [Category].[name] from [Category] where [Category].[ID] = @categoryId";
                sqlCommand.Parameters.Add(new SqlParameter("@categoryId", categoryId));
                object categoryObj = await db.ExecuteScalarAsync(sqlCommand);
                if (categoryObj == null)
                {
                    
                }
                else
                {
                    categoryName = (string)categoryObj;
                    string query = @"Select [Film].* from [Film], [CategoryDistribution] 
                            where [Film].[ID] = [CategoryDistribution].[filmId]
                                and [CategoryDistribution].[categoryId] = @categoryId";
                    SqlCommand command = new SqlCommand();
                    command.CommandType = CommandType.Text;
                    command.CommandText = query;
                    command.Parameters.Add(new SqlParameter("@categoryId", categoryId));
                    films = (await db.ExecuteReaderAsync<List<FilmInfo>>(command))
                        .Select(f => new FilmInfo {
                            ID = f.ID,
                            name = f.name,
                            thumbnail = VirtualPathUtility.ToAbsolute("~/images/") + f.thumbnail,
                            url = GetRouteUrl("User_FilmDetails", new { slug = f.name.TextToUrl(), id = f.ID })
                        }).ToList();
                }
            }
        }
    }
}