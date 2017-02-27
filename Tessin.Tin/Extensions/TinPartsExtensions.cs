using System;
using Tessin.Tin.Models;

namespace Tessin.Tin.Extensions
{
    public static class TinPartsExtensions
    {

        public static bool HasValidDate(this TinParts parts)
        {
            if (!parts.LongYearNumeric.HasValue || !parts.MonthNumeric.HasValue || !parts.DayNumeric.HasValue ||
                !parts.Century.HasValue) return false;
            return Utils.IsValidDate(parts.LongYearNumeric.Value, parts.MonthNumeric.Value, parts.DayNumeric.Value);
        }

        public static DateTime? GetDate(this TinParts parts)
        {
            if (!parts.HasValidDate()) return null;
            return new DateTime(parts.LongYearNumeric.Value, 
                parts.MonthNumeric.Value, 
                parts.DayNumeric.Value, 0, 0, 0);
        }

        public static void SetDate(this TinParts parts, string date, TinDateFormat format)
        {
            if (date.Length != 6) throw new ArgumentException("The datestring must be exactly 6 characters long.");
            var p1 = date.Substring(0, 2);
            var p2 = date.Substring(2, 2);
            var p3 = date.Substring(4, 2);
            switch (format)
            {
                case TinDateFormat.YearMonthDay:
                    parts.Year = p1;
                    parts.Month = p2;
                    parts.Day = p3;
                    break;
                case TinDateFormat.DayMonthYear:
                    parts.Year = p3;
                    parts.Month = p2;
                    parts.Day = p1;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(format), format, null);
            }
        }
    }
}
