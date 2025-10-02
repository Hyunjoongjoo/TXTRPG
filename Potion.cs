using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TXTRPG
{
    public class Potion : Item, IUsable , IQuantity
    {
        public int Quantity { get; set; } = 1;
        public int MaxQuantity { get; private set; } = 99;
        public void Use(Character player)
        {
            if (Quantity > 0)
            {
                Quantity--;
                Console.WriteLine($"포션을 사용하여 ??만큼 체력 회복");
            }
            else
            {
                Console.WriteLine("사용 가능한 포션이 없습니다.");
            }
        }
    }
}
