using System;
using System.Linq;
using NUnit.Framework;
using Tessin.Tin.Models;
using Tessin.Tin.Sweden;

namespace Tessin.Tin.Tests
{

    [TestFixture]
    public class TinEvaluatorSeTests
    {

        [SetUp]
        public void SetUp()
        {
            TinGlobal.OverrideDate = new DateTime(2017, 2, 11);
        }


        [TestCase("20000000-0000", ExpectedResult = false)]
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
        [TestCase("829327-8992", ExpectedResult = false)]
        public bool EvaluateWithTypeEntity_WithSwedishEntityTin_ReturnsValidOrInvalid(string value)
        {
            var evaluator = new TinEvaluatorSe();
            var tin = evaluator.Evaluate(value, TinType.Entity);
            return tin.Status == TinStatus.Valid;
        }

        [TestCase("630318-0910", TinStatus.Valid, TinGender.Male, 53)]
        [TestCase("140323-4709", TinStatus.Valid, TinGender.Female, 2)]
        [TestCase("140323+4709", TinStatus.Valid, TinGender.Female, 102)]
        [TestCase("350828-2328", TinStatus.Valid, TinGender.Female, 81)]
        [TestCase("870709-7138", TinStatus.Valid, TinGender.Male, 29)]
        public void Evaluate_WithSwedishPersonTin_ReturnsCorrectAgeAndGender(string value, TinStatus status, TinGender gender, int age)
        {
            var evaluator = new TinEvaluatorSe();
            var tin = evaluator.Evaluate(value, TinType.Person);
            Assert.That(tin.Type == TinType.Person);
            Assert.That(tin.Gender == gender);
            Assert.That(tin.Age == age);
        }

        [TestCase("140323-4709", TinMessageCode.InfoAgeMinor)]
        [TestCase("140323+4709", TinMessageCode.InfoAgeSenior)]
        public void Evaluate_WithSwedishPersonTin_GeneratesCorrectMessageCodes(string value, params TinMessageCode[] errorCodes)
        {
            var evaluator = new TinEvaluatorSe();
            var tin = evaluator.Evaluate(value, TinType.Person);
            foreach (var code in errorCodes)
            {
                Assert.That(tin.Messages.Any(p => p.Code == code));
            }
        }


    }
}
