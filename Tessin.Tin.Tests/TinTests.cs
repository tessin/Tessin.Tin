using NUnit.Framework;
using Tessin.Tin.Models;

namespace Tessin.Tin.Tests
{
    [TestFixture]
    public class TinTests
    {
        [TestCase("851116-1393", TinCountry.Sweden, ExpectedResult = true)]
        [TestCase("31046812355", TinCountry.Norway, ExpectedResult = false)]
        [TestCase("301232-670W", TinCountry.Finland, ExpectedResult = true)]
        [TestCase("010158-500J", TinCountry.Finland, ExpectedResult = true)]
        [TestCase("010990-295M", TinCountry.Finland, ExpectedResult = true)]
        [TestCase("200949-761B", TinCountry.Finland, ExpectedResult = true)]
        [TestCase("250639-549R", TinCountry.Finland, ExpectedResult = true)]
        [TestCase("291165-4883", TinCountry.Finland, ExpectedResult = true)]
        public static bool Validate_WithSuppliedValue_ReturnsTrueOrFalse(string value, TinCountry country)
        {
            return value.IsValid(country);
        }
    }
}
