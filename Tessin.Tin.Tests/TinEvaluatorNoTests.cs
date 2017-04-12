using System;
using System.Diagnostics;
using NUnit.Framework;
using Tessin.Tin.Models;
using Tessin.Tin.Models.Extensions;
using Tessin.Tin.Norway;

namespace Tessin.Tin.Tests
{
    [TestFixture]
    public class TinEvaluatorNoTests
    {

        [SetUp]
        public void SetUp()
        {
            TinGlobal.OverrideDate = new DateTime(2017, 2, 11);
        }

        [TestCase("31046812355", ExpectedResult = false)]
        [TestCase("27046891002", ExpectedResult = true)]
        [TestCase("27046828122", ExpectedResult = true)]
        [TestCase("d", ExpectedResult = false)]
        public bool Evaluate_WithPersonTin_ReturnValidOrInvalid(string value)
        {
            var evaluator = new TinEvaluatorNo();
            var tin = evaluator.Evaluate(value, TinType.Person);
            return tin.IsValid();
        }

        [TestCase("923609016", ExpectedResult = true)]
        [TestCase("982463718", ExpectedResult = true)]
        public bool Evaluate_WithEntityTin_ReturnValidOrInvalid(string value)
        {
            var evaluator = new TinEvaluatorNo();
            var tin = evaluator.Evaluate(value, TinType.Entity);
            return tin.IsValid();
        }

        [Test, Ignore]
        public void Generate()
        {
            var r = new Random(3546);
            var dates = new [] { "270468", "010472" };
            foreach (var date in dates)
            {
                for (var i = 0; i < 1000; i++)
                {
                    var serial = r.Next(1000).ToString("000");
                    var number = date + serial;
                    var chk = TinEvaluatorNo.GetPersonChecksum(number);
                    if (chk != null) Debug.WriteLine($"{number}{chk}");
                }
            }
        }


    }
}
