using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tessin.Tin
{
    public static class Utils
    {
        public static bool IsValidDate(int year, int month, int day)
        {
            // 30 days hath September,
            // April, June and November,
            // All the rest have 31,
            // Excepting February alone
            // (And that has 28 days clear,
            // With 29 in each leap year).
            if (year < 1 || year > 9999) return false;
            return (month == 4 || month == 6 || month == 9 || month == 11)
                             ? day < 31
                             : (month == 2
                             ? (DateTime.IsLeapYear(year)
                             ? day < 30 : day < 29)
                             : day < 32) && day > 0;
        }
    }
}
