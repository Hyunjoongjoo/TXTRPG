using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TXTRPG
{
    public class Potion : Item, IUsable , IQuantity
    {
        public int Quantity { get; set; }
        public int MaxQuantity { get; private set; } = 99; //최대갯수
        public float HealPercent { get; private set; } //회복 비율
        public Potion(string name, string info, int price, float healPercent) : base(name, info, price)
        {
            HealPercent = healPercent;
        }
        public override string ToString()
        {
            return ($"{Name} - {Info} - 회복량 : {HealPercent * 100}% - 가격 : {Price} G");
        }
        public void Use(Player player)
        {
            //최대 체력일때 사용 불가
            if (player.Hp >= player.MaxHp)
            {
                Console.Clear();
                Console.WriteLine("현재 체력이 이미 최대입니다. 포션을 사용할 수 없습니다!");
                Console.WriteLine("\nPress the button");
                Console.ReadKey(true);
                return;
            }
            if (Quantity > 0)
            {
                int healAmount = (int)(player.MaxHp * HealPercent);
                player.Heal(healAmount);  
                Quantity--;
                Console.WriteLine($"{Name}을(를) 사용하여 {healAmount}만큼 체력 회복!");
                Console.WriteLine($"\n현재 체력 : {player.Hp}");
                Console.WriteLine("\nPress the button");
                Console.ReadKey(true);
            }
            else
            {
                Console.WriteLine("사용 가능한 포션이 없습니다!");
                Console.WriteLine("\nPress the button");
                Console.ReadKey(true);
            }
        }
    }
}
