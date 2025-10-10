using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TXTRPG
{
    //플레이어에서 분리함
    public class Inventory
    {
        private Player player;
        private Dictionary<string, Item> items;
        private LinkedList<Item> itemList;
        public Dictionary<string, Item> Items { get { return items; } }
        public LinkedList<Item> ItemList { get { return itemList; } }
        public Inventory(Player player)
        {
            this.player = player;
            items = new Dictionary<string, Item>();
            itemList = new LinkedList<Item>();
        }
        public void UseItem(string itemName)
        {
            if (!items.ContainsKey(itemName))
            {
                return;
            }

            Item item = items[itemName];
            if (items[itemName] is Potion potion)
            {
                int beforeQuan = potion.Quantity;
                potion.Use(player);
                if (potion.Quantity <= 0 && beforeQuan > 0)
                {
                    items.Remove(potion.Name);
                    itemList.Remove(potion);
                }
            }
        }
        public void AddItem(Item item)
        {
            if (item is IQuantity qItem)
            {
                if (items.ContainsKey(item.Name))
                {
                    ((IQuantity)items[item.Name]).Quantity += qItem.Quantity;
                }
                else
                {
                    items[item.Name] = item;
                    itemList.AddLast(item);
                }
            }
            else
            {
                if (!items.ContainsKey(item.Name))
                {
                    items[item.Name] = item;
                    itemList.AddLast(item);
                }
            }
        }
        public void RemoveItem(Item item)
        {
            if (item is IQuantity qItem)
            {
                if (qItem.Quantity <= 0 && items.ContainsKey(item.Name))
                {
                    items.Remove(item.Name);
                    itemList.Remove(item);
                }
            }
            else
            {
                if (items.ContainsKey(item.Name))
                {
                    items.Remove(item.Name);
                    itemList.Remove(item);
                }
            }
        }
        public void ShowInventory()
        {
            Dictionary<string, int> itemCounts = new Dictionary<string, int>();

            foreach (Item item in itemList)
            {
                // 현재 장착 중인 장비는 인벤토리에서 출력 제외
                if (item == player.WeaponEqip || item == player.ArmorEqip)
                {
                    continue;
                }

                if (item is IQuantity qItem)
                {
                    if (itemCounts.ContainsKey(item.Name))
                        itemCounts[item.Name] += qItem.Quantity;
                    else
                        itemCounts[item.Name] = qItem.Quantity;
                }
                else
                {
                    if (itemCounts.ContainsKey(item.Name))
                        itemCounts[item.Name]++;
                    else
                        itemCounts[item.Name] = 1;
                }
            }

            if (itemCounts.Count == 0)
            {
                Console.WriteLine("- (비어 있음)");
                return;
            }

            foreach (var items in itemCounts)
            {
                if (items.Value > 1)
                    Console.WriteLine($"- {items.Key} x{items.Value}");
                else
                    Console.WriteLine($"- {items.Key}");
            }
        }
        public bool HasItem(string name)
        {
            return items.ContainsKey(name);
        }

        public Item GetItem(string name)
        {
            if (items.ContainsKey(name))
                return items[name];
            return null;
        }


    }
}
