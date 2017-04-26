using System;
using Tessin.Tin.Models.Extensions;

namespace Tessin.Tin
{

    public enum TinSeparatorPosition
    {
        None = 0,
        AfterDate = 1,
        AfterSerial = 2
    }

    public enum TinDateFormat
    {
        YearMonthDay = 1,
        DayMonthYear = 2
    }

    public enum TinYearFormat
    {
        Short = 1,
        Long = 0
    }

    public class TinParts
    {

        public TinParts()
        {
        }

        public TinParts(TinDateFormat dateFormat, TinYearFormat yearFormat)
        {
            DateFormat = dateFormat;
            YearFormat = yearFormat;
        }

        public int? LongYearNumeric => Century + YearNumeric;

        public string Year { get; set; }
        
        public string Month { get; set; }

        public string Day { get; set; }

        public int? Century { get; set; }

        public string Serial { get; set; }

        public int? YearNumeric => Year.ToNullableInteger();

        public int? MonthNumeric => Month.ToNullableInteger();

        public int? DayNumeric => Day.ToNullableInteger();

        public int SerialNumeric => Serial.ToInteger();
        
        public string Separator { get; set; }

        public string Checksum { get; set; }

        public TinYearFormat YearFormat { get; set; }

        public TinDateFormat DateFormat { get; set; }

        public TinSeparatorPosition SeparatorPosition { get; set; }

        public bool HasDate => !string.IsNullOrEmpty(Year) && !string.IsNullOrEmpty(Month) && !string.IsNullOrEmpty(Day);
            
        public override string ToString()
        {
            return ToString(true);
        }

        public string ToStringWithoutChecksum()
        {
            return ToString(false);
        }

        private string ToString(bool includeChecksum)
        {
            var year = YearFormat == TinYearFormat.Long ? LongYearNumeric?.ToString("0000") : YearNumeric?.ToString("00");
            string datePart = "";
            if (HasDate)
            { 
                switch (DateFormat)
                {
                    case TinDateFormat.YearMonthDay:
                        datePart = $"{year}{Month}{Day}";
                        break;
                    case TinDateFormat.DayMonthYear:
                        datePart = $"{Day}{Month}{year}";
                        break;
                    default:
                        throw new Exception("Unknown date format.");
                }
            }
            var separatorAfterDate = SeparatorPosition == TinSeparatorPosition.AfterDate ? Separator : "";
            var separatorAfterSerial = SeparatorPosition == TinSeparatorPosition.AfterSerial ? Separator : "";
            var checksum = includeChecksum ? Checksum : "";
            return $"{datePart}{separatorAfterDate}{Serial}{separatorAfterSerial}{checksum}";
        }

        public DateTime? GetDate()
        {
            var year = LongYearNumeric;
            var month = MonthNumeric;
            var day = DayNumeric;
            if (year == null || month == null || day == null) return null;
            if (!Utils.IsValidDate(year.Value, month.Value, day.Value)) return null;
            return new DateTime(year.Value, month.Value, day.Value, 0, 0, 0);
        }

        public void SetDateInfo(string date, TinDateFormat format)
        {
            if (date.Length != 6) throw new ArgumentException("The datestring must be exactly 6 characters long.");
            
            var p1 = date.Substring(0, 2);
            var p2 = date.Substring(2, 2);
            var p3 = date.Substring(4, 2);
            switch (format)
            {
                case TinDateFormat.YearMonthDay:
                    Year = p1;
                    Month = p2;
                    Day = p3;
                    break;
                case TinDateFormat.DayMonthYear:
                    Year = p3;
                    Month = p2;
                    Day = p1;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(format));
            }
        }

    }
}
