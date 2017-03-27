using NUnit.Framework;
using Tessin.Tin.Models;

namespace Tessin.Tin.Tests
{
    [TestFixture]
    public class TinTests
    {
        [TestCase("851116-1393", TinCountry.Sweden)]
        public static bool Validate_WithSuppliedValue_ReturnsTrueOrFalse(string value, TinCountry country)
        {
            return value.Validate(country);
        }
    }
}
