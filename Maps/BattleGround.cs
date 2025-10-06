using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TXTRPG
{
    internal class BattleGround : Map, IDangerousZone
    {
        private Monster monster;
        private VillageManager villageManager;

        public BattleGround(Monster monsters, VillageManager villageManager)
        {
            monster = monsters;
            this.villageManager = villageManager;
        }

        public void Battle(Player player)
        {
            var battleManager = new BattleManager(player, monster, villageManager);
            battleManager.StartBattleGround(player);
        }

        public override void Enter(Player player)
        {
            Battle(player);
        }
    }
}
