using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TXTRPG
{
   
    
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
