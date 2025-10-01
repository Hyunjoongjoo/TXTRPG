using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TXTRPG
{
    public static class RandomEx
    {
        public static int NextFix(this Random ran, int min, int max)
        {
            return ran.Next(min, max + 1);
        }
    }
}
