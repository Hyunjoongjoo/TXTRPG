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
    class Weapon : Item, IEquippable, ISpeed
    {
        public int AttPlus { get; private set; }
        public WeaponType Type { get; private set;}
        public float WeaponSpeed { get; private set; }

        public void AttSpeed()
        {
            
        }

        public void Equip()
        {
            
        }

        public void Unequip()
        {
            
        }
    }
}
