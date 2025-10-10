using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TXTRPG
{
    public static class RandomEx
    {
        public static int NextFix(this Random ran, int min, int max)
        {
            return ran.Next(min, max + 1);
        }
    }
    public class GameManager
    {
        private static Random ran = new Random();
        public static Random GetRandom() { return ran; }
        private Player player;
        private Village village;
        private Shop shop;
        private Monster monster;
        private BattleGround battleGround;
        private BattleManager battleManager;
        private ShopManager shopManager;
        private VillageManager villageManager;
        public GameManager()
        {
            player = new Player("플레이어");
            village = new Village("마을", player);
            shop = new Shop("상점", player);
            villageManager = new VillageManager(village, player);
            shopManager = new ShopManager(player, shop);
            battleManager = new BattleManager(player, monster, villageManager);
            battleGround = new BattleGround(monster, villageManager);
        }

        public void GameStart()
        {
            ChooseEquipment();//시작 장비선택            
            bool isPlaying = true;
            while (isPlaying)
            {
                Console.Clear();
                Console.WriteLine("메인 메뉴");
                Console.WriteLine("\n1. 사냥터");
                Console.WriteLine("\n2. 마을");
                Console.WriteLine("\n3. 상점");
                Console.WriteLine("\n4. 상태창");
                Console.WriteLine("\n0. 종료");

                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.D1:
                        {
                            bool escaped = battleManager.StartBattleGround(player);
                            if (escaped)
                            {
                                break;
                            }

                            break;
                        }
                    case ConsoleKey.D2:
                        {
                            villageManager.EnterVillage(player);
                            break;
                        }
                    case ConsoleKey.D3:
                        {
                            shopManager.EnterShop(player);
                            break;
                        }
                    case ConsoleKey.D4:
                        {
                            player.ShowStatus();
                            break;
                        }
                    case ConsoleKey.D0:
                        {
                            isPlaying = false;
                            break;
                        }
                    default:
                        {
                            InvalidInput();
                            break;
                        }
                }
            }
        }
        private void ChooseEquipment()
        {
            // 레벨 제한 0인 무기와 방어구만 선택 가능
            List<Weapon> startWeapons = new List<Weapon>();
            foreach (Weapon w in ItemRepository.WeaponList)
            {
                if (w.WearableLevel == 0)
                {
                    startWeapons.Add(w);
                }
            }
            List<Armor> startArmors = new List<Armor>();
            foreach (Armor a in ItemRepository.ArmorList)
            {
                if (a.WearableLevel == 0)
                {
                    startArmors.Add(a);
                }
            }

            // 무기 선택
            bool isInvalidInput = true; 
            while (isInvalidInput)
            {
                Console.Clear();
                Console.WriteLine("시작 장비 선택 \n");
                for (int i = 0; i < startWeapons.Count; i++)
                {
                    var w = startWeapons[i];
                    Console.WriteLine($"{i + 1}. {w.Name} - {w.Info} - 공격력 : {w.AttPlus} - 속도감소 : {w.SpeedMinus} \n");
                }

                Console.Write("\n장착할 무기를 선택하세요 (번호 입력): ");
                if (int.TryParse(Console.ReadLine(), out int weaponChoice) && weaponChoice >= 1 && weaponChoice <= startWeapons.Count)
                {
                    Weapon selectedWeapon = startWeapons[weaponChoice - 1];

                    // 인벤토리에 먼저 추가
                    player.Inventory.AddItem(selectedWeapon);

                    // 착용
                    player.EqipItem(selectedWeapon.Name);

                    isInvalidInput = false; //루프탈출
                }
                else
                {
                    Console.Clear();
                    InvalidInput();
                }
            }
            Console.Clear();
            // 방어구 선택
            isInvalidInput = true;
            while (isInvalidInput)
            {
                Console.Clear();
                Console.WriteLine("시작 장비 선택 \n");
                for (int i = 0; i < startArmors.Count; i++)
                {
                    var a = startArmors[i];
                    Console.WriteLine($"{i + 1}. {a.Name} - {a.Info} - 방어력 : {a.DefPlus} - 속도감소 : {a.SpeedMinus} \n");
                }

                Console.Write("\n장착할 방어구를 선택하세요 (번호 입력): ");
                if (int.TryParse(Console.ReadLine(), out int armorChoice) &&
                    armorChoice >= 1 && armorChoice <= startArmors.Count)
                {
                    var selectedArmor = startArmors[armorChoice - 1];

                    player.Inventory.AddItem(selectedArmor);

                    player.EqipItem(selectedArmor.Name);

                    isInvalidInput = false;
                }
                else
                {
                    Console.Clear();
                    InvalidInput();
                }
            }

            // 시작 포션 5개 지급
            Potion potionTemp = null;
            foreach (Potion p in ItemRepository.PotionList)
            {
                if (p.Name == "하급 포션")
                {
                    potionTemp = p;
                    break;
                }
            }
            if (potionTemp != null)
            {
                Potion startPotion = new Potion(potionTemp.Name,potionTemp.Info,potionTemp.Price,potionTemp.HealPercent);
                startPotion.Quantity = 5;
                player.Inventory.AddItem(startPotion);
            }
            Console.Clear();
            Console.WriteLine("장비 선택 완료!");
            Console.WriteLine("\n시작 장비 , 10 G , 하급 포션 5개를 가지고 시작합니다.");
            Console.WriteLine("\nPress the button");
            Console.ReadKey(true);

        }     
        public static void InvalidInput()
        {
            Console.Clear();
            Console.WriteLine("메뉴에 있는 버튼을 선택하세요");
            Console.WriteLine("\nPress the button");
            Console.ReadKey(true);
        }
    }
}
