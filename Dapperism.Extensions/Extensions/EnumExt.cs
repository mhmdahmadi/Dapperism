using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

namespace Dapperism.Extensions.Extensions
{
    public static class EnumExt
    {
        public static string Description(this Enum someEnum)
        {
            var memInfo = someEnum.GetType().GetMember(someEnum.ToString());
            if (memInfo.Length <= 0) return someEnum.ToString();
            var attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attrs.Length > 0 ? ((DescriptionAttribute)attrs[0]).Description : someEnum.ToString();
        }
        public static bool HasDescription(this Enum someEnum)
        {
            return !string.IsNullOrWhiteSpace(someEnum.Description());
        }
        public static bool HasDescription(this Enum someEnum, string expectedDescription)
        {
            return someEnum.Description().Equals(expectedDescription);
        }
        public static bool HasDescription(this Enum someEnum, string expectedDescription, StringComparison comparisionType)
        {
            return someEnum.Description().Equals(expectedDescription, comparisionType);
        }

        public static TEnum Parse<TEnum>(this string value)  where TEnum : struct 
        {
            return Parse<TEnum>(value, true);
        }

        public static TEnum Parse<TEnum>(this string value, bool ignoreCase) where TEnum : struct 
        {
            return (TEnum)Enum.Parse(typeof(TEnum), value, ignoreCase);
        }

        public static bool TryParse<TEnum>(this string value, out TEnum returnedValue) where TEnum : struct 
        {
            return TryParse(value, true, out returnedValue);
        }

        public static bool TryParse<TEnum>(this string value, bool ignoreCase, out TEnum returnedValue)  where TEnum : struct 
        {
            try
            {
                returnedValue = (TEnum)Enum.Parse(typeof(TEnum), value, ignoreCase);
                return true;
            }
            catch
            {
                returnedValue = default(TEnum);
                return false;
            }
        }

        public static string GetName(this Enum value)
        {
            return value.ToString();
        }

        public static int GetValue(this Enum value)
        {
            return Convert.ToInt32(value);
        }

        public static TEnum ToEnum<TEnum>(this string value, bool ignorecase) where TEnum : struct 
        {
            if (value == null)
                throw new ArgumentNullException("Value");

            value = value.Trim();

            if (value.Length == 0)
                throw new ArgumentNullException("Must specify valid information for parsing in the string.", "value");

            var t = typeof(TEnum);
            if (!t.IsEnum)
                throw new ArgumentException("Type provided must be an Enum.", "TEnum");

            return (TEnum)Enum.Parse(t, value, ignorecase);
        }

        public static TEnum ToEnum<TEnum>(this int value) where TEnum : struct 
        {
            var t = typeof(TEnum);
            if (!t.IsEnum)
                throw new ArgumentException("Type provided must be an Enum.", "TEnum");

            return (TEnum)Enum.Parse(t, value.ToString(CultureInfo.InvariantCulture));
        }      
    }

















    public static class Enum<T> where T : struct, IComparable, IFormattable, IConvertible
    {
        public static IEnumerable<int> GetValues()
        {
            var t = typeof(T);
            if (!t.IsEnum)
                throw new ArgumentException("Type provided must be an Enum.", "T");

            return (int[])Enum.GetValues(typeof(T));
        }

        public static IEnumerable<string> GetNames()
        {
            var t = typeof(T);
            if (!t.IsEnum)
                throw new ArgumentException("Type provided must be an Enum.", "T");
            return Enum.GetNames(typeof(T));
        }
    }
}
