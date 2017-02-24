using System;
using System.Linq;
using System.Text.RegularExpressions;
using Tessin.Tin.Extensions;
using Tessin.Tin.Models;

namespace Tessin.Tin.Finland
{
    public class TinEvaluatorFi : ITinEvaluator
    {

        private static readonly Regex FinishEntityTinRegex = new Regex("^[0-9]{7}-[0-9]$", RegexOptions.Compiled);

        private static readonly Regex FinishPersonTinRegex = new Regex("^[0-9]{6}[-+A][0-9]{3}[0-9A-FHJK-NPR-Y]$",
            RegexOptions.Compiled);

        public string Country => "fi";

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

            var tin = new TinResponse();
            tin.Value = value;
            tin.Country = Country;

            if (string.IsNullOrWhiteSpace(value))
            {
                return tin.AddError(TinMessageCode.ErrorValueIsNullOrWhitespace);
            }

            string normalized;
            if (type == TinType.Entity)
            {
                tin.Type = type;
                normalized = NormalizeEntity(value);
                if (normalized == null) return tin.AddError(TinMessageCode.ErrorNormalizationFailed);
                tin.NormalizedValue = normalized;
                if (!FinishEntityTinRegex.IsMatch(normalized))
                {
                    return tin.AddError(TinMessageCode.ErrorFormatMismatchEntity);
                }
                var parts = normalized.Split('-');
                var number = parts[0];
                var check = parts[1];
                var calculated = CalculateModulus11_2(number);
                if (calculated != check)
                {
                    return tin.AddError(TinMessageCode.ErrorInvalidChecksum);
                }
                tin.Status = TinStatus.Valid;
            }
            else if (type == TinType.Person)
            {
                tin.Type = type;
                normalized = NormalizePerson(value);
                if (normalized == null)
                    return tin.AddError(TinMessageCode.ErrorNormalizationFailed);
                tin.NormalizedValue = normalized;
                if (!FinishPersonTinRegex.IsMatch(normalized))
                {
                    return tin.AddError(TinMessageCode.ErrorFormatMismatchPerson);
                }

                tin.Date = GetDate(normalized);
                tin.Age = tin.Date.ToAge();
                if (tin.Age < 0) tin.AddError(TinMessageCode.ErrorNegativeAge);
                if (tin.Age < 18) tin.AddInfo(TinMessageCode.InfoAgeMinor);
                if (tin.Age >= 65) tin.AddInfo(TinMessageCode.InfoAgeSenior);
                if (tin.Age > 105) tin.AddInfo(TinMessageCode.InfoAgeExcessive);
                if (tin.Age > 150) tin.AddError(TinMessageCode.ErrorAgeLimit);
                tin.Gender = GetGender(normalized);

                var check = normalized.Last();
                var calculated = GetPnrCheckDigit(normalized);
                if (!calculated.HasValue || calculated != check)
                {
                    tin.AddError(TinMessageCode.ErrorInvalidChecksum);
                }

                tin.Status = tin.Messages.Any(p => p.Type == TinMessageType.Error) ? TinStatus.Invalid : TinStatus.Valid;

            }
            else
            {
                return tin.AddError(TinMessageCode.ErrorInvalidType);
            }

            return tin;
        }

        private static string NormalizePerson(string value)
        {
            value = value.Replace(" ", "").ToUpper();
            return value.Length != 11 ? null : value;
        }

        private static string NormalizeEntity(string value)
        {
            value = value.Replace(" ", "");
            if (value.Length < 8) return null;
            if (value.StartsWith("FI"))
                value = value.Remove(0, 2); // Convert from VAT number. TODO: Do the same for swedish.
            var di = value.Length - 2;
            var dash = value[di];
            if (dash != '-') value = value.Insert(di + 1, "-");
            return value.Length != 9 ? null : value;
        }

        /// <summary>
        /// Calculates the check digit for finish entity numbers.
        /// </summary>
        /// <remarks>This is the Finish variant of the Modulus 11-2 algorithm.</remarks>
        /// <param name="value"></param>
        /// <returns>Check digit.</returns>
        public static string CalculateModulus11_2(string value)
        {
            var p = 0;
            foreach (var c in value)
            {
                p = 2*(p + (int) char.GetNumericValue(c));
            }
            p = p%11;
            var check = ((11 - p)%11);
            return check > 9 ? "X" : check.ToString();
        }

        private static char? GetPnrCheckDigit(string value)
        {
            try
            {
                const string lookup = "0123456789ABCDEFHJKLMNPRSTUVWXY";
                value = value.Remove(6, 1);
                var numeric = value.Substring(0, 9);
                var num = long.Parse(numeric);
                var remainder = (int) (num%31);
                return lookup[remainder];
            }
            catch (Exception)
            {
                return null;
            }
        }

        private static TinGender GetGender(string value)
        {
            try
            {
                var individual = value.Substring(7, 3);
                var val = int.Parse(individual);
                return val % 2 == 0 ? TinGender.Female : TinGender.Male;
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
                var centuryChar = number[6];
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
            switch (century)
            {
                case '-':
                    return 1900;
                case '+':
                    return 1800;
                case 'A':
                    return 2000;
                default:
                    return -1;
            }
        }

    }
}
