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
        public int Level { get; protected set; } = 1;
        public int Hp { get; protected set; }
        public int MaxHp { get; protected set; }
        public int Att { get; protected set; }
        public int Def { get; protected set; }
        public int Speed { get; protected set; } = 10;
        public float CriChance { get; protected set; } = 0.1f; //크리티컬 확률 
        public float CriMultiplier { get; protected set; } = 1.25f;//크리티컬 배율
        protected Character(string name)
        {
            Name = name;
        }
        public void Heal(int healAmount)//회복
        {
            Hp += healAmount;
            if (Hp > MaxHp) {  MaxHp = Hp; }
        }
        protected int Damage()//공격력
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
        public void TakeDamage(int damage)//최종 데미지 계산
        {
            int finalDamage = damage - Def;
            if (finalDamage < 0)
            {
                finalDamage = 0;
            }
            Hp -= finalDamage;
            if (Hp < 0) { Hp = 0; }
        }
        public abstract void Attack(Character target);
    }
}
