
using System;

namespace Dapperism.Extensions.Extensions
{
    public static class TimeSpanExtensions
    {

        public static DateTime Ago(this TimeSpan ts)
        {
            return DateTime.Now.Subtract(ts);
        }
        public static DateTime UtcAgo(this TimeSpan ts)
        {
            return DateTime.UtcNow.Subtract(ts);
        }

        public static DateTime FromNow(this TimeSpan ts)
        {
            return DateTime.Now.Add(ts);
        }

        public static DateTime UtcFromNow(this TimeSpan ts)
        {
            return DateTime.UtcNow.Add(ts);
        }
    }
}
