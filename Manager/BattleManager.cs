using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static TXTRPG.Doppelganger;

namespace TXTRPG
{
    internal class BattleManager
    {
        private Player player; 
        private Monster monster;
        private Village village;//도망 성공시 돌아갈 마을
        private bool escape;//도망의 성공 여부
        private VillageManager villageManager;//도망,패배시 마을로 복귀처리용 

        public BattleManager(Player player, Monster monster, VillageManager villageManager)
        {
            this.player = player;
            this.monster = monster;
            this.escape = false;
            this.villageManager = villageManager;
        }

        public void StartBattleGround(Player player)
        {
            bool isBattling = true;

            while (isBattling)
            {
                Console.Clear();
                Console.WriteLine("사냥터 선택 :");
                Console.WriteLine("\n1. 울창한 숲 (슬라임)");
                Console.WriteLine("\n2. 어두운 동굴 (고블린)");
                Console.WriteLine("\n3. 오크 군락지 (오크)");
                Console.WriteLine("\n4. 화산지대 (골렘)");
                Console.WriteLine("\n5. 거울 호수 (도플갱어)");
                Console.WriteLine("\n6. 용의 둥지 (드래곤)");
                Console.WriteLine("\n0. 사냥터 나가기");

                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.D1:
                        StartBattle(new Slime("슬라임"));
                        break;
                    case ConsoleKey.D2:
                        StartBattle(new Goblin("고블린"));
                        break;
                    case ConsoleKey.D3:
                        StartBattle(new Orc("오크"));
                        break;
                    case ConsoleKey.D4:
                        StartBattle(new Golem("골렘"));
                        break;
                    case ConsoleKey.D5:
                        StartBattle(new Doppelganger("도플갱어"));
                        break;
                    case ConsoleKey.D6:
                        StartBattle(new Dragon("드래곤"));
                        break;
                    case ConsoleKey.D0:
                        Console.Clear();
                        isBattling = false;
                        break;
                    default:
                        GameManager.InvalidInput();
                        break;
                }
            }
        }
        public void StartBattle(Monster monster)
        {
            Console.Clear();
            Console.WriteLine($"전투 시작! \n\n적 : {monster.Name} Lv {monster.Level}");
            Console.WriteLine($"\n적의 체력 : {monster.Hp}");
            Console.WriteLine($"\n플레이어 속도 : {player.Speed}, 적 속도 : {monster.Speed}");
            Console.WriteLine("\nPress the button");
            Console.ReadKey(true);
            escape = false; //도망 성공 여부 초기화
            while (player.Hp > 0 && monster.Hp > 0 && !escape)
            {
                //턴 순서 결정 (속도 비교)
                bool playerFirst = player.Speed >= monster.Speed;

                if (playerFirst)
                {
                    // 플레이어가 선공일때
                    if (PlayerTurn(monster, ref escape))
                    {
                        if (escape)
                        {
                            villageManager.EnterVillage(player);
                            return;
                        }

                        if (monster.Hp <= 0)
                        {
                            Console.Clear();
                            Console.WriteLine($"{monster.Name} 처치!");
                            player.GetReward(monster);
                            Console.WriteLine("\nPress the button");
                            Console.ReadKey(true);
                            return;
                        }
                    }

                    // 그다음 몬스터 턴
                    if (monster.Hp > 0)
                    {
                        MonsterTurn(monster);
                        if (player.Hp <= 0)
                        {
                            Console.Clear();
                            Console.WriteLine($"{player.Name}가 패배했습니다...");
                            Console.WriteLine("\nPress the button");
                            Console.ReadKey(true);
                            player.Heal(1);
                            villageManager.EnterVillage(player);
                            return;
                        }
                    }
                }
                else
                {
                    //몬스터가 선공일때
                    MonsterTurn(monster);
                    if (player.Hp <= 0)
                    {
                        Console.Clear();
                        Console.WriteLine($"{player.Name}가 패배했습니다...\n플레이어가 마을에서 깨어납니다.");
                        Console.WriteLine("\nPress the button");
                        Console.ReadKey(true);
                        villageManager.EnterVillage(player);
                        return;
                    }

                    // 그다음 플레이어 턴
                    if (monster.Hp > 0)
                    {
                        PlayerTurn(monster, ref escape);
                        if (escape)
                        {
                            villageManager.EnterVillage(player);
                            return;
                        }

                        if (monster.Hp <= 0)
                        {
                            Console.Clear();
                            Console.WriteLine($"{monster.Name} 처치!");
                            player.GetReward(monster);
                            Console.WriteLine("\nPress the button");
                            Console.ReadKey(true);
                            return;
                        }
                    }
                }
            }
        }
        private bool PlayerTurn(Monster monster, ref bool escape)
        {
            bool invalidInput = true;
            while (invalidInput)
            {
                Console.Clear();
                ShowInfo(monster);
                Console.WriteLine("\n1. 공격  \n\n2. 포션 사용  \n\n3. 도망");
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.D1:
                        player.Attack(monster);
                        invalidInput = false;
                        return true;

                    case ConsoleKey.D2:
                        Potion potion = null;
                        foreach (Item item in player.InventoryDic.Values)
                        {
                            if (item is Potion p && p.Quantity > 0)
                            {
                                potion = p;
                                break;
                            }
                        }
                        if (potion != null)
                        {
                            player.UseItem(potion);
                            Console.ReadKey(true);
                            invalidInput = false;
                            return true;
                        }
                        else
                        {
                            break;
                        }

                    case ConsoleKey.D3:
                        escape = player.TryEscape();
                        invalidInput = false;
                        return true;

                    default:
                        GameManager.InvalidInput();
                        break;
                }
            }
         return false;
        }

        private void MonsterTurn(Monster monster)
        {
            Console.Clear();

            //어빌리티 적용
            monster.Abililty(monster, player);

            // 공격
            if (monster.Hp > 0)
            {
                monster.Attack(player);
            }
        }
        public void ShowInfo(Monster monster)//전투 상황 표시용 메서드
        {
            Console.WriteLine($"적 : {monster.Name} Lv {monster.Level}");
            Console.WriteLine($"\n적의 체력 : {monster.Hp}");
            Console.WriteLine($"\n플레이어의 체력 : {player.Hp}");
        }
    }
}
