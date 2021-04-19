using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Common.Mail
{
    public class GoogleMail
    {
        public static bool Send(string sendFrom, string sendTo, string subject, string message)
        {
            SmtpClient smtp = new SmtpClient();
            //ĐỊA CHỈ SMTP Server
            smtp.Host = "smtp.gmail.com";
            //Cổng SMTP
            smtp.Port = 587;
            //SMTP yêu cầu mã hóa dữ liệu theo SSL
            smtp.EnableSsl = true;
            //UserName và Password của mail
            smtp.Credentials = new NetworkCredential("phanxuanchanh77@gmail.com", "fkvissggfyytiedg");

            //Tham số lần lượt là địa chỉ người gửi, người nhận, tiêu đề và nội dung thư
            smtp.Send(sendFrom, sendTo, subject, message);
            return true;
        }
    }
}
