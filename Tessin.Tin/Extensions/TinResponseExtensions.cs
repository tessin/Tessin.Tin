using Tessin.Tin.Models;
using Tessin.Tin.Models.Extensions;

namespace Tessin.Tin.Extensions
{
    public static class TinResponseExtensions
    {
        public static TinResponse AddError(this TinResponse tin, TinMessageCode code)
        {
            if (code == TinMessageCode.None) return tin;
            tin.Messages.Add(code.ToErrorMessage());
            tin.Status = TinStatus.Invalid;
            return tin;
        }

        public static TinResponse AddInfo(this TinResponse tin, TinMessageCode code)
        {
            if (code == TinMessageCode.None) return tin;
            tin.Messages.Add(code.ToInfoMessage());
            return tin;
        }

        public static void HandleAge(this TinResponse tin)
        {
            var age = tin.Date.ToAge();
            if (age == null)
            {
                tin.AddError(TinMessageCode.ErrorInternal);
                return;
            }
            if (age < AgeLimits.AgeZero) tin.AddError(TinMessageCode.ErrorNegativeAge);
            if (age < AgeLimits.AgeAdult) tin.AddInfo(TinMessageCode.InfoAgeMinor);
            if (age >= AgeLimits.AgeSenior) tin.AddInfo(TinMessageCode.InfoAgeSenior);
            if (age > AgeLimits.AgeExcessive) tin.AddInfo(TinMessageCode.InfoAgeExcessive);
            tin.Age = age;
        }

    }
}
