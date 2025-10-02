using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TXTRPG
{
    public abstract class Monster : Character
    {
        public int ExpReward { get; private set; }
        public int GoldReward { get; private set; }
        public Monster(string name) : base(name)
        {
            
        }
        public override void Attack(Character playerTarget) 
        {
            int damage = Damage();
            playerTarget.TakeDamage(damage);
        }
    }
    public class Slime : Monster
    {
        public Slime(string name) : base(name)
        {
            
        }
    }
    public class Goblin : Monster
    {
        public Goblin(string name) : base(name)
        {
            
        }
    }
    public class Orc : Monster
    {
        public Orc(string name) : base(name)
        {
            
        }
    }
    public class Golem : Monster
    {
        public Golem(string name) : base(name)
        {
            
        }
    }
    public class Doppelganger : Monster
    {
        public Doppelganger(string name) : base(name)
        {
            
        }
    }
    public class Dragon : Monster
    {
        public Dragon(string name) : base(name)
        {
            
        }
    }


}
