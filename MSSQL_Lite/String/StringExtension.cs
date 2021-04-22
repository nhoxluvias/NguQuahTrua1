using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSSQL_Lite.String
{
    public class StringExtension
    {
        public static string Substring(string input, int start, int end)
        {
            return input.Substring(0, end - start);
        }

        public static string Substring(string input, char first, char last)
        {
            int start_idx = input.IndexOf(first);
            int last_idx = input.LastIndexOf(last);
            if (start_idx == -1 || last_idx == -1)
                throw new Exception("");
            return StringExtension.Substring(input, start_idx, last_idx + 1);
        }

        public static string Substring(string input, string first, string last)
        {
            int start_idx = input.IndexOf(first);
            int last_idx = input.LastIndexOf(last);
            if (start_idx == -1 || last_idx == -1)
                throw new Exception("");
            return StringExtension.Substring(input, start_idx, last_idx + last.Length);
        }

        public static string ConvertStringArrayToString(string[] stringArray)
        {
            string strResult = null;
            foreach (string str in stringArray)
            {
                strResult += str + " ";
            }
            return strResult.TrimEnd(' '); ;
        }

        public static string ConvertStringArrayToString(string[] stringArray, char separator)
        {
            string strResult = null;
            foreach (string str in stringArray)
            {
                strResult += str + separator;
            }
            return strResult.TrimEnd(separator);
        }

        public static string ConvertStringArrayToString(string[] stringArray, string separator)
        {
            string strResult = null;
            foreach (string str in stringArray)
            {
                strResult += str + separator;
            }
            return strResult;
        }

        public static bool IsUnicode(string input)
        {
            return Encoding.ASCII.GetByteCount(input) != Encoding.UTF8.GetByteCount(input);
        }
    }
}
