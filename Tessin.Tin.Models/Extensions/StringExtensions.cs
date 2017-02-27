using System;

namespace Tessin.Tin.Models.Extensions
{
    public static class StringExtensions
    {
        public static int? ToNullableInteger(this string value)
        {
            try
            {
                return int.Parse(value);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static int ToInteger(this string value)
        {
            return int.Parse(value);
        }

    }
}
