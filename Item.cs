using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TXTRPG
{
    public interface IQuantity
    {
        int Quantity { get; set; }
        int MaxQuantity { get; }
    }
    public interface IWearableLevel
    {
        int WearableLevel { get; }
    }
    public interface IUsable
    {
        void Use(Player player);
    }
    public interface IEquippable
    {
        void Equip(Player player);
        void Unequip(Player player);
    }
    
    public abstract class Item
    {
        public string Name { get; protected set; }
        public string Info { get; protected set; }
        public int Price { get; protected set; }
        public Item(string name, string info, int price)
        {
            Name = name;
            Info = info;
            Price = price;
        } 
        public override string ToString()
        {
            return ($"{Name} - {Info} - {Price}G");
        }

    }
}
