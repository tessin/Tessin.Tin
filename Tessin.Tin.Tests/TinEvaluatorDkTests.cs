using System;
using System.Linq;
using NUnit.Framework;
using Tessin.Tin.Denmark;
using Tessin.Tin.Models;
using Tessin.Tin.Models.Extensions;

namespace Tessin.Tin.Tests
{
    [TestFixture]
    public class TinEvaluatorDkTests
    {
        [SetUp]
        public void SetUp()
        {
            TinGlobal.OverrideDate = new DateTime(2017, 2, 11);
        }

        [TestCase("300954-3235", TinStatus.Valid, TinGender.Male, 62)]
        [TestCase("031048-1297", TinStatus.Valid, TinGender.Male, 68)]
        [TestCase("300788-4981", TinStatus.Valid, TinGender.Male, 28)]
        [TestCase("230448-3426", TinStatus.Valid, TinGender.Female, 68)]
        [TestCase("281071-1232", TinStatus.Valid, TinGender.Female, 45)]
        [TestCase("121249-4018", TinStatus.Valid, TinGender.Female, 67)]
        public void Evaluate_WithDanishPersonTin_ReturnsCorrectAgeAndGender(string value, TinStatus status, TinGender gender, int age, params string[] errorCodes)
        {
            var evaluator = new TinEvaluatorDk();
            var tin = evaluator.Evaluate(value, TinType.Person);
            Assert.That(tin.Type == TinType.Person);
            Assert.That(tin.Gender == gender);
            Assert.That(tin.Age == age);
        }

        [TestCase("031036-8297", TinMessageCode.ErrorNegativeAge)]
        [TestCase("03sdfsdf1036-8297", TinMessageCode.ErrorNormalizationFailed)]
        [TestCase("03sdfsdf1036-8297", TinMessageCode.ErrorNormalizationFailed)]
        [TestCase("  ", TinMessageCode.ErrorValueIsNullOrWhitespace)]
        [TestCase("", TinMessageCode.ErrorValueIsNullOrWhitespace)]
        [TestCase(null, TinMessageCode.ErrorValueIsNullOrWhitespace)]
        public void Evaluate_WithDanishPersonTin_GeneratesCorrectErrorCodes(string value, params TinMessageCode[] errorCodes)
        {
            var evaluator = new TinEvaluatorDk();
            var tin = evaluator.Evaluate(value, TinType.Person);
            var errors = tin.Messages.Where(p => p.Type == TinMessageType.Error).ToArray();
            foreach (var code in errorCodes)
            {
                Assert.That(errors.Any(p => p.Code == code));
            }
        }

        [TestCase("300954-3235", TinMessageCode.InfoChecksumNotVerified)]
        [TestCase("031048-1297", TinMessageCode.InfoChecksumNotVerified, TinMessageCode.InfoAgeSenior)]
        public void Evaluate_WithDanishPersonTin_GeneratesCorrectInfoCodes(string value, params TinMessageCode[] infoCodes)
        {
            var evaluator = new TinEvaluatorDk();
            var tin = evaluator.Evaluate(value, TinType.Person);
            var messages = tin.Messages.Where(p => p.Type == TinMessageType.Information).ToArray();
            foreach (var code in infoCodes)
            {
                Assert.That(messages.Any(p => p.Code == code));
            }
        }

        [TestCase("16315877", ExpectedResult = true)]
        [TestCase("22756214", ExpectedResult = true)]
        [TestCase("10582989 ", ExpectedResult = true)]
        [TestCase("24256790", ExpectedResult = true)]
        [TestCase("28504799", ExpectedResult = true)]
        [TestCase("25313763", ExpectedResult = true)]
        [TestCase("36213728", ExpectedResult = true)]
        [TestCase("61056416", ExpectedResult = true)]
        [TestCase("10403782", ExpectedResult = true)]
        [TestCase("10403783", ExpectedResult = false)]
        [TestCase("DK16315877", ExpectedResult = true)]
        [TestCase("CVR22756214", ExpectedResult = true)]
        [TestCase("SE10582989 ", ExpectedResult = true)]
        [TestCase("DK24256790", ExpectedResult = true)]
        [TestCase("CVR28504799", ExpectedResult = true)]
        [TestCase("SE25313763", ExpectedResult = true)]
        [TestCase("36  21 3 728 ", ExpectedResult = true)]
        [TestCase("6 1 0 56416", ExpectedResult = true)]
        [TestCase("10 4 03782", ExpectedResult = true)]
        [TestCase("104 0 3783", ExpectedResult = false)]
        [TestCase("", ExpectedResult = false)]
        [TestCase("   ", ExpectedResult = false)]
        [TestCase(null, ExpectedResult = false)]
        public bool Evaluate_WithEntityTin_ReturnValidOrInvalid(string value)
        {
            var evaluator = new TinEvaluatorDk();
            var tin = evaluator.Evaluate(value, TinType.Entity);
            return tin.IsValid();
        }

    }
}
