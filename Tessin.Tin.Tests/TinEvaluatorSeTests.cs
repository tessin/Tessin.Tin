using System.Diagnostics;
using NUnit.Framework;
using Tessin.Tin.Models;
using Tessin.Tin.Sweden;

namespace Tessin.Tin.Tests
{
    [TestFixture]
    public class TinEvaluatorSeTests
    {
        [TestCase("630318-0910", ExpectedResult = true)]
        [TestCase("140323-4709", ExpectedResult = true)]
        [TestCase("350828-2328", ExpectedResult = true)]
        [TestCase("870709-7138", ExpectedResult = true)]
        [TestCase("870709K7138", ExpectedResult = false)]
        [TestCase("19630318K0910", ExpectedResult = false)]
        [TestCase("19630318-0910", ExpectedResult = true)]
        [TestCase("19140323-4709", ExpectedResult = true)]
        [TestCase("19350828-2328", ExpectedResult = true)]
        [TestCase("19870709-7138", ExpectedResult = true)]
        [TestCase("196303180910", ExpectedResult = true)]
        [TestCase("191403234709", ExpectedResult = true)]
        [TestCase("193508282328", ExpectedResult = true)]
        [TestCase("198707097138", ExpectedResult = true)]
        [TestCase("193608282328", ExpectedResult = false)]
        [TestCase("996735-7279", ExpectedResult = true)]
        [TestCase("9967357279", ExpectedResult = true)]
        [TestCase(" 996735-7279", ExpectedResult = true)]
        [TestCase("9967357279 ", ExpectedResult = true)]
        [TestCase("996535-7279", ExpectedResult = false)]
        [TestCase("9965357279", ExpectedResult = false)]
        [TestCase("224633-1173", ExpectedResult = true)]
        [TestCase("598400-8010", ExpectedResult = true)]
        [TestCase("739040-0898", ExpectedResult = true)]
        [TestCase("966611-7677", ExpectedResult = true)]
        [TestCase("827675-7104", ExpectedResult = true)]
        [TestCase("827675K7104", ExpectedResult = false)]
        [TestCase(null, ExpectedResult = false)]
        [TestCase("", ExpectedResult = false)]
        [TestCase("2342we4fkunshkeujrytgnuy3", ExpectedResult = false)]
        public bool Evaluate_WithSwedishPersonalTin_ReturnsValidOrInvalid(string value)
        {
            var evaluator = new TinEvaluatorSe();
            var tin = evaluator.Evaluate(value);
            return tin.Status == TinStatus.Valid;
        }

        [TestCase("350828-2328", ExpectedResult = false)]
        [TestCase("870709-7138", ExpectedResult = false)]
        [TestCase("19350828-2328", ExpectedResult = false)]
        [TestCase("19870709-7138", ExpectedResult = false)]
        [TestCase("196303180910", ExpectedResult = false)]
        [TestCase("191403234709", ExpectedResult = false)]
        [TestCase("996735-7279", ExpectedResult = true)]
        [TestCase("9967357279", ExpectedResult = true)]
        [TestCase(" 996735-7279", ExpectedResult = true)]
        [TestCase("9967357279 ", ExpectedResult = true)]
        [TestCase("996535-7279", ExpectedResult = false)]
        [TestCase("9965357279", ExpectedResult = false)]
        [TestCase("224633-1173", ExpectedResult = true)]
        [TestCase("598400-8010", ExpectedResult = true)]
        [TestCase("739040-0898", ExpectedResult = true)]
        [TestCase("966611-7677", ExpectedResult = true)]
        [TestCase("827675-7104", ExpectedResult = true)]
        [TestCase("SE827675710401", ExpectedResult = true)]
        [TestCase("SE598400801001", ExpectedResult = true)]
        [TestCase("827675K7104", ExpectedResult = false)]
        [TestCase(null, ExpectedResult = false)]
        [TestCase("", ExpectedResult = false)]
        [TestCase("2342we4fkunshkeujrytgnuy3", ExpectedResult = false)]

        public bool EvaluateWithTypeEntity_WithSwedishEntityTin_ReturnsValidOrInvalid(string value)
        {
            var evaluator = new TinEvaluatorSe();
            var tin = evaluator.Evaluate(value, TinType.Entity);
            return tin.Status == TinStatus.Valid;
        }

    }
}
