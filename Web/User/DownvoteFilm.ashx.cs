using Data.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.User
{
    /// <summary>
    /// Summary description for DownvoteFilm
    /// </summary>
    public class DownvoteFilm : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            FilmBLL filmBLL = new FilmBLL();

            try
            {
                string filmId = context.Request.Form["filmId"];
                if (string.IsNullOrEmpty(filmId))
                {
                    context.Response.Write("Không thể thực hiện. Lý do: Dữ liệu đầu vào không hợp lệ");
                }
                else
                {
                    StateOfUpdate state = filmBLL.Downvote(filmId);
                    if (state == StateOfUpdate.Success)
                        context.Response.Write("Đánh giá (không thích) phim thành công");
                    else
                        context.Response.Write("Đánh giá (không thích) phim thất bại");
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