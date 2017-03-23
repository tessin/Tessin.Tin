using System;
using System.Diagnostics;
using System.Text;
using NUnit.Framework;
using Tessin.Tin.Finland;
using Tessin.Tin.Models;

namespace Tessin.Tin.Tests
{
    [TestFixture]
    public class TinEvaluatorFiTests
    {

        [SetUp]
        public void SetUp()
        {
            TinGlobal.OverrideDate = new DateTime(2017, 2, 11);
        }


        [TestCase("0112038-9", ExpectedResult = true)]
        [TestCase("2655044-9", ExpectedResult = true)]
        [TestCase("2255664-0", ExpectedResult = true)]
        [TestCase("1863026-2", ExpectedResult = true)]
        [TestCase("1886874-8", ExpectedResult = true)]
        [TestCase("1852302-9", ExpectedResult = true)]
        [TestCase("1041090-0", ExpectedResult = true)]
        [TestCase("0109862-8", ExpectedResult = true)]
        [TestCase("1927400-1", ExpectedResult = true)]
        [TestCase("0215254-2", ExpectedResult = true)]
        [TestCase("0116323-1", ExpectedResult = true)]
        [TestCase("0142213-3", ExpectedResult = true)]
        [TestCase("0128631-1", ExpectedResult = true)]
        [TestCase("FI01286311", ExpectedResult = true)]
        [TestCase("0128631-2", ExpectedResult = false)]
        [TestCase("01120389", ExpectedResult = true)]
        [TestCase(" 01120389 ", ExpectedResult = true)]
        [TestCase("0128631K1", ExpectedResult = false)]
        public bool Evaluate_WithFinishEntityTin_ReturnsValidOrInvalid(string value)
        {
            var evaluator = new TinEvaluatorFi();
            var tin = evaluator.Evaluate(value, TinType.Entity);
            return tin.Status == TinStatus.Valid;
        }

        [TestCase("131052-308T", ExpectedResult = true)]
        [TestCase("131052-308U", ExpectedResult = false)]
        [TestCase(" 131052-308T", ExpectedResult = true)]
        [TestCase("131052-308T ", ExpectedResult = true)]
        [TestCase(" 1 3 1 0 5 2 - 3 0 8 T ", ExpectedResult = true)]
        [TestCase("301232-670W", ExpectedResult = true)]
        [TestCase("010158-500J", ExpectedResult = true)]
        [TestCase("010990-295M", ExpectedResult = true)]
        [TestCase("200949-761B", ExpectedResult = true)]
        [TestCase("250639-549R", ExpectedResult = true)]
        [TestCase("291165-4883", ExpectedResult = true)]
        [TestCase("080193-538A", ExpectedResult = true)]
        [TestCase("060260-668H", ExpectedResult = true)]
        [TestCase("191179-2386", ExpectedResult = true)]
        [TestCase("030894-561R", ExpectedResult = true)]
        [TestCase("030800A561F", ExpectedResult = true)]
        [TestCase("030897+561H", ExpectedResult = true)]
        [TestCase("", ExpectedResult = false)]
        [TestCase("  ", ExpectedResult = false)]
        [TestCase(" sdlfjahjsdkfjhasdf ", ExpectedResult = false)]
        public bool Evaluate_WithFinishPersonTin_ReturnsValidOrInvalid(string value)
        {
            var evaluator = new TinEvaluatorFi();
            var tin = evaluator.Evaluate(value, TinType.Person);
            return tin.Status == TinStatus.Valid;
        }

        [TestCase("131052-308T", TinStatus.Valid, TinGender.Female, 64)]
        [TestCase("131052-308U", TinStatus.Invalid, TinGender.Female, 64)]
        [TestCase("301232-670W", TinStatus.Valid, TinGender.Female, 84)]
        [TestCase("010158-500J", TinStatus.Valid, TinGender.Female, 59)]
        [TestCase("030894-561R", TinStatus.Valid, TinGender.Male, 22)]
        [TestCase("301232+670W", TinStatus.Valid, TinGender.Female, 184)]
        [TestCase("301232A670W", TinStatus.Invalid, TinGender.Female, -16)]
        [TestCase("301200A670Y", TinStatus.Valid, TinGender.Female, 16)]
        public void Evaluate_WithFinishPersonTin_ReturnsCorrectAgeAndGender(string value, TinStatus status, TinGender gender, int age)
        {
            var evaluator = new TinEvaluatorFi();
            var tin = evaluator.Evaluate(value, TinType.Person);
            Assert.That(tin.Type == TinType.Person);
            Assert.That(tin.Gender == gender);
            Assert.That(tin.Age == age);
        }

