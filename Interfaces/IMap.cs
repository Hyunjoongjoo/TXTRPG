using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TXTRPG
{
    public interface ISafeZone
    {
        void Trade(Player player);
    }
    public interface IDangerousZone
    {
        void Battle(Player player);
    }
    public interface IVillageManager
    {
        void EnterVillage(Player player);
    }
}
