﻿using System.Net;
using System.Net.Mail;

namespace Common.Mail
{
    public class EMail
    {
        public static string Address = null;
        public static string Password = null;

        public bool Send(string sendTo, string subject, string message)
        {
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.EnableSsl = true;
            smtp.Credentials = new NetworkCredential(Address, Password);
            smtp.Send(Address, sendTo, subject, message);
            return true;
        }
    }
}
