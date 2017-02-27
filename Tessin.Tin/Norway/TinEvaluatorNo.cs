using System;
using System.Linq;
using System.Text.RegularExpressions;
using Tessin.Tin.Extensions;
using Tessin.Tin.Models;
using Tessin.Tin.Models.Extensions;

namespace Tessin.Tin.Norway
{
    public class TinEvaluatorNo : ITinEvaluator
    {
        private static readonly Regex DanishPersonTinRegex = new Regex("^[0-9]{11}$",
            RegexOptions.Compiled);

        private static readonly Regex DanishEntityTinRegex = new Regex("^[0-9]{9}$",
            RegexOptions.Compiled);

        public TinCountry Country => TinCountry.Denmark;

        public TinResponse Evaluate(string value)
        {
            var result = Evaluate(value, TinType.Person);
            if (result.Status != TinStatus.Invalid) return result;
            result = Evaluate(value, TinType.Entity);
            result.AddInfo(TinMessageCode.InfoAttemptedMatchForPerson);
            return result;
        }

        public TinResponse Evaluate(string value, TinType type)
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
                if (!DanishPersonTinRegex.IsMatch(normalized))
                {
                    return response.AddError(TinMessageCode.ErrorFormatMismatchPerson);
                }

                var parts = new TinParts();
                parts.Serial = normalized.Substring(6, 3);
                parts.Checksum = normalized.Substring(9, 2);
                parts.SetDate(normalized.Substring(0,6), TinDateFormat.DayMonthYear);
                parts.Century = GetCentury(response, parts.YearNumeric, parts.SerialNumeric);
                response.Gender = GetGender(parts);
                response.Date = parts.GetDate();
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

                var calculated = CalculatePersonChecksum(normalized);
                if (parts.Checksum != calculated)
                {
                    response.AddError(TinMessageCode.ErrorInvalidChecksum);
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
                if (!DanishEntityTinRegex.IsMatch(normalized))
                {
                    return response.AddError(TinMessageCode.ErrorFormatMismatchEntity);
                }

                var parts = new TinParts();
                parts.Serial = normalized.Substring(0, 8);
                parts.Checksum = normalized.Substring(8, 1);
                var calculated = CalculateEntityChecksum(parts.Checksum);
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
            value = value.Replace(" ", "").ToUpper();
            return value.Length != 11 ? null : value;
        }

        private static string NormalizeEntity(string value)
        {
            value = value.Replace(" ", "").ToUpper().Replace("MVA","");
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

        private static string CalculateEntityChecksum(string number)
        {
            var chk = number.CalculateMod11Checksum(new[] {3, 2, 7, 6, 5, 4, 3, 2});
            return chk.ToString();
        }

        private static string CalculatePersonChecksum(string number)
        {
            var chk1 = number.CalculateMod11Checksum(new[] { 3, 7, 6, 1, 8, 9, 4, 5, 2 });
            number = chk1 + number;
            var chk2 = number.CalculateMod11Checksum(new[] { 5, 4, 3, 2, 7, 6, 5, 4, 3, 2 });
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

        // function checkNIN(o) {
        //     var nin = o.value;
        //     if (nin.length != 11) {
        //         return "must be 11 chars";
        //     }
        //     var cc0 = '0'.charAt(0);
        //     var cw1 = [3, 7, 6, 1, 8, 9, 4, 5, 2];
        //     var cw2 = [5, 4, 3, 2, 7, 6, 5, 4, 3, 2];
        //     // check digit 1
        //     var ws1 = 0;
        //     for (var n=0; n<cw1.length; n++) {
        //         ws1 += cw1[n] * (nin.charAt(n) - cc0);
        //     }
        //     var cd1 = 11 - (ws1 % 11);
        //     if (cd1 == 10) {
        //         return "invalid number, check digit 1 would be 10";
        //     }
        //     if (cd1 == 11) {
        //         cd1 = 0;
        //     }
        //     if (nin.charAt(cw1.length) - cc0 != cd1) {
        //         return "check digit 1 should be " + cd1;
        //     }
        //     // check digit 2
        //     var ws2 = 0;
        //     for (var n=0; n<cw2.length; n++) {
        //         ws2 += cw2[n] * (nin.charAt(n) - cc0);
        //     }
        //     var cd2 = 11 - (ws2 % 11);
        //     if (cd2 == 10) { // assuming this is needed
        //         return "invalid number, check digit 2 would be 10";
        //     }
        //     if (cd2 == 11) {
        //         cd2 = 0;
        //     }
        //     if (nin.charAt(cw2.length) - cc0 != cd2) {
        //         return "check digit 2 should be " + cd2;
        //     }
        // 
        //     // check age
        //     var century = -1, numyear = Number(nin.substr(4,2)), numindiv = Number(nin.substr(6,3));
        //     if (numindiv > 499) {
        //         if (numindiv < 750 && numyear >= 54) {
        //             century = 18;
        //         } else if (numyear < 40) {
        //             century = 20;
        //         } else if (numindiv = 900 && numyear >= 40) { // special cases
        //             century = 19;
        //         }
        //     } else {
        //         century = 19;
        //     }
        // 
        //     if (century == -1) {
        //         return "invalid combination of year and individual number";
        //     }
        // 
        //     var check18 = new Date(century * 100 + numyear + 18, Number(nin.substr(2,2))-1, Number(nin.substr(0,2)), 0, 0, 0, 0);
        //     alert (check18);
        //     if (check18 > new Date()) {
        //         return "Sorry, you have to be at least 18 to get registered";
        //     }
        // 
        //     return "ok!";
        // }

    }
}
