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

        public static int ToAge(this DateTime dateTime)
        {
            var utc = dateTime.ToUniversalTime();
            var span = DateTime.UtcNow - utc;
            // Precise enough?
            var years = Math.Floor(span.TotalDays/AverageDaysPerYear);
            return (int)years;
        }
    }
}
