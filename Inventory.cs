using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TXTRPG
{
    class Inventory
    {
        private Dictionary<string,Item> items = new Dictionary<string,Item>();

        public void AddItem(Item item) //아이템추가
        {
            if (items.ContainsKey(item.Name)) //만약 같은 이름이라면
            {
                if(item is IQuantity quanItem) //IQuantity타입으로 변환이 가능한가,같은 타입인가
                {
                    quanItem.Quantity++; //수량을 +1
                }
            }
            else  
            {
                items[item.Name] = item;  //아이템을 저장
            }
            
        }
        public void RemoveItem(string itemName)
        {
            if (items.ContainsKey(itemName))
            {
                Item item = items[itemName];
                if (item is IQuantity quanItem) 
                {
                    quanItem.Quantity--; 
                    if (quanItem.Quantity <= 0)
                    {
                        items.Remove(itemName);
                    }
                }
                else
                {
                    items.Remove(itemName); //수량이 없는 것을 추가할수도...
                }
            }
            else
            {
                Console.WriteLine($"{itemName}은 인벤토리에 없습니다.");
            }
        }
    }
}
