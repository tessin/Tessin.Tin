using System;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using Tessin.Tin.Models;

namespace Tessin.Tin.Sweden
{

    public enum Gender
    { 
        Any = 0,
        Male = 1,
        Female = 2
    }

    public enum CompanyType
    { 
        Any = 0,
        State = 2,
        Aktiebolag = 5,
        Enkelt = 6,
        EkonomiskForening = 7,
        IdeelForening = 8,
        Other = 9,
        Enskild = 98,
        Unknown = 99 
    }
    
    public static class UtilSe
    {
        public static readonly Regex IsNumeric = new Regex("^[0-9]+$");
        public static readonly Regex IsPersonnummerStrict = new Regex("^[0-9]{6}[\\-+][0-9]{4}$");
        public static readonly Regex IsOrganisationNumberStrict = new Regex("^[0-9]{6}-[0-9]{4}$");

        private static readonly Random R = new Random();

        public static string GetRandomGenderDigit(Gender gender)
        {
            if (gender == Gender.Any) return R.Next(10).ToString();
            var rv = R.Next(5)*2;
            return (gender == Gender.Female ? rv : rv + 1).ToString();
        }

        public static Gender GetGender(string pnr)
        {

            return int.Parse(pnr[8].ToString()) % 2 == 0 ? Gender.Female : Gender.Male;
        }

        public static CompanyType GetCompanyType(string onr)
        {
            switch(onr[0])
            {
                case '2': return CompanyType.State;
                case '5': return CompanyType.Aktiebolag;
                case '6': return CompanyType.Enkelt;
                case '7': return CompanyType.EkonomiskForening;
                case '8': return CompanyType.IdeelForening;
                case '9': return CompanyType.Other;
                default:
                    {
                        // If the personal number validates as a canonical personal 
                        // number, then it is likely "enskild firma", otherwise it 
                        // is unknown.
                        return ValidateSe.ValidatePnrCanonical(onr.Insert(6, "-")) ? 
                            CompanyType.Enskild : CompanyType.Unknown;
                    }
            }
        }

        public static TinEntityType GetEntityType(string onr)
        {
            if (onr.Length != 11) return null;
            var c = onr[0];
            if (!char.IsDigit(c)) return null;
            var type = int.Parse(c.ToString());
            return EntityTypesSe.Lookup[type];
        }

        #region Generate

        public static CompanyType GetRandomCompanyType()
        {
            var choice = R.Next(6);
            switch (choice)
            {
                case 0: return CompanyType.Aktiebolag;
                case 1: return CompanyType.EkonomiskForening;
                case 2: return CompanyType.Enkelt;
                case 3: return CompanyType.IdeelForening;
                case 4: return CompanyType.Other;
                case 5: return CompanyType.State;
            }
            return CompanyType.Aktiebolag;
        }

        public static string RandomDateString()
        {
            var year = R.Next(0, 99);
            var leapYear = DateTime.IsLeapYear(1900 + year);
            var month = R.Next(1, 12);
            // 30 days hath September,
            // April, June and November,
            // All the rest have 31,
            // Excepting February alone
            // (And that has 28 days clear,
            // With 29 in each leap year).
            var day = (month == 4 || month == 6 || month == 9 || month == 11)
                          ? R.Next(1, 30)
                          : (month == 2 ? (leapYear ? R.Next(1, 29) : R.Next(1, 28)) : R.Next(1, 31));
            return string.Format(CultureInfo.InvariantCulture, "{0:D2}{1:D2}{2:D2}", year, month, day);
        }

        public static string RandomDecimalString(int length)
        {
            var sb = new StringBuilder(length);
            for (var i = 0; i < length; i++)
            {
                sb.Append(R.Next(10).ToString());
            }
            return sb.ToString();
        }

        #endregion
    }
}
