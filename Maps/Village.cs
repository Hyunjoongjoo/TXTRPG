using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TXTRPG
{
    internal class Village : Map, ISafeZone
    {
        private VillageManager villageManager;
        public Village(string name, Player player)
        {
            Name = name;
        }
        public override void Enter(Player player)
        {
            villageManager.EnterVillage(player);
        }
        public void Rest(Player player)
        {
            player.Heal(player.MaxHp);
        }
        public void Trade(Player player)
        {

            Shop shop = new Shop("상점",player);
            shop.Trade(player);
        }
    }
}
