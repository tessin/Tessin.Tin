using System;

namespace Tessin.Tin.Models.Extensions
{
    public static class EnumExtensions
    {
        public static string GetText(this Enum value)
        {
            return Enum.GetName(value.GetType(), value);
        }
    }
}
