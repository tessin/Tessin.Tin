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

        public static string RemoveAllNonNumeric(this string value)
        {
            return RemoveAllExcept(value, "0123456789");
        }

    }
}
