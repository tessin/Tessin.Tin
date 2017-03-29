using Tessin.Tin.Models;
using Tessin.Tin.Models.Extensions;

namespace Tessin.Tin
{
    public static class TinVerify
    {
        public static bool IsValid(string value, TinCountry country, TinType type = TinType.Unknown)
        {
            var evaluator = TinEvaluatorFactory.Default.Create(country);
            if (type == TinType.Unknown)
            {
                var result = evaluator.Evaluate(value);
                return result.IsValid();
            }
            else
            {
                var result = evaluator.Evaluate(value, type);
                return result.IsValid();
            }
        }
        public static TinResponse Validate(string value, TinCountry country, TinType type = TinType.Unknown)
        {
            var evaluator = TinEvaluatorFactory.Default.Create(country);
            return type == TinType.Unknown ? evaluator.Evaluate(value) : evaluator.Evaluate(value, type);
        }

        public static bool IsEntity(string value, TinCountry country)
        {
            var evaluator = TinEvaluatorFactory.Default.Create(country);
            var result = evaluator.Evaluate(value);
            return result.Type == TinType.Entity;
        }

        public static bool IsPerson(string value, TinCountry country)
        {
            var evaluator = TinEvaluatorFactory.Default.Create(country);
            var result = evaluator.Evaluate(value);
            return result.Type == TinType.Person;
        }

    }
}
