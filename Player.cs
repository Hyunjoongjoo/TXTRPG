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
        public override int Att
        {
            get
            {
                int totalAtt = BaseAtt;
                if (WeaponEqip != null) //무기 착용중이라면
                {
                    totalAtt += WeaponEqip.AttPlus; //무기공격력추가
                }
                return totalAtt;
            }
        }
        public override int Def
        {
            get
            {
                int totalDef = BaseDef;
                if (ArmorEqip != null)
                {
                    totalDef += ArmorEqip.DefPlus;
                }
                return totalDef;
            }
        }
        public override int Speed
        {
            get
            {
                int totalSpeed = BaseSpeed; //기본 속도
                if (WeaponEqip != null)
                {
                    totalSpeed -= WeaponEqip.WeaponSpeed; //무기 속도만큼 빼주는식
                }
                if (ArmorEqip != null)
                {
                    totalSpeed -= ArmorEqip.SpeedMinus; //방어구에 따른 속도 감소
                }
                return totalSpeed;
            }
        }


        public Player(string name) : base(name)
        {
            Hp = 100;
            MaxHp = 100;
            BaseAtt = 20;
            BaseSpeed = 100;
            CriChance = 0.2f;
            CriMultiplier = 1.3f;
            InventoryDic = new Dictionary<string, Item>();
            InventoryList = new LinkedList<Item>();
        }
        public void IncreaseAtt(int amount)
        {
            BaseAtt += amount;
        }
        public void IncreaseDef(int amount)
        {
            BaseDef += amount;
        }
        public void DecreaseSpeed(int amount)
        {
            BaseSpeed -= amount;
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
            BaseAtt += 5;
            BaseDef += 1;
            BaseSpeed += 10;
            Console.WriteLine("레벨업!\n공격력,방어력,최대 체력,공격속도가 소폭 상승합니다.");
            Console.WriteLine("Press the button");
            Console.ReadKey(true);
        }
        public void EqipItem(string itemName)
        {
            if (!InventoryDic.ContainsKey(itemName)) //인벤에 아이템이 없다면
            {
                Console.WriteLine($"{itemName}은 인벤토리에 없습니다.");
                return;
            }
            Item item = InventoryDic[itemName];
            if (item is Weapon weapon)
            {
                if (WeaponEqip != null)//장착중인 무기가 있다면
                {
                    InventoryDic[WeaponEqip.Name] = WeaponEqip;
                    WeaponEqip.Unequip(this);//능력치 제거
                    Console.Write("착용중이던 무기 ");
                }
                WeaponEqip = weapon;//슬롯에 저장
                weapon.Equip(this);//능력치 보정
                InventoryDic.Remove(itemName); // 인벤토리에서 제거
                Console.WriteLine($"{itemName} 장착 완료");
            }
            else if (item is Armor armor)
            {
                if (ArmorEqip != null)//장착중인 방어구가 있다면
                {
                    InventoryDic[ArmorEqip.Name] = ArmorEqip;
                    ArmorEqip.Unequip(this);
                    Console.WriteLine("착용중이던 방어구 ");
                }
                ArmorEqip = armor;//저장
                armor.Equip(this);//능력치 보정
                InventoryDic.Remove(itemName); // 인벤토리에서 제거
                Console.WriteLine($"{itemName} 장착 완료");
            }
            else
            {
                Console.WriteLine($"{itemName}은 착용할 수 없는 아이템입니다.");
            }
        }
        public void UneqipItem(string itemName)
        {
            if (WeaponEqip != null && WeaponEqip.Name == itemName)
            {
                var unequipName = WeaponEqip.Name;//이름 저장
                WeaponEqip.Unequip(this);//능력치 보정 해제
                WeaponEqip = null;//장착 슬롯 비우기
                Console.WriteLine($"{unequipName} 착용 해제");
                return;
            }
            //방어구도 동일하게
            if (ArmorEqip != null && ArmorEqip.Name == itemName)
            {
                var unequipName = ArmorEqip.Name;
                ArmorEqip.Unequip(this); 
                ArmorEqip = null;
                Console.WriteLine($"{unequipName} 착용 해제");
                return;
            }

            Console.WriteLine($"{itemName}은 장착되어 있지 않습니다.");
        } 
        public override void Attack(Character monsterTarget) 
        {
            int damage = Damage();
            monsterTarget.TakeDamage(damage);
        }
    }
}
