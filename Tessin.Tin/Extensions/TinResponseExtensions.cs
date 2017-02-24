using Tessin.Tin.Models;
using Tessin.Tin.Models.Extensions;

namespace Tessin.Tin.Extensions
{
    public static class TinResponseExtensions
    {
        public static TinResponse AddError(this TinResponse tin, TinMessageCode code)
        {
            tin.Messages.Add(code.ToErrorMessage());
            tin.Status = TinStatus.Invalid;
            return tin;
        }

        public static TinResponse AddInfo(this TinResponse tin, TinMessageCode code)
        {
            tin.Messages.Add(code.ToInfoMessage());
            return tin;
        }


    }
}
