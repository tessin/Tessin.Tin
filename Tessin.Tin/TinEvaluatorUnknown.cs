using Tessin.Tin.Models;
using Tessin.Tin.Models.Extensions;

namespace Tessin.Tin
{
    public class TinEvaluatorUnknown: TinEvaluator
    {
        public override TinCountry Country { get; }

        public TinEvaluatorUnknown()
        {
            Country = TinCountry.Unknown;
        }

        public override TinResponse Evaluate(string value)
        {
            var response = new TinResponse
            {
                Value = value,
                Country = Country,
                Type = TinType.Unknown,
                Status = TinStatus.Uncertain
            };
            response.Messages.Add(TinMessageCode.ErrorUnknownCountry.ToErrorMessage());
            return response;
        }

        public override TinResponse Evaluate(string value, TinType type)
        {
            var response = new TinResponse
            {
                Value = value,
                Country = Country,
                Type = type,
                Status = TinStatus.Uncertain
            };
            response.Messages.Add(TinMessageCode.ErrorUnknownCountry.ToErrorMessage());
            return response;
        }
    }
}
