using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TXTRPG
{
    internal class ShopManager
    {
        private Player player;
        private Shop shop;
        public ShopManager(Player player, Shop shop)
        {
            this.player = player;
            this.shop = shop;
        }
        public void EnterShop(Player player)
        {
            bool isShopping = true;

            while (isShopping)
            {
                Console.Clear();
                Console.WriteLine($"현재 위치 : {shop.Name}");
                Console.WriteLine("\n1. 구매");
                Console.WriteLine("\n2. 판매");
                Console.WriteLine("\n0. 나가기");

                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.D1:
                        ShowBuyMenu(player);
                        break;
                    case ConsoleKey.D2:
                        ShowSellMenu(player);
                        break;
                    case ConsoleKey.D0:
                        isShopping = false;
                        break;
                    default:
                        GameManager.InvalidInput();
                        break;
                }
            }
        }
        //구매 선택지
        private void ShowBuyMenu(Player player)
        {
            Console.Clear();
            Console.WriteLine("구매 메뉴");
            Console.WriteLine("\n1. 무기");
            Console.WriteLine("\n2. 방어구");
            Console.WriteLine("\n3. 포션");
            Console.WriteLine("\n0. 뒤로");

            switch (Console.ReadKey(true).Key)
            {
                case ConsoleKey.D1:
                    BuyItem(ItemRepository.WeaponList);
                    Console.WriteLine("\nPress the button");
                    Console.ReadKey(true);
                    break;
                case ConsoleKey.D2:
                    BuyItem(ItemRepository.ArmorList);
                    Console.WriteLine("\nPress the button");
                    Console.ReadKey(true);
                    break;
                case ConsoleKey.D3:
                    BuyItem(ItemRepository.PotionList);
                    Console.WriteLine("\nPress the button");
                    Console.ReadKey(true);
                    break;
                case ConsoleKey.D0:
                    return;
                default:
                    GameManager.InvalidInput();
                    break;
            }
        }
        //판매 선택지
        private void ShowSellMenu(Player player)
        {
            Console.Clear();
            Console.WriteLine("판매 메뉴");
            Console.WriteLine("\n1. 무기");
            Console.WriteLine("\n2. 방어구");
            Console.WriteLine("\n3. 포션");
            Console.WriteLine("\n0. 뒤로");

            switch (Console.ReadKey(true).Key)
            {
                case ConsoleKey.D1:
                    SellItem<Weapon>();
                    Console.WriteLine("\nPress the button");
                    Console.ReadKey(true);
                    break;
                case ConsoleKey.D2:
                    SellItem<Armor>();
                    Console.WriteLine("\nPress the button");
                    Console.ReadKey(true);
                    break;
                case ConsoleKey.D3:
                    SellItem<Potion>();
                    Console.WriteLine("\nPress the button");
                    Console.ReadKey(true);
                    break;
                case ConsoleKey.D0:
                    return;
                default:
                    GameManager.InvalidInput();
                    break;
            }
        }
        //아이템 구매
        public void BuyItem<T>(List<T> itemList) where T : Item
        {
            Console.Clear();
            Console.WriteLine("아이템 구매\n");

            for (int i = 0; i < itemList.Count; i++)
            {
                T item = itemList[i]; //T 형 아이템
                //각 타입으로 변환
                var w = item as Weapon;
                var a = item as Armor;
                var p = item as Potion;
                //타입에 따라
                if (w != null)
                {
                    Console.WriteLine($"{i + 1}. {w.ToString()}\n");
                }
                else if (a != null)
                {
                    Console.WriteLine($"{i + 1}. {a.ToString()}\n");
                }
                else if (p != null)
                {
                    Console.WriteLine($"{i + 1}. {p.ToString()}\n");
                }
                else
                {
                    Console.WriteLine($"{i + 1}. {item.Name} - {item.Info} - 가격 : {item.Price} G\n");
                }
            }

            Console.WriteLine("0. 뒤로");

            string input = Console.ReadLine();
            int choice;
            if (!(int.TryParse(input, out choice) && choice > 0 && choice <= itemList.Count))
                return;

            T temp = itemList[choice - 1];
            //같은 이름의 장비는 구매불가
            if (!(temp is Potion) && player.InventoryDic.ContainsKey(temp.Name))
            {
                Console.Clear();
                Console.WriteLine("이미 소유 중인 장비입니다!");
                return;
            }
            if (!player.SpendGold(temp.Price))
            {
                Console.Clear();
                Console.WriteLine("골드가 부족합니다.");
                return;
            }
            // 새 인스턴스를 생성해서 플레이어 인벤토리에 추가
            Item boughtItem;
            var weapon = temp as Weapon;
            var armor = temp as Armor;
            var potion = temp as Potion;
            if (weapon != null)
            {
                boughtItem = new Weapon(weapon.Name, weapon.Info, weapon.WearableLevel, weapon.AttPlus, weapon.SpeedMinus, weapon.Price, weapon.Type);
            }
            else if (armor != null)
            {
                boughtItem = new Armor(armor.Name, armor.Info, armor.WearableLevel, armor.DefPlus, armor.SpeedMinus, armor.Price, armor.Type);
            }
            else
            {
                Potion newPotion = new Potion(potion.Name, potion.Info, potion.Price, potion.HealPercent);
                newPotion.Quantity = 1;
                boughtItem = newPotion;
            }
            //포션만 수량 누적
            player.AddItem(boughtItem);
            Console.WriteLine($"\n'{boughtItem.Name}' 구매 완료!");
            Console.WriteLine($"\n남은 골드 : {player.Gold} G");
        }

        //아이템 판매
        public void SellItem<T>() where T : Item
        {
            List<T> inventoryItems = new List<T>();
            foreach (Item item in player.InventoryDic.Values)
            {
                if (item is T && item.Name != "없음")//없음은 제외
                {
                    inventoryItems.Add((T)item);
                }
            }
            if (!inventoryItems.Any())
            {
                Console.WriteLine($"판매할 {typeof(T).Name}이(가) 없습니다.");
                return;
            }

            Console.Clear();
            Console.WriteLine("아이템 판매\n");

            for (int i = 0; i < inventoryItems.Count; i++)
            {
                var item = inventoryItems[i];
                var potionQuantity = item as Potion;

                // 표시는 포션만 xN, 그 외는 1개
                if (potionQuantity != null)
                {
                    Console.WriteLine($"{i + 1} {potionQuantity.Name} x{potionQuantity.Quantity}  - 판매 가격 {(potionQuantity.Price / 2)}G (1개당)\n");
                }
                else
                {
                    Console.WriteLine($"{i + 1} {item.Name} - 판매 가격 {(item.Price / 2)} G\n");
                }
            }

            Console.WriteLine("0. 뒤로");

            // 판매할 아이템 선택
            string input = Console.ReadLine();
            int choice;
            if (!(int.TryParse(input, out choice) && choice > 0 && choice <= inventoryItems.Count))
            {
                return;
            }

            Item selected = inventoryItems[choice - 1];
            int maxSell = 1;

            // 수량이 있는 아이템인지 확인
            if (selected is IQuantity q)
            {
                maxSell = q.Quantity;
            }

            Console.WriteLine("판매할 개수를 입력하세요");
            string cntInput = Console.ReadLine();
            int sellCount;

            if (!(int.TryParse(cntInput, out sellCount) && sellCount > 0 && sellCount <= maxSell))
            {
                Console.WriteLine("잘못된 수량입니다.");
                return;
            }

            // 포션처럼 수량이 있는 아이템 처리
            if (selected is Potion sp)
            {
                sp.Quantity -= sellCount;
                if (sp.Quantity <= 0)
                {
                    if (player.InventoryDic.ContainsKey(sp.Name))
                    {
                        player.InventoryDic.Remove(sp.Name);
                        player.InventoryList.Remove(sp);
                    }
                }
            }
            else
            {
                // 일반 아이템 (무기/방어구 등)
                if (player.InventoryDic.ContainsKey(selected.Name))
                {
                    Item invItem = player.InventoryDic[selected.Name];
                    player.InventoryDic.Remove(selected.Name);
                    player.InventoryList.Remove(invItem);
                }
            }

            // 판매 금액 계산 (1개당 절반 가격)
            int goldGained = Math.Max(1, (int)(selected.Price * sellCount / 2.0));
            player.GainGold(goldGained);

            Console.WriteLine($"\n{selected.Name} x{sellCount}개 판매 완료!");
            Console.WriteLine($"\n{goldGained} G 획득!");
            Console.WriteLine($"\n현재 골드 : {player.Gold} G");
        }
    }
}



