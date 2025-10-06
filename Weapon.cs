using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TXTRPG
{
    public enum WeaponType
    {
        Sword, Bow, Axe, Staff
    }
    public class Weapon : Item, IEquippable, IWearableLevel , IQuantity
    {
        public int AttPlus { get; private set; }
        public WeaponType Type { get; private set;}
        public int WeaponSpeed { get; private set; }
        public int WearableLevel { get; private set; }
        public int Quantity { get; set; } = 1;
        public int MaxQuantity { get; private set; } = 5;

        public Weapon(string name, string info, int attPlus, int weaponSpeed, int price, WeaponType type) : base(name,info,price)
        {
            AttPlus = attPlus;
            WeaponSpeed = weaponSpeed;
            Price = price;
            Type = type;
            switch(type)
            {
                case WeaponType.Sword:
                    break;
                case WeaponType.Bow:
                    break;
                case WeaponType.Axe:
                    break;
                case WeaponType.Staff:
                    break;
            }
        }
        public void Equip(Player player)
        {
            player.IncreaseAtt(AttPlus); //공격력 보정
            player.DecreaseSpeed(WeaponSpeed); //속도 감소
        }

        public void Unequip(Player player)
        {
            player.IncreaseAtt(-AttPlus); //복구
            player.DecreaseSpeed(-WeaponSpeed); 
        }
    }
}
