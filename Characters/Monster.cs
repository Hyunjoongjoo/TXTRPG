using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TXTRPG
{
   
    public abstract class Monster : Character
    {
        //보상관련
        public int ExpReward { get; protected set; }
        public int GoldReward { get; protected set; }
        //스텟관련
        public int ExpBase { get; protected set; }
        public int GoldBase { get; protected set; }
        public Monster(string name) : base(name)
        {
            
        }
        public abstract void Abililty(Character monster, Character player);//몬스터 능력
        //몬스터 레벨당 능력치 조절
        protected void LevelStat()
        {
            Hp += Level * 10;
            MaxHp = Hp;
            BaseAtt += Level * 5;
            BaseDef += Level * 2;
            BaseSpeed += Level * 2;
            ExpReward = ExpBase * Level;
            GoldReward = GoldBase * Level;
        }
    }
    public class Slime : Monster
    {
        public Slime(string name) : base(name)
        {
            Level = GameManager.GetRandom().Next(1, 3);
            Hp = MaxHp = 80;
            BaseAtt = 10;
            BaseDef = 3;
            BaseSpeed = 30;
            ExpBase = 10;
            GoldBase = 10;
            LevelStat();
        }

        public override void Abililty(Character monster, Character player)
        {
            Heal(1);//턴당회복
            Console.WriteLine($"{Name} 패시브 발동! 체력 1회복");
            Console.WriteLine("\nPress the button");
            Console.ReadKey(true);
        }
    }
    public class Goblin : Monster
    {
        public Goblin(string name) : base(name)
        {
            Level = GameManager.GetRandom().Next(1, 3);
            Hp = MaxHp = 100;
            BaseAtt = 25;
            BaseDef = 5;
            BaseSpeed = 35;
            ExpBase = 15;
            GoldBase = 15;
            LevelStat();
        }

        public override void Abililty(Character monster, Character player)
        {
            Heal(2);
            BaseSpeed += 5;
            if (BaseSpeed > 100) { BaseSpeed = 100;}
            Console.WriteLine($"{Name} 패시브 발동! 체력 2회복, 속도가 5증가합니다. \n현재 속도 : {BaseSpeed}");
            Console.WriteLine("\nPress the button");
            Console.ReadKey(true);
        }
    }
    public class Orc : Monster
    {
        public Orc(string name) : base(name)
        {
            Level = GameManager.GetRandom().Next(1, 3);
            Hp = MaxHp = 120;
            BaseAtt = 40;
            BaseDef = 7;
            BaseSpeed = 32;
            ExpBase = 20;
            GoldBase = 20;
            LevelStat();

        }

        public override void Abililty(Character monster, Character player)
        {
            Heal(3);
            BaseAtt += 5;
            if (BaseAtt > 100) { BaseAtt = 100;}
            Console.WriteLine($"{Name} 패시브 발동! 체력 3회복, 공격력이 5증가합니다. \n현재 공격력 : {BaseAtt}");
            Console.WriteLine("\nPress the button");
            Console.ReadKey(true);
        }
    }
    public class Golem : Monster
    {
        public Golem(string name) : base(name)
        {
            Level = GameManager.GetRandom().Next(1, 3);
            Hp = MaxHp = 150;
            BaseAtt = 50;
            BaseDef = 12;
            BaseSpeed = 32;
            ExpBase = 25;
            GoldBase = 25;
            LevelStat();
        }

        public override void Abililty(Character monster, Character player)
        {
            Heal(4);
            BaseDef += 5;
            if (BaseDef > 100) { BaseDef = 100;}
            Console.WriteLine($"{Name} 패시브 발동! 체력 4회복, 방어력이 5증가합니다. \n현재 방어력 : {BaseDef}");
            Console.WriteLine("\nPress the button");
            Console.ReadKey(true);
        }
    }
    public class Doppelganger : Monster
    {
        public Doppelganger(string name) : base(name)
        {
            Level = GameManager.GetRandom().Next(1, 3);
            Hp = MaxHp = 180;
            BaseAtt = 50;
            BaseDef = 10;
            BaseSpeed = 35;
            ExpBase = 30;
            GoldBase = 30;
            LevelStat();
        }

        public override void Abililty(Character monster, Character player)
        {
            Heal(5);
            BaseAtt += player.Att / 6; //플레이어 능력치의 6분의 1만큼 추가
            if (BaseAtt > 180) { BaseAtt = 180;}
            BaseDef += player.Def / 6;
            if (BaseDef > 180) { BaseDef = 180;}
            Console.WriteLine($"{Name} 패시브 발동! 체력 5회복, 공격력,방어력이 플레이어의 능력치에 비례하여 증가합니다.");
            Console.WriteLine($"\n현재 공격력 : {BaseAtt} , 방어력 : {BaseDef}");
            Console.WriteLine("\nPress the button");
            Console.ReadKey(true);
        }
        public class Dragon : Monster
        {
            public Dragon(string name) : base(name)
            {
                Level = GameManager.GetRandom().Next(1, 3);
                Hp = MaxHp = 200;
                BaseAtt = 50;
                BaseDef = 15;
                BaseSpeed = 40;
                ExpBase = 100;
                GoldBase = 100;
                LevelStat();
            }

            public override void Abililty(Character monster, Character player)
            {
                Heal(10);
                BaseAtt += player.Att / 8;
                if (BaseAtt > 200) { BaseAtt = 200; }
                BaseDef += player.Def / 8;
                if (BaseDef > 200) { BaseDef = 200; }
                BaseSpeed += player.Speed / 13;
                if (BaseSpeed > 130) {BaseSpeed = 130;}
                Console.WriteLine($"{Name} 패시브 발동! 체력 10회복, 공격력,방어력,속도가 플레이어의 능력치에 비례하여 증가합니다.");
                Console.WriteLine($"\n현재 공격력 : {BaseAtt} , 방어력 : {BaseDef} , 속도 : {BaseSpeed}");
                Console.WriteLine("\nPress the button");
                Console.ReadKey(true);
            }
        }

    }
}
