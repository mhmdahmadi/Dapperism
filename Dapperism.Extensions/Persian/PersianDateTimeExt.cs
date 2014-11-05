using System;

namespace Dapperism.Extensions.Persian
{
    public static class PersianDateTimeExt
    {

        //Hamed Fathi
        public static PersianDateTime ToPersianDateTime(this DateTime dateTime)
        {
            return new PersianDateTime(dateTime);
        }

        public static bool IsBetweenEqual(this PersianDateTime dateTime, PersianDateTime startDateTime, PersianDateTime endDateTime)
        {
            return (dateTime >= startDateTime && dateTime <= endDateTime);
        }
        public static bool IsBetween(this PersianDateTime dateTime, PersianDateTime startDateTime, PersianDateTime endDateTime)
        {
            return (dateTime > startDateTime && dateTime < endDateTime);
        }
        public static bool IsLessThanEqual(this PersianDateTime dateTime, PersianDateTime dt)
        {
            return dateTime <= dt;
        }

        public static bool IsLessThan(this PersianDateTime dateTime, PersianDateTime dt)
        {
            return dateTime < dt;
        }

        public static bool IsGreaterThanEqual(this PersianDateTime dateTime, PersianDateTime dt)
        {
            return dateTime >= dt;
        }

        public static bool IsGreaterThan(this PersianDateTime dateTime, PersianDateTime dt)
        {
            return dateTime > dt;
        }

        //Hamed Fathi

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
    }
}