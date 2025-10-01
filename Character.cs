using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace TXTRPG
{
    public abstract class Character
    {
        public string Name { get; set; }
        public int Hp { get; protected set; }
        public int MaxHp { get; protected set; }
        public int Att { get; protected set; }
        public int Def { get; protected set; }
        public float Speed { get; protected set; }
        public float CriChance { get; protected set; }//크리티컬 확률
        public float CriMultiplier { get; protected set; }//크리티컬 배율
        protected Character(string name)
        {
            Name = name;
        }
        protected virtual int Damage()
        {
            Random ran = GameManager.GetRandom();
            float damage = Att;
            if (ran.NextDouble() < CriChance)
            {
                damage *= CriMultiplier;
                Console.WriteLine("크리티컬!");
            }

            return (int)damage;
        }
        
        public abstract void Attack(Character target);

        public void TakeDamage(int damage)
        {
            Hp -= damage;
            if (Hp < 0) { Hp = 0; }
        }
    }
}
