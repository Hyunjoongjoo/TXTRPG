using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TXTRPG
{
    class GameManager
    {
        private static Random ran = new Random();
        public static Random GetRandom()
        {
            return ran;
        }
    }
}
