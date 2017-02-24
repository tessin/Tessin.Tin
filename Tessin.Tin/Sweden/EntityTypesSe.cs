using System.Collections.Generic;
using Tessin.Tin.Models;

namespace Tessin.Tin.Sweden
{

    //public enum CompanyType
    //{
    //    Any = 0,
    //    State = 2,
    //    Aktiebolag = 5,
    //    Enkelt = 6,
    //    EkonomiskForening = 7,
    //    IdeelForening = 8,
    //    Other = 9,
    //    Enskild = 98,
    //    Unknown = 99
    //}

    public static class EntityTypesSe
    {

        public static TinEntityType[] Lookup = new TinEntityType[10];

        static EntityTypesSe()
        {
            Lookup[1] = Type1;
            Lookup[2] = Type2;
            Lookup[3] = Type3;
            Lookup[5] = Type5;
            Lookup[6] = Type6;
            Lookup[7] = Type7;
            Lookup[8] = Type8;
            Lookup[9] = Type9;
        }
        
        public static TinEntityType Type1 = new TinEntityType(TinCountry.Sweden, "Dödsbo", null);
        public static TinEntityType Type2 = new TinEntityType(TinCountry.Sweden, "Stat, landsting, kommun, eller församling.", null);
        public static TinEntityType Type3 = new TinEntityType(TinCountry.Sweden, "Utländskt företag", null);
        public static TinEntityType Type5 = new TinEntityType(TinCountry.Sweden, "Aktiebolag", "AB");
        public static TinEntityType Type6 = new TinEntityType(TinCountry.Sweden, "Enkelt bolag", null);
        public static TinEntityType Type7 = new TinEntityType(TinCountry.Sweden, "Ekonomisk förening eller bostadsrättsförening.", null);
        public static TinEntityType Type8 = new TinEntityType(TinCountry.Sweden, "Ideel förening eller stiftelse.", "");
        public static TinEntityType Type9 = new TinEntityType(TinCountry.Sweden, "Handelsbolag, kommanditbolag eller enkelt bolag.", "HB,KB");
    }
}
