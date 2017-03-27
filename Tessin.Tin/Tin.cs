using System;
using Tessin.Tin.Models;
using Tessin.Tin.Models.Extensions;

namespace Tessin.Tin
{
    public static class Tin
    {
        public static bool Validate(this string value, TinCountry country, TinType type = TinType.Unknown)
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
    }
}
