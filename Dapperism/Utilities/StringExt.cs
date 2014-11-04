using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dapperism.Utilities
{
   public static class StringExt
    {
       public static string ToEnglishNumber(this string input)
       {
           if (input.Trim() == "") return "";
           input = input.Replace("۰", "0");
           input = input.Replace("۱", "1");
           input = input.Replace("۲", "2");
           input = input.Replace("۳", "3");
           input = input.Replace("۴", "4");
           input = input.Replace("۵", "5");
           input = input.Replace("۶", "6");
           input = input.Replace("۷", "7");
           input = input.Replace("۸", "8");
           input = input.Replace("۹", "9");
           return input;
       }

       public static string FixArabicChars(this string value)
       {
           return value.Replace("ك", "ک").Replace("ي", "ی").Replace("ة", "ه");
       }
    }
}
