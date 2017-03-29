using System;
using System.Linq;
using System.Text;

namespace Tessin.Tin.Extensions
{
    public static class StringExtensions
    {

        public static bool ContainsAnyOf(this string value, string pattern)
        {
            var array = pattern.ToCharArray();
            return value.Any(c => array.Contains(c));
        }

        public static bool ConformsTo(this string value, string pattern)
        {
            // Fix for logically consistent but quirky behavior of All.
            if (string.IsNullOrEmpty(value)) return false;
            var array = pattern.ToCharArray();
            return value.All(c => array.Contains(c));
        }

        public static string RemoveAllExcept(this string value, string pattern)
        {
            var sb = new StringBuilder();
            var array = pattern.ToCharArray();
            foreach (var c in value)
            {
                if (array.Contains(c))
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }

        public static string RemoveAll(this string value, string pattern)
        {
            var sb = new StringBuilder();
            var array = pattern.ToCharArray();
            foreach (var c in value)
            {
                if (!array.Contains(c))
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }

        public static string RemoveAllWhitespace(this string value)
        {
            return value.RemoveAll(" \t\r\n\b\f\v\0");
        }

        public static string RemoveAllNonNumeric(this string value)
        {
            return RemoveAllExcept(value, "0123456789");
        }

        public static int CalculateMod11Checksum(this string number, int[] weights, Func<int,int> adjustment = null)
        {
            if (number.Length != weights.Length)
                throw new ArgumentException(
                    "The number of characters must equal " +
                    "the number of weights.");
            var sum = 0;
            for (var i = 0; i < weights.Length; i++)
            {
                var num = (int)char.GetNumericValue(number[i]);
                sum += num * weights[i];
            }
            var chk = (11 - sum % 11);
            return adjustment?.Invoke(chk) ?? chk;
        }

    }
}
