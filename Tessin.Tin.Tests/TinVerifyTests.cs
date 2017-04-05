using NUnit.Framework;
using Tessin.Tin.Models;

namespace Tessin.Tin.Tests
{
    [TestFixture]
    public class TinVerifyTests
    {
        [TestCase("d", TinCountry.Sweden, TinType.Person, ExpectedResult = false)]
        [TestCase("d", TinCountry.Sweden, TinType.Entity, ExpectedResult = false)]
        [TestCase("851116-1393", TinCountry.Sweden, TinType.Person, ExpectedResult = true)]
        [TestCase("31046812355", TinCountry.Norway, TinType.Person, ExpectedResult = false)]
        [TestCase("301232-670W", TinCountry.Finland, TinType.Person, ExpectedResult = true)]
        [TestCase("010158-500J", TinCountry.Finland, TinType.Person, ExpectedResult = true)]
        [TestCase("010990-295M", TinCountry.Finland, TinType.Person, ExpectedResult = true)]
        [TestCase("200949-761B", TinCountry.Finland, TinType.Person, ExpectedResult = true)]
        [TestCase("250639-549R", TinCountry.Finland, TinType.Person, ExpectedResult = true)]
        [TestCase("291165-4883", TinCountry.Finland, TinType.Person, ExpectedResult = true)]
        [TestCase("291165-4883", TinCountry.Unknown, TinType.Person, ExpectedResult = false)]
        [TestCase("291165-4883", TinCountry.Finland, TinType.Unknown, ExpectedResult = true)]
        public static bool Validate_WithSuppliedValue_ReturnsTrueOrFalse(string value, TinCountry country, TinType type)
        {
            return TinVerify.IsValid(value, country, type);
        }
    }
}
