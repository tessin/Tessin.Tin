using NUnit.Framework;
using Tessin.Tin.Models;
using Tessin.Tin.Models.Extensions;
using Tessin.Tin.Norway;

namespace Tessin.Tin.Tests
{
    [TestFixture]
    public class TinEvaluatorNoTests
    {
        [TestCase("01129955131", ExpectedResult = true)]
        public bool Evaluate_WithPersonTin_ReturnValidOrInvalid(string value)
        {
            var evaluator = new TinEvaluatorNo();
            var tin = evaluator.Evaluate(value, TinType.Person);
            return tin.IsValid();
        }
    }
}
