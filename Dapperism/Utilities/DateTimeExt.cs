using System;

namespace Dapperism.Utilities
{
    public static class DateTimeExt
    {
        public static bool IsBetweenEqual(this DateTime dateTime, DateTime startDateTime, DateTime endDateTime)
        {
            return (dateTime >= startDateTime && dateTime <= endDateTime);
        }
        public static bool IsBetween(this DateTime dateTime, DateTime startDateTime, DateTime endDateTime)
        {
            return (dateTime > startDateTime && dateTime < endDateTime);
        }
        public static bool IsLessThanEqual(this DateTime dateTime, DateTime dt)
        {
            return dateTime <= dt;
        }

        public static bool IsLessThan(this DateTime dateTime, DateTime dt)
        {
            return dateTime < dt;
        }

        public static bool IsGreaterThanEqual(this DateTime dateTime, DateTime dt)
        {
            return dateTime >= dt;
        }

        public static bool IsGreaterThan(this DateTime dateTime, DateTime dt)
        {
            return dateTime > dt;
        }

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
    }
}
