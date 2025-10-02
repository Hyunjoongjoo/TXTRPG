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
    public class Armor : Item , IEquippable , IWearableLevel , IQuantity
    {
        public int DefPlus {  get; private set; }
        public int SpeedMinus { get; private set; }
        public int WearableLevel { get; private set; }
        public int Quantity { get; set; } = 1;
        public int MaxQuantity { get; private set; } = 5;

        public void Equip(Character player)
        {
            
        }

        public void Unequip(Character player)
        {
            
        }
    }
}
