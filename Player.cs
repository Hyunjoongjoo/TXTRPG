using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TXTRPG
{
    public class Player : Character
    {
        public int Exp { get; private set; } = 0;
        public int MaxExp { get; private set; } = 100;
        public int Gold { get; private set; } = 10;
        public Dictionary<string, Item> InventoryDic { get; private set; } //아이템 검색
        public LinkedList<Item> InventoryList { get; private set; } //아이템 순서 관리
        public Armor ArmorEqip { get; private set; }
        public Weapon WeaponEqip { get; private set; }
        public Player(string name) : base(name)
        {
            Hp = 100;
            MaxHp = 100;
            Att = 20;
            Speed = 100;
            CriChance = 0.2f;
            CriMultiplier = 1.3f;
            InventoryDic = new Dictionary<string, Item>();
            InventoryList = new LinkedList<Item>();
        }
        
        public void UseItem(Item item)
        {

        }
        public void GetReward(Monster monster)
        {
            Exp += monster.ExpReward;
            Gold += monster.GoldReward;
            while (Exp >=MaxExp)
            {
                Exp -= MaxExp;
                LevelUp();
            }
        }
        public void LevelUp()
        {
            Level++;
            MaxExp += 10;
            MaxHp += 10;
            Hp = MaxHp;
            Att += 5;
            Def += 1;
            Speed += 10;
            Console.WriteLine("레벨업!\n공격력,방어력,최대 체력,공격속도가 소폭 상승합니다.");
            Console.WriteLine("Press the button");
            Console.ReadKey(true);
        }
        public void EqipItem(string itemName)
        {
            if(InventoryDic.ContainsKey(itemName))
            {
                Item item = InventoryDic[itemName];
                if (item is IEquippable eqip)
                {
                    eqip.Equip(this);
                    Console.WriteLine($"{itemName} 착용");
                }
                else
                {
                    Console.WriteLine($"{itemName}은 착용 가능한 아이템이 아닙니다.");
                }
            }
            else
            {
                Console.WriteLine($"착용 가능한 장비가 인벤토리에 없습니다.");
            }
        }
        public void UneqipItem(string itemName)
        {
            Item item = InventoryDic[itemName];
            if (item is IEquippable uneqip)
            {
                uneqip.Unequip(this);
                Console.WriteLine($"{itemName} 착용 해제");
            }
        }
        public override void Attack(Character monsterTarget) 
        {
            int damage = Damage();
            monsterTarget.TakeDamage(damage);
        }
    }
}
