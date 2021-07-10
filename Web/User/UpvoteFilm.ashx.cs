using Data.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Web.User
{
    /// <summary>
    /// Summary description for UpvoteFilm
    /// </summary>
    public class UpvoteFilm : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            FilmBLL filmBLL = new FilmBLL(DataAccessLevel.User);

            try
            {
                string filmId = context.Request.Form["filmId"];
                if (string.IsNullOrEmpty(filmId))
                {
                    context.Response.Write("Không thể thực hiện. Lý do: Dữ liệu đầu vào không hợp lệ");
                }
                else
                {
                    StateOfUpdate state = filmBLL.Upvote(filmId);
                    if (state == StateOfUpdate.Success)
                        context.Response.Write("Đánh giá (thích) phim thành công");
                    else
                        context.Response.Write("Đánh giá (thích) phim thất bại");

                    filmBLL.Dispose();
                }
            }
            catch(Exception ex)
            {
                filmBLL.Dispose();
                context.Response.Write(string.Format("Đã xảy ra ngoại lệ: {0}", ex.Message));
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}