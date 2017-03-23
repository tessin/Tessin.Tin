using System;

namespace Tessin.Tin.Extensions
{
    public static class DateTimeExtensions
    {

        private const double AverageDaysPerYear = 365.25;

        public static int? ToAge(this DateTime? dateTime)
        {
            return dateTime?.ToAge();
        }

        public static int ToAge(this DateTime date)
        {
            var year = date.Year;
            var month = date.Month;
            var day = date.Day;

            var now = TinGlobal.Now;

            var years = now.Year - year;

            years -= now.Month < month || (now.Month == month && now.Day < day) ? 1 : 0;

            return years;
        }
    }
}
