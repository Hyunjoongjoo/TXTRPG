using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TXTRPG
{
    public enum ArmorType 
    { 
        Helmet, Chest, Gloves, Boots, Shield 
    }
    class Armor : Item , IEquippable
    {
        public int DefPlus {  get; private set; }

        public void Equip()
        {
            
        }

        public void Unequip()
        {
            
        }
    }
}
