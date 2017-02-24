using Tessin.Tin.Models;

namespace Tessin.Tin
{
    public interface ITinEvaluator
    {

        TinCountry Country { get; }

        TinResponse Evaluate(string value);

        TinResponse Evaluate(string value, TinType type);

    }
}
