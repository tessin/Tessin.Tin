namespace Tessin.Tin.Models
{

    public enum TinMessageCode
    {
        None = 0,
        ErrorValueIsNullOrWhitespace = 101,
        ErrorNormalizationFailed = 103,
        ErrorInvalidType = 105,
        ErrorFormatMismatch = 111,
        ErrorFormatMismatchEntity = 113,
        ErrorFormatMismatchPerson = 115,
        ErrorInvalidDate = 121,
        ErrorNegativeAge = 123,
        ErrorInvalidEntityType = 131,
        ErrorInvalidChecksum = 141,
        ErrorInternal = 199,

        InfoAttemptedMatchForPerson = 10,
        InfoAgeMinor = 20,
        InfoAgeSenior = 22,
        InfoAgeExcessive = 24,
        InfoAmbiguousCentury = 26,
        InfoUnknownEntityType = 30,
        InfoChecksumNotVerified = 40,

    }
}