        [TestCase("0112038-9", ExpectedResult = true)]
        [TestCase("2655044-9", ExpectedResult = true)]
        [TestCase("2255664-0", ExpectedResult = true)]
        [TestCase("1863026-2", ExpectedResult = true)]
        [TestCase("1886874-8", ExpectedResult = true)]
        [TestCase("1852302-9", ExpectedResult = true)]
        [TestCase("1041090-0", ExpectedResult = true)]
        [TestCase("0109862-8", ExpectedResult = true)]
        [TestCase("1927400-1", ExpectedResult = true)]
        [TestCase("0215254-2", ExpectedResult = true)]
        [TestCase("0116323-1", ExpectedResult = true)]
        [TestCase("0142213-3", ExpectedResult = true)]
        [TestCase("0128631-1", ExpectedResult = true)]
        [TestCase("FI01286311", ExpectedResult = true)]
        [TestCase("0128631-2", ExpectedResult = false)]
        [TestCase("01120389", ExpectedResult = true)]
        [TestCase(" 01120389 ", ExpectedResult = true)]
        [TestCase("0128631K1", ExpectedResult = false)]
        [TestCase("131052-308T", ExpectedResult = true)]
        [TestCase("131052-308U", ExpectedResult = false)]
        [TestCase(" 131052-308T", ExpectedResult = true)]
        [TestCase("131052-308T ", ExpectedResult = true)]
        [TestCase(" 1 3 1 0 5 2 - 3 0 8 T ", ExpectedResult = true)]
        [TestCase("301232-670W", ExpectedResult = true)]
        [TestCase("010158-500J", ExpectedResult = true)]
        [TestCase("010990-295M", ExpectedResult = true)]
        [TestCase("200949-761B", ExpectedResult = true)]
        [TestCase("250639-549R", ExpectedResult = true)]
        [TestCase("291165-4883", ExpectedResult = true)]
        [TestCase("080193-538A", ExpectedResult = true)]
        [TestCase("060260-668H", ExpectedResult = true)]
        [TestCase("191179-2386", ExpectedResult = true)]
        [TestCase("030894-561R", ExpectedResult = true)]
        [TestCase("030800A561F", ExpectedResult = true)]
        [TestCase("030897+561H", ExpectedResult = true)]
        [TestCase(null, ExpectedResult = false)]
        [TestCase("", ExpectedResult = false)]
        [TestCase("  ", ExpectedResult = false)]
        [TestCase(" sdlfjahjsdkfjhasdf ", ExpectedResult = false)]
        public bool Evaluate_WithFinishTinOfUnspecifiedType_ReturnsValidOrInvalid(string value)
        {
            var evaluator = new TinEvaluatorFi();
            var tin = evaluator.Evaluate(value);
            return tin.Status == TinStatus.Valid;
        }


        [Test, Ignore]
        public void CalculateModulus11_2_WithRandomDigits_NeverReturnsTwoDigits()
        {
            var r = new Random(1234);
            for (var i = 0; i < 10000; i++)
            {
                var len = (i%20 + 1);
                var digits = GetRandomDigitString(r, len);
                var check = TinEvaluatorFi.CalculateModulus11_2(digits);
                Debug.WriteLine($"{digits}-{check}");
                Assert.That(check.Length, Is.EqualTo(1));
            }
        }

        private static string GetRandomDigitString(Random r, int n)
        {
            var sb = new StringBuilder();
            for (var i = 0; i < n; i++)
            {
                var d = r.Next(10);
                sb.Append(d);
            }
            return sb.ToString();
        }
    }
}
