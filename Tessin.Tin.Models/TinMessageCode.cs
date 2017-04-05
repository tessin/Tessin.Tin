namespace Tessin.Tin.Models
{

    public enum TinMessageCode
    {
        None = 0,
        ErrorValueIsNullOrWhitespace = 101,
        ErrorNormalizationFailed = 102,
        ErrorInvalidType = 103,
        ErrorFormatMismatch = 111,
        ErrorFormatMismatchEntity = 112,
        ErrorFormatMismatchPerson = 113,
        ErrorInvalidDate = 121,
        ErrorNegativeAge = 122,
        ErrorUndefinedAge = 123,
        ErrorInvalidEntityType = 131,
        ErrorInvalidChecksum = 141,
        ErrorUnknownCountry = 151,
        ErrorInternal = 199,
        

        InfoAttemptedMatchForPerson = 11,
        InfoAgeMinor = 21,
        InfoAgeSenior = 22,
        InfoAgeExcessive = 23,
        InfoAmbiguousCentury = 24,
        InfoUnknownEntityType = 31,
        InfoChecksumNotVerified = 41,

    }
}
