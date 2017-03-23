using System;
using Tessin.Tin.Models;

namespace Tessin.Tin
{
    public abstract class TinEvaluator: ITinEvaluator
    {
        public abstract TinCountry Country { get; }

        public abstract TinResponse Evaluate(string value);

        public abstract TinResponse Evaluate(string value, TinType type);

        protected void Validate(TinParts parts, Func<string, bool> checksum = null)
        {
            
        }

    }
}
