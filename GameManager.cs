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
    internal class GameManager
    {
        private static Random ran = new Random();
        public static Random GetRandom() { return ran; }
        public Player player { get; private set; }
        public void GameStart()
        {
            Console.WriteLine("플레이어 이름을 입력하세요");
            string playerName = Console.ReadLine();
            player = new Player(playerName);
            Console.Clear();
            bool isPlaying = true;
            while(isPlaying)
            {
                Console.WriteLine("메인 메뉴");
                Console.WriteLine("\n1. 사냥터");
                Console.WriteLine("\n2. 마을");
                Console.WriteLine("\n3. 상점");
                Console.WriteLine("\n4. 상태창");
                Console.WriteLine("\n0. 종료");
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                ConsoleKey key = keyInfo.Key; 
                if(key == ConsoleKey.D1)
                {
                    //사냥터 이동
                }
                else if(key == ConsoleKey.D2)
                {
                    //마을 이동
                }
                else if(key == ConsoleKey.D3)
                {
                    //상점 이동
                }
                else if(key == ConsoleKey.D4)
                {
                    //상태창 이동
                }
                else if(key == ConsoleKey.D0)
                {
                    isPlaying = false;
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("메뉴에 있는 버튼을 선택하세요");
                    Console.WriteLine("Press the button");
                    Console.ReadKey(true);
                }

            }
        }
    }
}
