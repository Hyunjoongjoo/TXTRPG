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
        void Save(Player player);
    }
    public interface IDangerousZone
    {
        void Battle(Player player);
    }
    public abstract class Map
    {
        protected string Name {  get; set; }
        public abstract void Enter(Player player);
    }
    public class Village : Map, ISafeZone
    {
        public override void Enter(Player player)
        {
            
        }

        public void Rest(Player player)
        {
            
        }

        public void Save(Player player)
        {
            
        }

        public void Trade(Player player)
        {
            
        }
    }
    public class Shop : Map, ISafeZone
    {
        public override void Enter(Player player)
        {
            
        }
        public void Save(Player player)
        {
            
        }

        public void Trade(Player player)
        {
            
        }
    }
    public class BattleGround : Map, IDangerousZone
    {
        public void Battle(Player player)
        {
            
        }

        public override void Enter(Player player)
        {
            
        }
    }
}
