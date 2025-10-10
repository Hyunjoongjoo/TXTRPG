using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace TXTRPG
{
    //플레이어에서 분리함(분리해야지했는데 깜빡하고 안함)
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
                if (item is Potion p && p.Quantity > 0)
                    potions.Add(p);
            }

            if (potions.Count == 0)
            {
                Console.Clear();
                Console.WriteLine("사용 가능한 포션이 없습니다!");
                Console.WriteLine("\nPress the button");
                Console.ReadKey(true);
                return false;
            }

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

            if (!int.TryParse(input, out int choice) || choice < 0 || choice > potions.Count)
            {
                GameManager.InvalidInput();
                return false;
            }
            if (choice == 0) return false;

            Potion selectedPotion = potions[choice - 1];
            bool used = selectedPotion.Use(player);

            if (used && selectedPotion.Quantity <= 0)
            {
                items.Remove(selectedPotion.Name);
                itemList.Remove(selectedPotion);
            }

            return used;
        }
        //인벤토리에 아이템 추가,제거
        public void AddItem(Item item)
        {
            if (item is IQuantity itemQuan)
            {
                if (items.ContainsKey(item.Name))
                {
                    IQuantity existing = (IQuantity)Items[item.Name];
                    int addQuantity = itemQuan.Quantity;
                    int newQuantity = existing.Quantity + addQuantity;

                    //상한선 적용
                    if (item is Potion)
                    {
                        if (newQuantity > Potion.MaxQuantity)
                        {
                            existing.Quantity = Potion.MaxQuantity;
                            Console.WriteLine($"\n'{item.Name}은 최대 {Potion.MaxQuantity}개까지만 보유할 수 있습니다!");
                            Console.WriteLine("\nPress the button");
                            Console.ReadKey(true);
                        }
                        else
                        {
                            existing.Quantity = newQuantity;
                        }
                    }
                    else
                    {
                        existing.Quantity = newQuantity;
                    }
                }
                else
                {
                    items[item.Name] = item;
                    itemList.AddLast(item);
                }
            }
            else //IQuantity가 아닌 장비류
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
            if (item is IQuantity itemQuan)
            {
                if (itemQuan.Quantity <= 0 && items.ContainsKey(item.Name))
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

                if (item is IQuantity itemQuan)
                {
                    if (itemCounts.ContainsKey(item.Name))
                        itemCounts[item.Name] += itemQuan.Quantity;
                    else
                        itemCounts[item.Name] = itemQuan.Quantity;
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
