using System;

namespace Tessin.Tin
{
    public static class TinGlobal
    {
        public static DateTime? OverrideDate { get; set; }

        public static DateTime Now => OverrideDate ?? DateTime.Now;
        
    }
}
