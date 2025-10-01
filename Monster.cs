using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TXTRPG
{
    class Monster : Character
    {
        public int ExpReward { get; private set; }
        public int GoldReward { get; private set; }
        public Monster(string name) : base(name)
        {
            
        }
        public override void Attack(Character target)
        {
            int damage = Damage();
            target.TakeDamage(damage);
        }
    }
}
