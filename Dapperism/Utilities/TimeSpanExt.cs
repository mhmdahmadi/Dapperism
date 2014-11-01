
using System;

namespace Dapperism.Utilities
{
    public static class TimeSpanExtensions
    {
        // Start - PersianDateTime Extensions
        public static int ToInteger(this TimeSpan time)
        {
            return int.Parse(time.Hours.ToString() + time.Minutes.ToString().PadLeft(2, '0') + time.Seconds.ToString().PadLeft(2, '0'));
        }

        public static short ToShort(this TimeSpan time)
        {
            return short.Parse(time.Hours.ToString() + time.Minutes.ToString().PadLeft(2, '0'));
        }

        public static string ToHHMM(this TimeSpan time)
        {
            return time.ToString("hh\\:mm");
        }

        public static string ToHHMMSS(this TimeSpan time)
        {
            return time.ToString("hh\\:mm\\:ss");
        }
        // End - PersianDateTime Extensions
    }
}
