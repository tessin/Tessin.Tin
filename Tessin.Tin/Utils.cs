using System;
using System.Collections.Generic;
using System.Text;
using Tessin.Tin.Extensions;

namespace Tessin.Tin
{
    public class Utils
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
            if (!month.Between(1, 12)) return false;
            return (month == 4 || month == 6 || month == 9 || month == 11)
                             ? day < 31
                             : (month == 2
                             ? (DateTime.IsLeapYear(year)
                             ? day < 30 : day < 29)
                             : day < 32) && day > 0;
        }

        public static T RandomEnumValue<T>()
        {
            var v = Enum.GetValues(typeof(T));
            return (T)v.GetValue(new Random().Next(v.Length));
        }

        public static IEnumerable<string> GetRandomStrings(int n, int max, bool garbage = false)
        {
            var r = new Random(12132);
            for (var i = 0; i < n; i++)
            {
                var sb = new StringBuilder();
                var length = r.Next(max + 2) - 1;
                if (length == -1) yield return null;
                for (var j = 0; j < length; j++)
                {
                    var c = garbage ? (char)r.Next(128) : GetSemiRandomCharacter(r);
                    sb.Append(c);
                }
                yield return sb.ToString();
            }
        }

        [ThreadStatic]
        private static string _pattern;

        private static char GetSemiRandomCharacter(Random r)
        {
            const string upperCase = "ABCDEFGHIJKLMNOPQRSTUVWXYZÅÄÖ";
            const string lowerCase = "abcdefghijklmnopqrstuvwxyzåäö";
            const string digits = "0123456789";
            const string whitespace = "\n\t ";
            const string misc = "_+*-!\"#%&/()@£€[]{}.:,;|><`´=";

            if (_pattern == null)
            { 
                var sb = new StringBuilder();
                sb.Append(Duplicate(upperCase, 10));
                sb.Append(Duplicate(lowerCase, 10));
                sb.Append(Duplicate(digits, 100));
                sb.Append(Duplicate(whitespace, 20));
                sb.Append(misc);
                _pattern = sb.ToString();
            }

            return _pattern[r.Next(_pattern.Length)];
        }

        private static string Duplicate(string value, int n)
        {
            var sb = new StringBuilder();
            for (var i = 0; i < n; i++)
            {
                sb.Append(value);
            }
            return sb.ToString();
        }

    }
}
