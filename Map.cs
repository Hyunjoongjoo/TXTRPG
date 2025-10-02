using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TXTRPG
{
    public interface ISafeZone
    {

    }
    public interface IDangerousZone
    {

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
            throw new NotImplementedException();
        }
    }
    public class Shop : Map, ISafeZone
    {
        public override void Enter(Player player)
        {
            throw new NotImplementedException();
        }
    }
    public class BattleGround : Map, IDangerousZone
    {
        public override void Enter(Player player)
        {
            throw new NotImplementedException();
        }
    }
}
