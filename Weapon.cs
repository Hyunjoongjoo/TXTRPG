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
    public class Weapon : Item, IEquippable, ISpeed , IWearableLevel , IQuantity
    {
        public int AttPlus { get; private set; }
        public WeaponType Type { get; private set;}
        public float WeaponSpeed { get; private set; }
        public int WearableLevel { get; private set; }
        public int Quantity { get; set; } = 1;
        public int MaxQuantity { get; private set; } = 5;

        public Weapon(WeaponType type)
        {
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

        public void AttSpeed()
        {
            
        }

        public void Equip(Character player)
        {
            
        }

        public void Unequip(Character player)
        {
            
        }
    }
}
