using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TXTRPG
{

    public abstract class Map
    {
        public string Name { get; protected set; }
        public abstract void Enter(Player player);
    }

}
