using System.Globalization;
using System.Linq;
using Tessin.Tin.Extensions;
using Tessin.Tin.Models;

namespace Tessin.Tin.Sweden
{
    public class TinEvaluatorSe: ITinEvaluator
    {
        public string Country => nameof(TinCountry.Sweden);

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
            var tin = new TinResponse();
            tin.Country = Country;
            tin.Value = value;
            tin.Type = type;

            if (string.IsNullOrWhiteSpace(value))
            {
                return tin.AddError(TinMessageCode.ErrorValueIsNullOrWhitespace);
            }

            return EvaluateInternal(tin, value, type);
        }

        public TinResponse EvaluateInternal(TinResponse tin, string value, TinType type)
        {
            if (type != TinType.Entity && type != TinType.Person) return TinResponse.ForError(TinMessageCode.ErrorInvalidType);
            var normalized = type == TinType.Entity ? ValidateSe.NormalizeOnr(value) : ValidateSe.NormalizePnr(value);
            if (normalized == null) return TinResponse.ForError(TinMessageCode.ErrorNormalizationFailed);
            tin.NormalizedValue = normalized;

            //bool valid;
            if (type == TinType.Entity)
            {
                var valid = ValidateSe.ValidateOnr(normalized);
                if (!valid) tin.AddError(TinMessageCode.ErrorInvalidChecksum);
            }
            else
            {
                var valid = ValidateSe.ValidatePnrLong(normalized);

                if (!valid) tin.AddError(TinMessageCode.ErrorInvalidChecksum);

                tin.Date = ValidateSe.GetDate(normalized, Culture);

                if (tin.Date == null) tin.AddError(TinMessageCode.ErrorInvalidDate);

                tin.Gender = ValidateSe.GetNormalizedPnrGender(normalized);
                
                tin.Age = tin.Date.ToAge();

                if (tin.Age < 0) tin.AddError(TinMessageCode.ErrorNegativeAge);
                if (tin.Age < 18) tin.AddInfo(TinMessageCode.InfoAgeMinor);
                if (tin.Age >= 65) tin.AddInfo(TinMessageCode.InfoAgeSenior);
                if (tin.Age > 105) tin.AddInfo(TinMessageCode.InfoAgeExcessive);
                if (tin.Age > 150) tin.AddError(TinMessageCode.ErrorAgeLimit);

            }
            tin.Status = tin.Messages.Any(p => p.Type == TinMessageType.Error) ? TinStatus.Invalid : TinStatus.Valid;
            return tin;
        }
    }
}
