namespace Tessin.Tin.Models.Extensions
{
    public static class TinResponseExtensions
    {
        public static bool IsValid(this TinResponse tin)
        {
            return tin.Status == TinStatus.Valid;
        }

        public static bool IsValidOrUncertain(this TinResponse tin)
        {
            return tin.Status == TinStatus.Valid || tin.Status == TinStatus.Uncertain;
        }

        public static bool IsInvalid(this TinResponse tin)
        {
            return tin.Status == TinStatus.Invalid;
        }

        public static bool IsUncertain(this TinResponse tin)
        {
            return tin.Status == TinStatus.Uncertain;
        }

    }
}
