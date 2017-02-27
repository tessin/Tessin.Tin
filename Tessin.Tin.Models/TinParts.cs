using System;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using Tessin.Tin.Models.Extensions;

namespace Tessin.Tin.Models
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

    public enum YearFormat
    {
        Short = 1,
        Long = 0
    }

    public class TinParts
    {

        public int? LongYearNumeric
        {
            get
            {
                var year = Century + YearNumeric;
                return year;
            }
        }

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

        public YearFormat YearFormat { get; set; }

        public TinDateFormat DateFormat { get; set; }

        public TinSeparatorPosition SeparatorPosition { get; set; }

    }
}
