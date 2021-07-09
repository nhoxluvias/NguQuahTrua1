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
            string filmId = context.Request.Form["filmId"];

            context.Response.ContentType = "text/plain";
            if (string.IsNullOrEmpty(filmId))
            {
                context.Response.Write("Không thể thực hiện. Lý do: Dữ liệu đầu vào không hợp lệ");
            }
            else
            {
                FilmBLL filmBLL = new FilmBLL(DataAccessLevel.User);
                StateOfUpdate state = filmBLL.Upvote(filmId);
                if (state == StateOfUpdate.Success)
                    context.Response.Write("Đánh giá (không thích) phim thành công");
                else
                    context.Response.Write("Đánh giá (không thích) phim thất bại");
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