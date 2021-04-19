using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MSSQL.String
{
    public class HtmlContent
    {
        public static string RemoveHtml(string text)
        {
            return Regex.Replace(text, "<[^>]*>", string.Empty);
        }

        public static string HtmlEncode(string src)
        {
            return System.Net.WebUtility.HtmlEncode(src);
        }

        public static string HtmlDecode(string src)
        {
            return System.Net.WebUtility.HtmlDecode(src);
        }

        public static string UrlEncode(string src)
        {
            return System.Web.HttpUtility.UrlEncode(src);
        }

        public static string UrlDecode(string src)
        {
            return System.Web.HttpUtility.UrlDecode(src);
        }
    }
}
