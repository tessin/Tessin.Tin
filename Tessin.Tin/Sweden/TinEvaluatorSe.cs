﻿using System.Globalization;
using System.Linq;
using Tessin.Tin.Extensions;
using Tessin.Tin.Models;
using Tessin.Tin.Models.Extensions;

namespace Tessin.Tin.Sweden
{
    public class TinEvaluatorSe: TinEvaluator
    {
        public override TinCountry Country => TinCountry.Sweden;

        private CultureInfo Culture { get; }

        public TinEvaluatorSe()
        {
            Culture = new CultureInfo("sv-SE");
        }

        public override TinResponse Evaluate(string value)
        {
            var result = Evaluate(value, TinType.Person);
            if (result.Status != TinStatus.Invalid) return result;
            result = Evaluate(value, TinType.Entity);
            result.AddInfo(TinMessageCode.InfoAttemptedMatchForPerson);
            if (!result.IsInvalid()) return result;
            result.AddError(TinMessageCode.ErrorFailedAttemptedTypeMatch);
            result.Type = TinType.Unknown;
            return result;
        }

        public override TinResponse Evaluate(string value, TinType type)
        {
            var response = new TinResponse();
            response.Country = Country;
            response.Value = value;
            response.Type = type;

            return string.IsNullOrWhiteSpace(value) ? 
                response.AddError(TinMessageCode.ErrorValueIsNullOrWhitespace) : 
                EvaluateInternal(response, value, type);
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
                response.HandleAge();
            }
            response.Status = response.Messages.Any(p => p.Type == TinMessageType.Error) ? TinStatus.Invalid : TinStatus.Valid;
            return response;
        }
    }
}
