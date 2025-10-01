using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TXTRPG
{
    class Player : Character
    {
        
        public int Level { get; private set; }
        public int Exp { get; private set; }
        public int MaxExp { get; private set; }
        public int Gold { get; private set; }
        public Dictionary<string, Item> InventoryDic { get; private set; } //아이템 검색
        public LinkedList<Item> InventoryList { get; private set; } //아이템 순서 관리
        public Player(string name) : base(name)
        {
            
            Level = 1;
            Exp = 0;
            MaxExp = 100;
            Gold = 10;
            Hp = 100;
            MaxHp = 100;
            Att = 20;

        }
        protected override int Damage()
        {
            Random ran = GameManager.GetRandom();
            float damage = Att;
            if (ran.NextDouble() < CriChance)
            {
                damage *= CriMultiplier;
                Console.WriteLine("크리티컬 히트!");
            }
            return (int)damage;
        }
        public override void Attack(Character target)
        {
            int damage = Damage();
            target.TakeDamage(damage);
        }
    }
}
