using System;

using System.Linq;
using System.Text.RegularExpressions;
using Tessin.Tin.Extensions;
using Tessin.Tin.Models;
using Tessin.Tin.Models.Extensions;

namespace Tessin.Tin.Norway
{
    public class TinEvaluatorNo : TinEvaluator
    {
        private static readonly Func<int, int> ModuloRule = (p) => p == 10 ? -1 : (p == 11 ? 0 : p);

        private static readonly Regex NorwegianPersonTinRegex = new Regex("^[0-9]{11}$");

        private static readonly Regex NorwegianEntityTinRegex = new Regex("^[0-9]{9}$");

        public override TinCountry Country => TinCountry.Norway;

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
            response.Value = value;
            response.Type = type;
            response.Country = Country;

            if (string.IsNullOrWhiteSpace(value))
            {
                return response.AddError(TinMessageCode.ErrorValueIsNullOrWhitespace);
            }

            if (type == TinType.Person)
            {
                var normalized = NormalizePerson(value);
                if (normalized == null) return response.AddError(TinMessageCode.ErrorNormalizationFailed);

                response.NormalizedValue = normalized;

                if (!NorwegianPersonTinRegex.IsMatch(normalized))
                {
                    return response.AddError(TinMessageCode.ErrorFormatMismatchPerson);
                }
                var parts = new TinParts(TinDateFormat.DayMonthYear, TinYearFormat.Short);
                parts.Serial = normalized.Substring(6, 3);
                parts.Checksum = normalized.Substring(9, 2);
                parts.SetDateInfo(normalized.Substring(0,6), TinDateFormat.DayMonthYear);
                parts.Century = GetCentury(response, parts.YearNumeric, parts.SerialNumeric);
                response.Gender = GetGender(parts);
                response.Date = parts.GetDate();

                var calculated = GetPersonChecksum(parts);
                if (parts.Checksum != calculated)
                {
                    response.AddError(TinMessageCode.ErrorInvalidChecksum);
                }

                if (response.Date == null)
                {
                    response.AddError(TinMessageCode.ErrorInvalidDate);
                }
                else
                {
                    response.HandleAge();
                }
                
                response.Status = response.Messages.Any(p => p.Type == TinMessageType.Error)
                    ? TinStatus.Invalid
                    : TinStatus.Valid;

            }
            else if (type == TinType.Entity)
            {
                var normalized = NormalizeEntity(value);

                if (normalized == null)
                    return response.AddError(TinMessageCode.ErrorNormalizationFailed);
                if (!NorwegianEntityTinRegex.IsMatch(normalized))
                {
                    return response.AddError(TinMessageCode.ErrorFormatMismatchEntity);
                }

                response.NormalizedValue = normalized;

                var parts = new TinParts();
                parts.Serial = normalized.Substring(0, 8);
                parts.Checksum = normalized.Substring(8, 1);
                var calculated = CalculateEntityChecksum(parts);
                if (parts.Checksum != calculated)
                {
                    return response.AddError(TinMessageCode.ErrorInvalidChecksum);
                }

                response.Status = TinStatus.Valid;
            }
            else
            {
                return response.AddError(TinMessageCode.ErrorInvalidType);
            }

            return response;
        }

        private static string NormalizePerson(string value)
        {
            value = value.RemoveAllWhitespace().ToUpper();
            return value.Length != 11 ? null : value;
        }

        private static string NormalizeEntity(string value)
        {
            value = value.RemoveAllWhitespace().ToUpper().Replace("MVA","");
            return value.Length == 9 ? value : null;
        }

        private static TinGender GetGender(TinParts parts)
        {
            try
            {
                return parts.SerialNumeric % 2 == 0 ? TinGender.Female : TinGender.Male;
            }
            catch (Exception)
            {
                return TinGender.Unknown;
            }
        }

        private static string CalculateEntityChecksum(TinParts parts)
        {
            var number = parts.ToStringWithoutChecksum();
            var chk = number.CalculateMod11Checksum(new [] { 3, 2, 7, 6, 5, 4, 3, 2 }, ModuloRule);
            return chk.ToString();
        }

        private static string GetPersonChecksum(TinParts parts)
        {
            var number = parts.ToStringWithoutChecksum();
            return GetPersonChecksum(number);
            //var chk1 = number.CalculateMod11Checksum(new [] { 3, 7, 6, 1, 8, 9, 4, 5, 2 }, ModuloRule);
            //number = number + chk1;
            //var chk2 = number.CalculateMod11Checksum(new [] { 5, 4, 3, 2, 7, 6, 5, 4, 3, 2 }, ModuloRule);
            //return $"{chk1}{chk2}";
        }

        public static string GetPersonChecksum(string value)
        {
            var chk1 = value.CalculateMod11Checksum(new[] { 3, 7, 6, 1, 8, 9, 4, 5, 2 }, ModuloRule);

            if (!chk1.Between(0, 9)) return null;

            value += chk1.ToString();
            var chk2 = value.CalculateMod11Checksum(new[] { 5, 4, 3, 2, 7, 6, 5, 4, 3, 2 }, ModuloRule);

            if (!chk2.Between(0, 9)) return null;

            return $"{chk1}{chk2}";
        }

        public static int GetCentury(TinResponse response, int? year, int serial)
        {
            if (!year.HasValue) throw new ArgumentException("No year of birth.");
            if (serial >= 500 && serial < 750)
            {
                response.AddInfo(TinMessageCode.InfoAmbiguousCentury);
            }
            return serial < 500 ? 1900 : (year < 40) ? 2000 : 1800;
        }

    }
}
