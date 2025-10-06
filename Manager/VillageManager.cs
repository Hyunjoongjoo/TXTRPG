using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TXTRPG
{
    internal class VillageManager : IVillageManager
    {
        private Village village;
        private Player player;
        public VillageManager(Village village, Player player)
        {
            this.village = village;
            this.player = player;
        }
        public void EnterVillage(Player player)
        {
            bool inVillage = true;
            while (inVillage)
            {
                Console.Clear();
                Console.WriteLine($"현재 위치 : {village.Name}");
                Console.WriteLine("\n1. 휴식");
                Console.WriteLine("\n2. 거래");
                Console.WriteLine("\n0. 메인으로");
                
                switch(Console.ReadKey(true).Key)
                {
                    case ConsoleKey.D1:
                        {
                            RestUI();
                            break;
                        }
                    case ConsoleKey.D2:
                        {
                            TradeUI();
                            break;
                        }
                    case ConsoleKey.D0:
                        {
                            inVillage = false;
                            break;
                        }
                    default:
                        {
                            GameManager.InvalidInput();
                            break;
                        }
                }

            }
        }
        public void RestUI()
        {
            Console.Clear();
            int cost = 50;
            if (player.SpendGold(cost)) //골드가 충분하면
            {
                village.Rest(player);
                Console.WriteLine($"{cost}G를 사용");
                Console.WriteLine("\n휴식을 취합니다, Hp가 모두 회복되었습니다.");
                Console.WriteLine($"\n현재 남은 골드 : {player.Gold}G");
            }
            else
            {
                Console.Clear();
                Console.WriteLine("골드가 부족합니다.");
            }
            Console.WriteLine("\nPress the button");
            Console.ReadKey(true);
        }
        public void TradeUI()
        {
            Shop shop = new Shop("상점", player);
            var shopManager = new ShopManager(player, shop);
            shopManager.EnterShop(player);
        }
    }
}
