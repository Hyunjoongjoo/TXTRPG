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
        //포션 사용
        public bool UseItem()
        {
            List<Potion> potions = new List<Potion>();
            foreach (Item item in items.Values)
            {
                Potion potion = item as Potion;
                if (potion != null && potion.Quantity > 0)
                {
                    potions.Add(potion);
                }
            }
            //포션 선택 UI
            Console.Clear();
            Console.WriteLine("*** 사용 가능한 포션 목록 ***\n");

            for (int i = 0; i < potions.Count; i++)
            {
                Potion p = potions[i];
                Console.WriteLine($"{i + 1}. {p.Name} (x{p.Quantity}) - 회복량: {p.HealPercent * 100}%");
            }

            Console.WriteLine("\n0. 취소");
            Console.Write("\n사용할 포션 번호를 입력하세요: ");
            string input = Console.ReadLine();

            if (!int.TryParse(input, out int choice))
            {
                GameManager.InvalidInput();
                return false;
            }

            if (choice == 0)
            {
                Console.Clear();
                Console.WriteLine("포션 사용을 취소했습니다.");
                return false;
            }

            if (choice < 1 || choice > potions.Count)
            {
                GameManager.InvalidInput();
                return false;
            }

            //선택된 포션 사용
            Potion selectedPotion = potions[choice - 1];
            int beforeQuantity = selectedPotion.Quantity;
            selectedPotion.Use(player);

            bool used = selectedPotion.Use(player);
            if (!used)
            {
                return false; // 턴 유지
            }
            //수량이 0이면 인벤토리에서 제거
            if (selectedPotion.Quantity <= 0 && beforeQuantity > 0)
            {
                items.Remove(selectedPotion.Name);
                itemList.Remove(selectedPotion);
            }
            return true;
        }
        //인벤토리에 아이템 추가,제거
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
        //인벤토리 아이템 확인용
        public void ShowInventory()
        {
            Dictionary<string, int> itemCounts = new Dictionary<string, int>();

            foreach (Item item in itemList)
            {
                //현재 장착 중인 장비는 인벤토리에서 출력 제외
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
