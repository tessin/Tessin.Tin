using System;
using System.Globalization;
using Tessin.Tin.Extensions;
using Tessin.Tin.Models;

namespace Tessin.Tin.Sweden
{
    public class ValidateSe
    {

        public static string NormalizePnr(string pnr)
        {
            pnr = pnr.Replace(" ","");
            var di = pnr.Length - 5;
            var dash = pnr[di];
            if (!(dash == '-' || dash == '+'))
            {
                dash = '-';
                pnr = pnr.Insert(di + 1, dash.ToString()); // TODO: Check
            }
            if (pnr.Length != 11 && pnr.Length != 13) return null;
            if (pnr.Length == 13 && dash == '+') pnr = pnr.Replace('+', '-');
            if (pnr.Length != 11) return pnr;
            int year;
            if (int.TryParse(pnr.Substring(0, 2), out year))
            {
                var century = GetLikelyCentury(year, dash == '+');
                // The plus sign is not expected in the normalized form.
                pnr = pnr.Replace('+', '-'); 
                pnr = century + pnr;
            }
            else
            {
                return null;
            }
            return pnr;
        }

        public static TinGender GetNormalizedPnrGender(string pnr)
        {
            var c = int.Parse(pnr[pnr.Length - 2].ToString());
            return (c%2 == 0) ? TinGender.Female : TinGender.Male;
        }

        public static string NormalizeOnr(string onr)
        {
            onr = onr.Replace(" ","");
            if (onr.StartsWith("SE") && onr.EndsWith("01"))
            {
                onr = onr.Substring(2, onr.Length - 4);
            }
            var di = onr.Length - 5;
            var dash = onr[di];
            if (dash == '-') return onr.Length != 11 ? null : onr;
            dash = '-';
            onr = onr.Insert(di + 1, dash.ToString());
            return onr.Length != 11 ? null : onr;
        }


        /// <summary>
        /// Validates a Swedish person number in the canonical form.
        /// </summary>
        /// <param name="pnr">10 digit person number to validate.</param>
        /// <returns>True if the person number appears valid. 
        /// False if not.</returns>
        /// <remarks>The number should be of the form: 
        /// 121212-345X where X is the Luhn control 
        /// digit.</remarks>
        public static bool ValidatePnrCanonical(string pnr)
        {
            if (pnr.Length != 11) return false;
            //throw new ArgumentException("The parameter must be of length 11."); // TODO: Add test for exception.
            if (!UtilSe.IsPersonnummerStrict.IsMatch(pnr))
                return false; //throw new ArgumentException("The input string must match the format" + 
            //    " of a swedish person number (i.e. 121212[-/+]345X)."); // TODO: Add test for exception.

            // Educated guessing.
            var lc = GetLikelyCentury(int.Parse(pnr.Substring(0, 2)), pnr[7] == '+');

            // Add the century to the date part of the person number.
            var date = lc + pnr.Substring(0, 6);

            // Bail out if no valid date could be 
            // found in the number.
            if (!IsValidDate(date)) return false;

            var luhnstring = pnr.Substring(0, 6) + pnr.Substring(7);

            // Test the check digit.
            return LuhnAlgorithm.Test(luhnstring);
        }

        /// <summary>
        /// Validates a Swedish person number in the now typical form including 
        /// century information.
        /// </summary>
        /// <param name="pnr">12 digit person number to validate.</param>
        /// <returns>True if the person number appears valid. 
        /// False if not.</returns>
        /// <remarks>The number should be of the form: 
        /// 19121212345X where X is the Luhn control 
        /// digit.</remarks>
        public static bool ValidatePnrLong(string pnr)
        {
            pnr = pnr.RemoveAllNonNumeric();
            if (pnr.Length != 12) return false;
            if (!IsValidDate(pnr.Substring(0, 8))) return false;
            // Test the check digit.
            return LuhnAlgorithm.Test(pnr.Substring(2));
        }

        /// <summary>
        /// Validates a Swedish organisation number 
        /// (legal entity id).
        /// </summary>
        /// <param name="onr">Organisation number to validate.</param>
        /// <returns>True if the number appears valid. False if not.</returns>
        public static bool ValidateOnr(string onr)
        {
            //if (onr.Length != 11) return false;
            onr = onr.RemoveAllNonNumeric();
            if (onr.Length != 10) return false;
            // Validate the check digit.
            if (!LuhnAlgorithm.Test(onr)) return false;
            // Check that the "month" component is equal to or greater than 20.
            var month = int.Parse(onr.Substring(2, 2));
            if (month < 20) return false;
            // Check that the company type is not unknown.
            return UtilSe.GetCompanyType(onr) != CompanyType.Unknown;
        }


        public static int GetLikelyCentury(int year, bool plus100)
        {
            // 501127 -> 18501127
            var now = TinGlobal.Now;
            var scy = now.Year%100; // Current short year (e.g. 11)
            var cc = (now.Year - scy)/100; // Current century (e.g. 2000)
            var siy = year%100; // Input short year (e.g. 78).
            var ec = plus100 ? cc - 1 : cc; // Earliest century for birth date if 100+ (e.g. 1900).
            return siy > scy ? ec - 1 : ec;
        }

        public static DateTime? GetDate(string pnr, CultureInfo cultureInfo)
        {
            var datePart = pnr.Substring(0, 8);
            if (!IsValidDate(datePart)) return null;
            try
            {
                return DateTime.ParseExact(datePart, "yyyyMMdd", cultureInfo, DateTimeStyles.AssumeLocal);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Validates the string representation of a date on the form YYYYMMDD.
        /// </summary>
        /// <param name="date">Date string to validate.</param>
        /// <returns>True if valid. False if not.</returns>
        public static bool IsValidDate(string date)
        {
            if (date.Length != 8)
                return false;
            if (!UtilSe.IsNumeric.IsMatch(date)) return false;
            var year = int.Parse(date.Substring(0, 4));
            var month = int.Parse(date.Substring(4, 2));
            var day = int.Parse(date.Substring(6, 2));
            // 30 days hath September,
            // April, June and November,
            // All the rest have 31,
            // Excepting February alone
            // (And that has 28 days clear,
            // With 29 in each leap year).
            if (year < 1 || year > 9999) return false;
            return (month == 4 || month == 6 || month == 9 || month == 11)
                             ? day < 31
                             : (month == 2 
                             ? (DateTime.IsLeapYear(year) 
                             ? day < 30 : day < 29) 
                             : day < 32) && day > 0;
        }
    }
}
