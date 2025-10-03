using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TXTRPG
{
   
    public abstract class Monster : Character
    {
        public int ExpReward { get; protected set; }
        public int GoldReward { get; protected set; }
        public Monster(string name) : base(name)
        {
            
        }
        public override void Attack(Character playerTarget) 
        {
            int damage = Damage();
            playerTarget.TakeDamage(damage);
        }
        public abstract void Abililty(Character monster, Character player);//몬스터 능력
        protected void LevelStat()
        {
            Hp += Level * 10;
            MaxHp = Hp;
            Att += Level * 5;
            Def += Level * 2;
            Speed += Level * 2;
            ExpReward = 0 * Level;
            GoldReward = 0 * Level;
        }
    }
    public class Slime : Monster
    {
        public Slime(string name) : base(name)
        {
            Level = GameManager.GetRandom().Next(1, 3);
            Hp = MaxHp = 0;
            Att = 0;
            Def = 0;
            LevelStat();
            
        }

        public override void Abililty(Character monster, Character player)
        {
            Heal(1);//턴당회복
        }
    }
    public class Goblin : Monster
    {
        public Goblin(string name) : base(name)
        {
            Level = GameManager.GetRandom().Next(1, 3);
            Hp = MaxHp = 0;
            Att = 0;
            Def = 0;
            LevelStat();
        }

        public override void Abililty(Character monster, Character player)
        {
            Heal(2);
            Speed += 5;
            if (Speed > 100) {Speed = 100;}
        }
    }
    public class Orc : Monster
    {
        public Orc(string name) : base(name)
        {
            Level = GameManager.GetRandom().Next(1, 3);
            Hp = MaxHp = 0;
            Att = 0;
            Def = 0;
            LevelStat();

        }

        public override void Abililty(Character monster, Character player)
        {
            Heal(3);
            Att += 5;
            if (Att > 100) { Att = 100;}
        }
    }
    public class Golem : Monster
    {
        public Golem(string name) : base(name)
        {
            Level = GameManager.GetRandom().Next(1, 3);
            Hp = MaxHp = 0;
            Att = 0;
            Def = 0;
            LevelStat();
        }

        public override void Abililty(Character monster, Character player)
        {
            Heal(4);
            Def += 5;
            if (Def > 100) {Def = 100;}
        }
    }
    public class Doppelganger : Monster
    {
        public Doppelganger(string name) : base(name)
        {
            Level = GameManager.GetRandom().Next(1, 3);
            Hp = MaxHp = 0;
            Att = 0;
            Def = 0;
            LevelStat();
        }

        public override void Abililty(Character monster, Character player)
        {
            Heal(5);
            Att += player.Att / 2; //플에이어 능력치의 반만큼 추가
            if (Att > 200) { Att = 200;}
            Def += player.Def / 2;
            if (Def > 200) { Def = 200;}
        }
        public class Dragon : Monster
        {
            public Dragon(string name) : base(name)
            {
                Level = GameManager.GetRandom().Next(1, 3);
                Hp = MaxHp = 0;
                Att = 0;
                Def = 0;
                LevelStat();
            }

            public override void Abililty(Character monster, Character player)
            {
                Heal(10);
            }
        }

    }
}
