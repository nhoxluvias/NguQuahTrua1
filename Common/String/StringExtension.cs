using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace System
{
    public static class StringExtension
    {
        public static string TextToUrl(this string text)
        {
            Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            string url = text.Normalize(NormalizationForm.FormD).Trim().ToLower();

            url = regex.Replace(url, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D').Replace(",", "-").Replace(".", "-")
                        .Replace("!", "").Replace("(", "").Replace(")", "").Replace(";", "-").Replace("/", "-")
                        .Replace("%", "ptram").Replace("&", "va").Replace("?", "").Replace('"', '-').Replace(' ', '-');
            return url;
        }
    }
}
