using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Dapperism.Extensions.Extensions
{
    public static class StringExt
    {

        public static string DefaultIfEmpty(this string str, string defaultValue, bool considerWhiteSpaceIsEmpty = false)
        {
            return (considerWhiteSpaceIsEmpty ? string.IsNullOrWhiteSpace(str) : string.IsNullOrEmpty(str)) ? defaultValue : str;
        }

        public static string FindBetween(this string src, string findfrom, string findto)
        {
            var start = src.IndexOf(findfrom, StringComparison.Ordinal);
            var to = src.IndexOf(findto, start + findfrom.Length, StringComparison.Ordinal);
            if (start < 0 || to < 0) return "";
            var s = src.Substring(
                start + findfrom.Length,
                to - start - findfrom.Length);
            return s;
        }       

        public static string Reverse(this string input)
        {
            var array = input.ToCharArray();
            Array.Reverse(array);
            return new string(array);
        }

        public static string[] Split(this string value, string regexPattern, RegexOptions options)
        {
            return Regex.Split(value, regexPattern, options);
        }

        public static string[] Split(this string value, string splitter)
        {
            var stringSeparator = new[] { splitter };
            return value.Split(stringSeparator, StringSplitOptions.None);
        }

        public static string[] Split(this string value, string[] splitters)
        {
            return value.Split(splitters, StringSplitOptions.None);
        }

        public static byte[] ToByteArray(this string str)
        {
            return Encoding.UTF8.GetBytes(str);
        }

        public static string ToStringFormat(this string stringFormat, params object[] stringParams)
        {
            return string.Format(stringFormat, stringParams);
        }



        public static string RemoveHtmlTags(this string htmlString)
        {
            var text = Regex.Replace(htmlString, "<.*?>", string.Empty);
            var newText = new StringWriter();
            HttpUtility.HtmlDecode(text, newText);
            return newText.ToString();
        }    

        public static bool ContainsIgnoreCase(this string source, string target)
        {
            return source.IndexOf(target, StringComparison.CurrentCultureIgnoreCase) >= 0;
        }    
    }
}
