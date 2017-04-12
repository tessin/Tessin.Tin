using System;
using System.Diagnostics;
using NUnit.Framework;
using Tessin.Tin.Models;
using Tessin.Tin.Models.Extensions;

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
        [TestCase("917254788", TinCountry.Norway, TinType.Entity, ExpectedResult = true)]// 
        public static bool IsValid_WithSuppliedValue_ReturnsTrueOrFalse(string value, TinCountry country, TinType type)
        {
            return TinVerify.IsValid(value, country, type);
        }

        [TestCase(" 3 00 9 5 4-32 3 5 ", TinCountry.Denmark, TinType.Person, ExpectedResult = "300954-3235")]
        [TestCase(" 0 3 1 048-1 2 97 ", TinCountry.Denmark, TinType.Person, ExpectedResult = "031048-1297")]
        [TestCase(" 0 3 1 0481 2 97 ", TinCountry.Denmark, TinType.Person, ExpectedResult = "031048-1297")]
        [TestCase(" 85 1 116-1 3 93 ", TinCountry.Sweden, TinType.Person,   ExpectedResult = "19851116-1393")]
        [TestCase(" 85 1 116 1 3 93 ", TinCountry.Sweden, TinType.Person, ExpectedResult = "19851116-1393")]
        [TestCase(" 31 04 6812 355 ", TinCountry.Norway, TinType.Person,   ExpectedResult = "31046812355")]
        [TestCase(" 31 04 6812 355 ", TinCountry.Norway, TinType.Person, ExpectedResult = "31046812355")]
        [TestCase(" 30123  2-670W ", TinCountry.Finland, TinType.Person,  ExpectedResult = "301232-670W")]
        [TestCase(" 01 0 15 8-5 00J ", TinCountry.Finland, TinType.Person,  ExpectedResult = "010158-500J")]
        [TestCase(" 0109  90-295M ", TinCountry.Finland, TinType.Person,  ExpectedResult = "010990-295M")]
        [TestCase(" 2 0 0 949-7 6 1B", TinCountry.Finland, TinType.Person,  ExpectedResult = "200949-761B")]
        [TestCase(" 2 5 0 639-5 4 9R ", TinCountry.Finland, TinType.Person,  ExpectedResult = "250639-549R")]
        [TestCase(" 2 9 1 1 6 5-4 8 83 ", TinCountry.Finland, TinType.Person,  ExpectedResult = "291165-4883")]
        [TestCase(" 2 9 1 1 6 5 - 4 8 83 ", TinCountry.Unknown, TinType.Person,  ExpectedResult = null)]
        [TestCase(" 2 9 1 1 6 5 - 48 8 3 ", TinCountry.Finland, TinType.Unknown, ExpectedResult = "291165-4883")]
        public static string Validate_WithSuppliedValue_ReturnsNormalizedValue(string value, TinCountry country, TinType type)
        {
            return TinVerify.Validate(value, country, type).NormalizedValue;
        }

        [TestCase("851116-1393", TinCountry.Sweden, TinType.Person)]
        [TestCase("598400-8010", TinCountry.Sweden, TinType.Entity)]
        [TestCase("917254788", TinCountry.Norway, TinType.Entity)]
        [TestCase("27046828122", TinCountry.Norway, TinType.Person)]
        [TestCase("291165-4883", TinCountry.Finland, TinType.Person)]
        [TestCase("0109862-8", TinCountry.Finland, TinType.Entity)]
        [TestCase("300788-4981", TinCountry.Denmark, TinType.Person)]
        [TestCase("36213728", TinCountry.Denmark, TinType.Entity)]
        public static void BothOverloadsOfValidate_WithValidTin_ReturnsEquivalentResult(string value, TinCountry country, TinType type)
        {
            var result1 = TinVerify.Validate(value, country, type);

            var result2 = TinVerify.Validate(value, country);

            Assert.That(result1.Status, Is.EqualTo(result2.Status));
            Assert.That(result1.Type, Is.EqualTo(result2.Type));
            Assert.That(result1.Country, Is.EqualTo(result2.Country));
            Assert.That(result1.Gender, Is.EqualTo(result2.Gender));
            Assert.That(result1.NormalizedValue, Is.EqualTo(result2.NormalizedValue));

        }

        [TestCase("851116-1397", TinCountry.Sweden, TinType.Person)]
        [TestCase("598400-8015", TinCountry.Sweden, TinType.Entity)]
        [TestCase("917254784", TinCountry.Norway, TinType.Entity)]
        [TestCase("27046828126", TinCountry.Norway, TinType.Person)]
        [TestCase("291165-4888", TinCountry.Finland, TinType.Person)]
        [TestCase("0109862-5", TinCountry.Finland, TinType.Entity)]
        [TestCase("330788-4982", TinCountry.Denmark, TinType.Person)]
        [TestCase("36213724", TinCountry.Denmark, TinType.Entity)]
        public static void BothOverloadsOfValidate_WithInvalidTin_BehaveAsExpected(string value, TinCountry country, TinType type)
        {
            var result1 = TinVerify.Validate(value, country, type);

            var result2 = TinVerify.Validate(value, country);

            Assert.That(result1.Type, Is.EqualTo(type));
            Assert.That(result2.Type, Is.EqualTo(TinType.Unknown));
            Assert.That(result1.Status, Is.EqualTo(result2.Status));
            
        }


        [Test]
        public static void Validate_WithRandomCountryTypeAndInput_ShouldNotThrowException()
        {
            var strings = Utils.GetRandomStrings(200000, 64);
            var watch = new Stopwatch();
            watch.Start();
            foreach (var s in strings)
            {
                var country = Utils.RandomEnumValue<TinCountry>();
                var type = Utils.RandomEnumValue<TinType>();
                TinVerify.Validate(s, country, type);
            }
            watch.Stop();
            Debug.WriteLine($"Elapsed: {watch.ElapsedMilliseconds}");
        }
        
    }
}
