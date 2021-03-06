﻿using System;
using System.Linq;
using System.Text.RegularExpressions;
using Tessin.Tin.Extensions;
using Tessin.Tin.Models;
using Tessin.Tin.Models.Extensions;

namespace Tessin.Tin.Denmark
{
    public class TinEvaluatorDk: TinEvaluator
    {

        private static readonly Regex DanishPersonTinRegex = new Regex("^[0-9]{6}-[0-9]{4}$");

        private static readonly Regex DanishEntityTinRegex = new Regex("^[0-9]{8}$");
        
        public override TinCountry Country => TinCountry.Denmark;

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

                if (!DanishPersonTinRegex.IsMatch(normalized))
                {
                    return response.AddError(TinMessageCode.ErrorFormatMismatchPerson);
                }

                response.Gender = GetGender(normalized);
                response.Date = GetDate(normalized);
                response.HandleAge();;
                
                response.AddInfo(TinMessageCode.InfoChecksumNotVerified);

                response.Status = response.Messages.Any(p => p.Type == TinMessageType.Error)
                    ? TinStatus.Invalid
                    : TinStatus.Valid;

            }
            else if (type == TinType.Entity)
            {
                var normalized = NormalizeEntity(value);
                if (normalized == null)
                    return response.AddError(TinMessageCode.ErrorNormalizationFailed);

                response.NormalizedValue = normalized;

                if (!DanishEntityTinRegex.IsMatch(normalized))
                {
                    return response.AddError(TinMessageCode.ErrorFormatMismatchEntity);
                }
                if (!IsValidCvr(normalized))
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
            if (value.Length < 10) return null;
            var dash = value[6];
            if (dash != '-') value = value.Insert(6, "-");
            return value.Length != 11 ? null : value;
        }

        private static string NormalizeEntity(string value)
        {
            value = value.RemoveAllWhitespace().ToUpper();
            if (value.StartsWith("DK"))
                value = value.Remove(0, 2);
            if (value.StartsWith("CVR"))
                value = value.Remove(0, 3);
            if (value.StartsWith("SE"))
                value = value.Remove(0, 2);
            return value.Length == 8 ? value : null;
        }

        private static TinGender GetGender(string value)
        {
            try
            {
                var number = (int)char.GetNumericValue(value[10]);
                return number%2 == 0 ? TinGender.Female : TinGender.Male;
            }
            catch (Exception)
            {
                return TinGender.Unknown;
            }
        }

        private static DateTime? GetDate(string number)
        {
            try
            {
                var centuryChar = number[7];
                var day = int.Parse(number.Substring(0, 2));
                var month = int.Parse(number.Substring(2, 2));
                var year = int.Parse(number.Substring(4, 2));
                year = GetCentury(year, centuryChar) + year;
                var valid = Utils.IsValidDate(year, month, day);
                if (!valid) return null;
                return new DateTime(year, month, day, 0, 0, 0, DateTimeKind.Local);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static int GetCentury(int year, char century)
        {
            const int unused = 0;
            switch (century)
            {
                case '0':
                case '1':
                case '2':
                case '3':
                    return 1900;
                case '5':
                case '6':
                case '7':
                case '8':
                    if (year < 37) return 2000;
                    return year > 57 ? 1800 : unused;
                case '4':
                case '9':
                    return year < 37 ? 2000 : 1900;
                default:
                    return 0;
            }
        }

        private static bool IsValidCvr(string value)
        {
            value = value.RemoveAllExcept("0123456789");
            if (value.Length != 8) return false;
            var sum = 0;
            for (var i = 0; i < 8; i++)
            {
                var counter = i + 1;
                var d = (int)char.GetNumericValue(value[i]);
                if (counter == 1)
                {
                    sum += d*2;
                }
                else
                {
                    sum += d * (9 - counter);
                }
            }
            return sum%11 == 0;
        }

    }
}
