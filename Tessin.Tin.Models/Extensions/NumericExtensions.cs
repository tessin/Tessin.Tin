namespace Tessin.Tin.Models.Extensions
{
    public static class NumericExtensions
    {
        public static string ToPaddedString(this int value, int n)
        {
            var number = value.ToString();
            var diff = n - number.Length;
            if (diff < 1) return number;
            return new string('0', diff) + number;
        }
    }
}
