using System.Linq;

namespace Tessin.Tin.Extensions
{
    internal static class GenericExtensions
    {
        public static bool In<T>(this T item, params T[] array)
        {
            return array.Contains(item);
        }
    }
}
