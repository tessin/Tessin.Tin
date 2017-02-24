using System;
using System.Collections.Generic;
using System.Globalization;

namespace Tessin.Tin.Sweden
{
    public static class SeGenerate
    {

        private static readonly Random r = new Random();

        public static string GeneratePnr()
        {
            var pnr = UtilSe.RandomDateString() + UtilSe.RandomDecimalString(3);
            pnr = pnr + LuhnAlgorithm.Calculate(pnr);
            return pnr;
        }

        public static string GeneratePnr(Gender gender)
        {
            var pnr = UtilSe.RandomDateString() + UtilSe.RandomDecimalString(2) +
                         UtilSe.GetRandomGenderDigit(gender);
            pnr = pnr + LuhnAlgorithm.Calculate(pnr);
            return pnr;
        }

        public static List<string> GeneratePnr(int number)
        {
            var lookup = new HashSet<string>();
            var result = new List<string>(number);
            for (var i = 0; i < number; i++)
            {
                redo:
                var pnr = UtilSe.RandomDateString() + UtilSe.RandomDecimalString(3);
                pnr = pnr + LuhnAlgorithm.Calculate(pnr);
                if (!lookup.Contains(pnr))
                {
                    lookup.Add(pnr);
                    result.Add(pnr);
                }
                else
                {
                    goto redo;    
                }
            }
            return result;
        }

        public static string GenerateOnr()
        {
            return GenerateOnr(CompanyType.Any);
        }

        public static string GenerateOnr(CompanyType type)
        {
            var idx0 = (type == CompanyType.Any) ? 
                (int)UtilSe.GetRandomCompanyType() : (int) type;
            var idx1 = r.Next(10);
            var month = 20 + r.Next(80);
            var day = r.Next(100);
            var serial = r.Next(1000);
            var onr = string.Format(CultureInfo.InvariantCulture,
                "{0:D1}{1:D1}{2:D2}{3:D2}{4:D3}",
                idx0, idx1, month, day, serial);
            onr += LuhnAlgorithm.Calculate(onr);
            return onr;
        }

    }
}
