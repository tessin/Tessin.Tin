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

        public static TinCountry ToTinCountry(this string value)
        {
            var country = value.Trim().ToLower();
            switch (country)
            {
                case "se":
                case "swe":
                case "sverige":
                case "sweden": return TinCountry.Sweden;
                case "no":
                case "nor":
                case "norge":
                case "norway": return TinCountry.Norway;
                case "fi":
                case "fin":
                case "finland": return TinCountry.Finland;
                case "dk":
                case "dnk":
                case "danmark":
                case "denmark": return TinCountry.Denmark;
                default: return TinCountry.Unknown;
            }
        }

    }
}
