using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tessin.Tin.Extensions
{
    public static class IntegerExtensions
    {
        public static bool Between(this int value, int min, int max)
        {
            return value >= min && value <= max;
        }
    }
}
