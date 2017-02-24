using System;
using System.Globalization;
using System.Linq;
using Tessin.Tin.Models;

namespace Tessin.Tin.Extensions
{
    public static class TinCountryExtensions
    {
        public static RegionInfo ToRegionInfo(this TinCountry country)
        {
            var regions = CultureInfo.GetCultures(CultureTypes.SpecificCultures).Select(x => new RegionInfo(x.LCID));
            return regions.FirstOrDefault(region => region.EnglishName == nameof(country));
        }
    }
}
