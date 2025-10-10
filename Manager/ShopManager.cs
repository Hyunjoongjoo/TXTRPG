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
            Console.WriteLine("*** 구매 메뉴 ***");
            Console.WriteLine("\n1. 무기");
            Console.WriteLine("\n2. 방어구");
            Console.WriteLine("\n3. 포션");
            Console.WriteLine("\n0. 뒤로");

            switch (Console.ReadKey(true).Key)
            {
                case ConsoleKey.D1:
                    BuyItem(ItemRepository.WeaponList);
                    break;
                case ConsoleKey.D2:
                    BuyItem(ItemRepository.ArmorList);
                    break;
                case ConsoleKey.D3:
                    BuyItem(ItemRepository.PotionList);                    
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
            Console.WriteLine("*** 판매 메뉴 ***");
            Console.WriteLine("\n1. 무기");
            Console.WriteLine("\n2. 방어구");
            Console.WriteLine("\n3. 포션");
            Console.WriteLine("\n0. 뒤로");

            switch (Console.ReadKey(true).Key)
            {
                case ConsoleKey.D1:
                    SellItem<Weapon>();
                    break;
                case ConsoleKey.D2:
                    SellItem<Armor>();
                    break;
                case ConsoleKey.D3:
                    SellItem<Potion>();
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
            bool isShopping = true;
            while (isShopping)
            {
                Console.Clear();
                Console.WriteLine("*** 아이템 구매 ***\n");

                for (int i = 0; i < itemList.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {itemList[i]}\n");
                }

                Console.WriteLine("0. 뒤로");
                Console.WriteLine($"\n현재 골드 : {player.Gold} G");

                Console.Write("\n구매할 번호를 입력하세요: ");
                string input = Console.ReadLine();

                if (!(int.TryParse(input, out int choice)))
                {
                    GameManager.InvalidInput();
                    continue;
                }

                if (choice == 0)
                {
                    // 뒤로가기
                    isShopping = false;
                    continue;
                }

                if (choice < 0 || choice > itemList.Count)
                {
                    GameManager.InvalidInput();
                    continue;
                }

                T temp = itemList[choice - 1];
                //같은 이름의 장비는 구매불가,착용 중인 무기 포함
                if ((!(temp is Potion) && player.Inventory.Items.ContainsKey(temp.Name)) || (player.WeaponEqip.Name == temp.Name) || (player.ArmorEqip.Name == temp.Name))
                {
                    Console.WriteLine("\n이미 소유 중인 장비입니다!");
                    Console.WriteLine("\nPress the button");
                    Console.ReadKey(true);
                    continue;
                }
                if (temp is Potion potion)
                {
                    // 현재 보유 수량
                    int current = 0;
                    if (player.Inventory.HasItem(potion.Name))
                        current = ((Potion)player.Inventory.GetItem(potion.Name)).Quantity;

                    if (current >= Potion.MaxQuantity)
                    {
                        Console.WriteLine($"\n이미 '{potion.Name}'이(가) 최대 보유량({Potion.MaxQuantity})입니다!");
                        Console.WriteLine("\nPress the button");
                        Console.ReadKey(true);
                        continue;
                    }

                    Console.Write($"\n몇 개를 구매하시겠습니까? ");
                    if (!int.TryParse(Console.ReadLine(), out int quantity) || quantity <= 0)
                    {
                        Console.WriteLine("\n잘못된 수량입니다.");
                        Console.WriteLine("\nPress the button");
                        Console.ReadKey(true);
                        continue; //여기서 바로 루프 재시작 → 금액 차감 없음
                    }

                    // 상한선 조정
                    if (current + quantity > Potion.MaxQuantity)
                    {
                        quantity = Potion.MaxQuantity - current;
                        Console.WriteLine($"\n{Potion.MaxQuantity}개까지만 구매할 수 있습니다. ({quantity}개로 조정됨)");
                        Console.WriteLine("\nPress the button");
                        Console.ReadKey(true);
                        if (quantity <= 0) continue;
                    }
                    // 총 가격 계산
                    int totalPrice = temp.Price * quantity;
                    if (!player.SpendGold(totalPrice))
                    {
                        Console.WriteLine($"\n골드가 부족합니다! (필요: {totalPrice}G, 현재: {player.Gold}G)");
                        Console.WriteLine("\nPress the button");
                        Console.ReadKey(true);
                        continue;
                    }
                    // 새 인스턴스를 생성해서 플레이어 인벤토리에 추가
                    Item boughtItem;
                    if (temp is Weapon w)
                    {
                        boughtItem = new Weapon(w.Name, w.Info, w.WearableLevel, w.AttPlus, w.SpeedMinus, w.Price, w.Type);
                    }
                    else if (temp is Armor a)
                    {
                        boughtItem = new Armor(a.Name, a.Info, a.WearableLevel, a.DefPlus, a.SpeedMinus, a.Price, a.Type);
                    }
                    else if (temp is Potion p)
                    {
                        boughtItem = new Potion(p.Name, p.Info, p.Price, p.HealPercent)
                        {
                            Quantity = quantity
                        };
                    }
                    else
                    {
                        Console.WriteLine("잘못된 아이템 타입입니다.");
                        Console.WriteLine("\nPress the button");
                        Console.ReadKey(true);
                        return;
                    }
                    //포션만 수량 누적
                    player.Inventory.AddItem(boughtItem);
                    Console.WriteLine($"\n'{boughtItem.Name}' 구매 완료!");
                    Console.WriteLine($"\n남은 골드 : {player.Gold} G");
                    Console.WriteLine("\nPress the button");
                    Console.ReadKey(true);
                }
            }
        }

        //아이템 판매
        public void SellItem<T>() where T : Item
        {
            bool isShopping = true;
            while (isShopping)
            {
                List<T> inventoryItems = new List<T>();
                foreach (Item item in player.Inventory.Items.Values)
                {
                    if (item is T)
                    {
                        inventoryItems.Add((T)item);
                    }
                }
                if (!inventoryItems.Any())
                {
                    Console.WriteLine($"\n판매 가능한 아이템이 없습니다.");
                    Console.WriteLine("\nPress the button");
                    Console.ReadKey(true);
                    return;
                }

                Console.Clear();
                Console.WriteLine("*** 아이템 판매 ***\n");

                for (int i = 0; i < inventoryItems.Count; i++)
                {
                    var item = inventoryItems[i];
                    Potion potionQuan = item as Potion;//포션타입으로 변경된다면

                    //xN개로 표시
                    if (potionQuan != null)
                    {
                        Console.WriteLine($"{i + 1}. {potionQuan.Name} x{potionQuan.Quantity}  - 판매 가격 {(potionQuan.Price / 2)}G (1개당)\n");
                    }
                    else
                    {
                        Console.WriteLine($"{i + 1}. {item.Name} - 판매 가격 {(item.Price / 2)} G\n");
                    }
                }

                Console.WriteLine("0. 뒤로");
                Console.WriteLine($"\n현재 골드 : {player.Gold} G");
                Console.Write("\n판매할 번호를 입력하세요: ");

                //판매할 아이템 선택
                string input = Console.ReadLine();
                if (!int.TryParse(input, out int choice))
                {
                    GameManager.InvalidInput();
                    continue;
                }

                if (choice == 0)
                {
                    // 나가기
                    isShopping = false;
                    continue;
                }

                if (choice < 0 || choice > inventoryItems.Count)
                {
                    GameManager.InvalidInput();
                    continue;
                }

                Item selected = inventoryItems[choice - 1];
                int maxSell = 1;

                //수량이 있는 아이템인지 확인
                if (selected is IQuantity quan)
                {
                    maxSell = quan.Quantity;
                }

                Console.WriteLine("\n판매할 개수를 입력하세요");
                string sellInput = Console.ReadLine();

                if (!(int.TryParse(sellInput, out int sellCount) && sellCount > 0 && sellCount <= maxSell))
                {
                    Console.WriteLine("\n잘못된 수량입니다.");
                    Console.WriteLine("\nPress the button");
                    Console.ReadKey(true);
                    continue;
                }

                //수량이 있는 아이템 처리
                if (selected is Potion potion)
                {
                    potion.Quantity -= sellCount;
                    if (potion.Quantity <= 0)
                    {
                        player.Inventory.RemoveItem(potion);
                    }
                }
                else
                {
                    //장비
                    player.Inventory.RemoveItem(selected);
                }

                // 판매 금액 계산 (1개당 절반 가격)
                int goldGained = Math.Max(1, (int)(selected.Price * sellCount / 2.0));
                player.GainGold(goldGained);

                Console.WriteLine($"\n{selected.Name} x{sellCount}개 판매 완료!");
                Console.WriteLine($"\n{goldGained} G 획득!");
                Console.WriteLine($"\n현재 골드 : {player.Gold} G");
                Console.WriteLine("\nPress the button");
                Console.ReadKey(true);

            }
        }
    }
}



