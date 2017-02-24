using NUnit.Framework;
using Tessin.Tin.Sweden;

namespace Tessin.Tin.Tests
{
    [TestFixture]
    public class SeValidateTests
    {

        [TestCase("811022-8312", ExpectedResult = "19811022-8312")]
        [TestCase("8110228311", ExpectedResult = "19811022-8311")]
        [TestCase("810231-8279", ExpectedResult = "19810231-8279")]
        [TestCase("810231+8279", ExpectedResult = "18810231+8279")]
        [TestCase("810231-8279", ExpectedResult = "19810231-8279")]
        [TestCase("198102318279", ExpectedResult = "19810231-8279")] // Inserts dash
        [TestCase("20810231+8279", ExpectedResult = "20810231-8279")] // Plus is ignored in long form pnr's and replaced with dash.
        [TestCase("8102318279", ExpectedResult = "19810231-8279")] // Add century and insert dash.
        [TestCase("19810231-8279", ExpectedResult = "19810231-8279")]
        public string NormalizePnr(string pnr)
        {
            return ValidateSe.NormalizePnr(pnr);
        }

        [TestCase("811022-8312", ExpectedResult = true)] // Luhn valid
        [TestCase("811022-8311", ExpectedResult = false)] // Luhn invalid
        [TestCase("810231-8279", ExpectedResult = false)] // Invalid date
        [TestCase("810231+8279", ExpectedResult = false)] // 100+, Invalid date.
        public bool ValidatePnrCanonical(string pnr)
        {
            return ValidateSe.ValidatePnrCanonical(pnr);
        }

        [TestCase("198110228312", ExpectedResult = true)] // Luhn valid
        [TestCase("198110228311", ExpectedResult = false)] // Luhn invalid
        [TestCase("198102318279", ExpectedResult = false)] // Invalid date
        public bool ValidatePnr(string pnr)
        {
            return ValidateSe.ValidatePnrLong(pnr);
        }

        [TestCase("7072474914", ExpectedResult = true)] // Luhn valid
        [TestCase("7072474917", ExpectedResult = false)] // Luhn invalid
        [TestCase("8102318279", ExpectedResult = false)] // Invalid date
        [TestCase("5565573895", ExpectedResult = true)] // Valid Onr
        [TestCase("5565573896", ExpectedResult = false)] // Onr, invalid checksum.
        public bool ValidateOnr(string onr)
        {
            return ValidateSe.ValidateOnr(onr);
        }

        [TestCase("20110231", ExpectedResult = false)]
        [TestCase("20110228", ExpectedResult = true)]
        public bool IsValidDate(string dateString)
        {
            return ValidateSe.IsValidDate(dateString);
        }

        [TestCase(78, false, ExpectedResult = 19)]
        [TestCase(78, true, ExpectedResult = 18)]
        [TestCase(11, false, ExpectedResult = 20)] // This could be a centenarian!
        public int ReturnsCorrectLikelyCentury(int year, bool plus100)
        {
            return ValidateSe.GetLikelyCentury(year, plus100);
        }

    }
}
