using System;
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
        [TestCase("04017531749", ExpectedResult = true)]
        [TestCase("10017924155", ExpectedResult = true)]
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


    }
}
