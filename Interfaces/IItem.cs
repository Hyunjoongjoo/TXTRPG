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
        bool Use(Player player);
    }
    public interface IEquippable
    {
        void Equip(Player player);
        void Unequip(Player player);
    }
}
