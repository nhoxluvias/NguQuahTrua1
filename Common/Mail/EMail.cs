using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Common.Mail
{
    public class EMail
    {
        public bool Send(string sendFrom, string sendTo, string subject, string message)
        {
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.EnableSsl = true;
            smtp.Credentials = new NetworkCredential("phanxuanchanh77@gmail.com", "fkvissggfyytiedg");
            smtp.Send(sendFrom, sendTo, subject, message);
            return true;
        }
    }
}
