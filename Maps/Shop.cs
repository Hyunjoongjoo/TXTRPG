using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TXTRPG
{
    internal class Shop : Map, ISafeZone
    {
        private ShopManager shopManager;
        private Shop shop;

        public Shop(string name, Player player)
        {
            Name = name;
            shopManager = new ShopManager(player, shop);
        }

        public override void Enter(Player player)
        {
            shopManager.EnterShop(player);
        }
        public void Trade(Player player)
        {
            Enter(player);
        }
    }
}
