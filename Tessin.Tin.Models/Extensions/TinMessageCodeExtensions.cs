using System;

namespace Tessin.Tin.Models.Extensions
{
    public static class TinMessageCodeExtensions
    {

        public static TinMessage ToInfoMessage(this TinMessageCode code)
        {
            switch (code)
            {
                case TinMessageCode.InfoAttemptedMatchForPerson:
                    return new TinMessage(TinMessageType.Information, "Previosly failed to match value as a person. Retrying evaluation as entity.", code);
                case TinMessageCode.InfoAgeExcessive:
                    return new TinMessage(TinMessageType.Information, "The calculated age is excessive.", code);
                case TinMessageCode.InfoAgeMinor:
                    return new TinMessage(TinMessageType.Information, "This person is a minor.", code);
                case TinMessageCode.InfoAgeSenior:
                    return new TinMessage(TinMessageType.Information, "This person is a senior.", code);
                case TinMessageCode.InfoChecksumNotVerified:
                    return new TinMessage(TinMessageType.Information, "No checksum present or checksum not verified.", code);
                case TinMessageCode.InfoUnknownEntityType:
                    return new TinMessage(TinMessageType.Information, "The normalized value does not correspond to a known entity type.", code);
                case TinMessageCode.InfoAmbiguousCentury:
                    return new TinMessage(TinMessageType.Information, "The calculated century may be incorrect.", code);
                default:
                    throw new InvalidOperationException($"Unknown info code '{code}'.");
            }
        }

        public static TinMessage ToErrorMessage(this TinMessageCode code)
        {
            switch (code)
            {

                case TinMessageCode.ErrorValueIsNullOrWhitespace:
                    return new TinMessage(TinMessageType.Error, "The supplied value is null or whitespace.", code);
                case TinMessageCode.ErrorNormalizationFailed:
                    return new TinMessage(TinMessageType.Error, "Normalization failed.", code);
                case TinMessageCode.ErrorInvalidType:
                    return new TinMessage(TinMessageType.Error, "Invalid type code.", code);
                case TinMessageCode.ErrorInvalidChecksum:
                    return new TinMessage(TinMessageType.Error, "Invalid checksum.", code);
                case TinMessageCode.ErrorFormatMismatch:
                    return new TinMessage(TinMessageType.Error, "The normalized value does not match the expected pattern.", code);
                case TinMessageCode.ErrorFormatMismatchEntity:
                    return new TinMessage(TinMessageType.Error, "The normalized value does not match the expected pattern.", code);
                case TinMessageCode.ErrorFormatMismatchPerson:
                    return new TinMessage(TinMessageType.Error, "The normalized value does not match the expected pattern.", code);
                case TinMessageCode.ErrorInvalidDate:
                    return new TinMessage(TinMessageType.Error, "The embedded date is invalid.", code);
                case TinMessageCode.ErrorNegativeAge:
                    return new TinMessage(TinMessageType.Error, "The computed age is negative.", code);
                case TinMessageCode.ErrorInvalidEntityType:
                    return new TinMessage(TinMessageType.Error, "The infered entity type is invalid.", code);
                case TinMessageCode.ErrorInternal:
                    return new TinMessage(TinMessageType.Error, "An internal error occured.", code);
                default:
                    throw new InvalidOperationException($"Unknown error code '{code}'.");
            }
        }


    }
}
