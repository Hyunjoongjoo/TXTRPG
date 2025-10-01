using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TXTRPG
{
    public interface IUsable
    {
        void Use();
    }
    public interface IEquippable
    {
        void Equip();
        void Unequip();
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
