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
        void Use(Character player);
    }
    public interface IEquippable
    {

        void Equip(Character player);
        void Unequip(Character player);
    }
    public interface ISpeed
    {
        void AttSpeed();
    }
    public abstract class Item
    {
        public string Name { get; protected set; }
        public int Price { get; protected set; }

    }
}
