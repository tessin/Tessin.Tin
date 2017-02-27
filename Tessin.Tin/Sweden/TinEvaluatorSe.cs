using System.Globalization;
using System.Linq;
using Tessin.Tin.Extensions;
using Tessin.Tin.Models;

namespace Tessin.Tin.Sweden
{
    public class TinEvaluatorSe: ITinEvaluator
    {
        public TinCountry Country => TinCountry.Sweden;

        private CultureInfo Culture { get; }

        public TinEvaluatorSe()
        {
            Culture = new CultureInfo("sv-SE");
        }

        public TinResponse Evaluate(string value)
        {
            var result = Evaluate(value, TinType.Person);
            var temp = result;
            if (result.Status != TinStatus.Invalid) return result;
            result = Evaluate(value, TinType.Entity);
            return result.Status == TinStatus.Invalid ? temp : result;
        }

        public TinResponse Evaluate(string value, TinType type)
        {
            var response = new TinResponse();
            response.Country = Country;
            response.Value = value;
            response.Type = type;

            if (string.IsNullOrWhiteSpace(value))
            {
                return response.AddError(TinMessageCode.ErrorValueIsNullOrWhitespace);
            }

            return EvaluateInternal(response, value, type);
        }

        public TinResponse EvaluateInternal(TinResponse response, string value, TinType type)
        {
            if (type != TinType.Entity && type != TinType.Person) return TinResponse.ForError(TinMessageCode.ErrorInvalidType, value, Country);

            var normalized = type == TinType.Entity ? ValidateSe.NormalizeOnr(value) : ValidateSe.NormalizePnr(value);
            if (normalized == null) return TinResponse.ForError(TinMessageCode.ErrorNormalizationFailed, value, TinCountry.Sweden);
            response.NormalizedValue = normalized;

            if (type == TinType.Entity)
            {
                var valid = ValidateSe.ValidateOnr(normalized);
                if (!valid) response.AddError(TinMessageCode.ErrorInvalidChecksum);
            }
            else
            {
                var valid = ValidateSe.ValidatePnrLong(normalized);

                if (!valid) response.AddError(TinMessageCode.ErrorInvalidChecksum);

                response.Date = ValidateSe.GetDate(normalized, Culture);

                if (response.Date == null) response.AddError(TinMessageCode.ErrorInvalidDate);

                response.Gender = ValidateSe.GetNormalizedPnrGender(normalized);
                
                response.Age = response.Date.ToAge();

                if (response.Age < 0) response.AddError(TinMessageCode.ErrorNegativeAge);
                if (response.Age < 18) response.AddInfo(TinMessageCode.InfoAgeMinor);
                if (response.Age > 150)
                {
                    response.AddError(TinMessageCode.ErrorAgeLimit);
                }
                else
                {
                    if (response.Age >= 65) response.AddInfo(TinMessageCode.InfoAgeSenior);
                    if (response.Age > 105) response.AddInfo(TinMessageCode.InfoAgeExcessive);
                }
            }
            response.Status = response.Messages.Any(p => p.Type == TinMessageType.Error) ? TinStatus.Invalid : TinStatus.Valid;
            return response;
        }
    }
}
