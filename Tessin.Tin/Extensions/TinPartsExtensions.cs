using System;
using Tessin.Tin.Models;

namespace Tessin.Tin.Extensions
{
    public static class TinPartsExtensions
    {

        //public static bool HasValidDate(this TinParts parts)
        //{
        //    var year = parts.LongYearNumeric;
        //    var month = parts.MonthNumeric;
        //    var day = parts.DayNumeric;
        //    if (year == null || month == null || day == null) return false;
        //    return Utils.IsValidDate(year.Value, month.Value, day.Value);
        //}

        //public static DateTime? GetDate(this TinParts parts)
        //{
        //    if (!parts.HasValidDate()) return null;
        //    var year = parts.LongYearNumeric;
        //    var month = parts.MonthNumeric;
        //    var day = parts.DayNumeric;
        //    if (year == null || month == null || day == null) return null;
        //    return new DateTime(year.Value, month.Value, day.Value, 0, 0, 0);
        //}

        //public static void SetDate(this TinParts parts, string date, TinDateFormat format)
        //{
        //    if (date.Length != 6) throw new ArgumentException("The datestring must be exactly 6 characters long.");
        //    var p1 = date.Substring(0, 2);
        //    var p2 = date.Substring(2, 2);
        //    var p3 = date.Substring(4, 2);
        //    switch (format)
        //    {
        //        case TinDateFormat.YearMonthDay:
        //            parts.Year = p1;
        //            parts.Month = p2;
        //            parts.Day = p3;
        //            break;
        //        case TinDateFormat.DayMonthYear:
        //            parts.Year = p3;
        //            parts.Month = p2;
        //            parts.Day = p1;
        //            break;
        //        default:
        //            throw new ArgumentOutOfRangeException(nameof(format), format, null);
        //    }
        //}
    }
}
