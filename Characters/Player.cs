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
        public int MaxExp { get; private set; } = 10; // 클리어 확인용
        public int Gold { get; private set; } = 10000; // 상점 확인용
        public Inventory Inventory { get; private set; } //결국 따로 분리함
        public Armor ArmorEqip { get; private set; }
        public Weapon WeaponEqip { get; private set; }
        private bool gameClear = false; //게임 클리어 여부 확인
        //능력치합산용
        public override int Att
        {
            get
            {
                int totalAtt = BaseAtt;
                if (WeaponEqip != NoneWeapon.noneWeapon)
                {
                    totalAtt += WeaponEqip.AttPlus;
                }
                return totalAtt;
            }
        }
        public override int Def
        {
            get
            {
                int totalDef = BaseDef;
                if (ArmorEqip != NoneArmor.noneArmor)
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
                int totalSpeed = BaseSpeed;
                if (WeaponEqip != NoneWeapon.noneWeapon)
                {
                    totalSpeed -= WeaponEqip.SpeedMinus;
                }
                if (ArmorEqip != NoneArmor.noneArmor)
                {
                    totalSpeed -= ArmorEqip.SpeedMinus;
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
            //인벤초기화
            Inventory = new Inventory(this);
            //빈장비로 시작
            WeaponEqip = NoneWeapon.noneWeapon;
            ArmorEqip = NoneArmor.noneArmor;
        }
        public void GainGold(int amount) { Gold += amount; }
        public void IncreaseAtt(int amount) { BaseAtt += amount; }
        public void IncreaseDef(int amount) { BaseDef += amount; }
        public void DecreaseSpeed(int amount) { BaseSpeed -= amount; }
        public bool SpendGold(int amount)
        {
            if (Gold >= amount)
            {
                Gold -= amount;
                return true;
            }
            return false;
        }
        //보상
        public void GetReward(Monster monster)
        {
            Console.Clear();
            int gainedExp = monster.ExpReward;
            int gainedGold = monster.GoldReward;
            Exp += monster.ExpReward;
            Gold += monster.GoldReward;            
            Console.Clear();
            Console.WriteLine($"\n전투 보상!");
            Console.WriteLine($"\n획득 경험치 : {gainedExp}");
            Console.WriteLine($"\n획득 골드 : {gainedGold} G");
            Console.WriteLine($"\n현재 플레이어가 가진 골드 : {Gold} G");
            while (Exp >= MaxExp)
            {
                Exp -= MaxExp;
                LevelUp();
            }
        }
        //스텟,장비 확인용
        public void ShowStatus()
        {
            bool inStatus = true;
            while (inStatus)
            {
                Console.Clear();
                Console.WriteLine("*** 플레이어 상태창 ***\n");
                Console.WriteLine($"이름   : {Name}");
                Console.WriteLine($"레벨   : {Level}");
                Console.WriteLine($"체력   : {Hp} / {MaxHp}");
                Console.WriteLine($"경험치 : {Exp} / {MaxExp}");
                Console.WriteLine($"골드   : {Gold} G\n");
                Console.WriteLine($"공격력 : {Att} ({BaseAtt} + {WeaponEqip.AttPlus})");
                Console.WriteLine($"방어력 : {Def} ({BaseDef} + {ArmorEqip.DefPlus})");
                Console.WriteLine($"속도   : {Speed} ({BaseSpeed} - {WeaponEqip.SpeedMinus} - {ArmorEqip.SpeedMinus})\n");

                Console.WriteLine("장착 무기   : " + (WeaponEqip != null ? WeaponEqip.Name : "없음"));
                Console.WriteLine("장착 방어구 : " + (ArmorEqip != null ? ArmorEqip.Name : "없음"));
                Console.WriteLine("\n\n*** 인벤토리 ***\n");

                Inventory.ShowInventory();

                Console.WriteLine("\n1. 장비 착용");
                Console.WriteLine("\n2. 장비 해제");
                Console.WriteLine("\n0. 나가기");

                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.D1:
                        EquipMenu();
                        break;
                    case ConsoleKey.D2:
                        UnequipMenu();
                        break;
                    case ConsoleKey.D0:
                        return;
                    default:
                        GameManager.InvalidInput();
                        break;
                }
            }
        }
        //래벨업,클리어관련용
        public void LevelUp()
        {
            Level++;
            MaxExp += 10;
            MaxHp += 10;
            Hp = MaxHp;
            BaseAtt += 5;
            BaseDef += 1;
            BaseSpeed += 10;
            Console.WriteLine("\n레벨업!\n\n공격력,방어력,최대 체력,공격속도가 소폭 상승합니다.");
            if (!gameClear && Level >= 10)
            {
                gameClear = true;
                Console.Clear();
                Console.WriteLine("플레이어가 레벨10에 도달했습니다.");
                Console.WriteLine("\n목표 달성!");
                Console.WriteLine("\n1. 계속 모험하기");
                Console.WriteLine("\n2. 게임 종료");
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.D1:
                        Console.Clear();
                        Console.WriteLine("모험을 계속합니다!");
                        Console.WriteLine("\nPress the button");
                        Console.ReadKey(true);
                        break;
                    case ConsoleKey.D2:
                        Console.Clear();
                        Console.WriteLine("게임을 종료합니다. 플레이해주셔서 감사합니다!");
                        Console.WriteLine("\nPress the button");
                        Console.ReadKey(true);
                        Environment.Exit(0);
                        break;
                }
            }
        }
        //장비 착용
        public void EqipItem(string itemName)
        {
            if (!Inventory.HasItem(itemName))
            {
                Console.WriteLine($"\n{itemName}은 인벤토리에 없습니다.");
                Console.WriteLine("\nPress the button");
                Console.ReadKey(true);
                return;
            }

            Item item = Inventory.GetItem(itemName);

            // 레벨 제한 확인
            if (item is IWearableLevel wl && Level < wl.WearableLevel)
            {
                Console.WriteLine($"\n레벨이 부족합니다! (필요 레벨: {wl.WearableLevel})");
                Console.WriteLine("\nPress the button");
                Console.ReadKey(true);
                return;
            }

            // 무기 착용
            if (item is Weapon weapon)
            {
                if (!(WeaponEqip is NoneWeapon))
                {
                    Weapon prevWeapon = WeaponEqip;
                    prevWeapon.Unequip(this);


                    if (Inventory.HasItem(prevWeapon.Name))
                    {
                        if (prevWeapon is IQuantity PrevQuan)
                        {
                            ((IQuantity)Inventory.GetItem(prevWeapon.Name)).Quantity += PrevQuan.Quantity;
                        }
                    }
                    else
                    {
                        Inventory.Items[prevWeapon.Name] = prevWeapon;
                        Inventory.ItemList.AddLast(prevWeapon);
                    }
                    Console.WriteLine($"\n착용 중이던 무기 {prevWeapon.Name} 장착 해제!");
                }
                //새 무기 장착
                WeaponEqip = weapon;
                weapon.Equip(this);

                //인벤토리에서 제거
                Inventory.Items.Remove(itemName);
                Inventory.ItemList.Remove(item);//이걸 깜빡함..

                Console.WriteLine($"\n{weapon.Name} 장착 완료!");
                Console.WriteLine("\nPress the button");
                Console.ReadKey(true);
                return;
            }

            // 방어구 착용
            if (item is Armor armor)
            {
                if (!(ArmorEqip is NoneArmor))
                {
                    Armor prevArmor = ArmorEqip;
                    prevArmor.Unequip(this);

                    if (Inventory.HasItem(prevArmor.Name))
                    {
                        if (prevArmor is IQuantity PrevQuan)
                        {
                            ((IQuantity)Inventory.GetItem(prevArmor.Name)).Quantity += PrevQuan.Quantity;
                        }
                    }
                    else
                    {
                        Inventory.Items[prevArmor.Name] = prevArmor;
                        Inventory.ItemList.AddLast(prevArmor);
                    }
                    Console.WriteLine($"\n착용 중이던 방어구 {prevArmor.Name} 장착 해제!");
                }

                ArmorEqip = armor;
                armor.Equip(this);
                Inventory.Items.Remove(itemName);
                Inventory.ItemList.Remove(item);

                Console.WriteLine($"\n{armor.Name} 장착 완료!");
                Console.WriteLine("\nPress the button");
                Console.ReadKey(true);
                return;
            }

            Console.WriteLine($"\n{itemName}은 착용할 수 없는 아이템입니다.");
            Console.WriteLine("\nPress the button");
            Console.ReadKey(true);
        }
        //장비 해제
        public void UneqipItem(string itemName)
        {
            // 무기 해제
            if (!(WeaponEqip is NoneWeapon) && WeaponEqip.Name == itemName)
            {
                Weapon unequipped = WeaponEqip;
                unequipped.Unequip(this);//능력치 제거
                WeaponEqip = NoneWeapon.noneWeapon;//빈 무기로 교체

                //복사본으로 인벤토리에 추가
                Weapon copy = new Weapon(
                    unequipped.Name,
                    unequipped.Info,
                    unequipped.WearableLevel,
                    unequipped.AttPlus,
                    unequipped.SpeedMinus,
                    unequipped.Price,
                    unequipped.Type
                );
                //중복방지
                if (!Inventory.HasItem(copy.Name))
                {
                    Inventory.Items[copy.Name] = copy;
                    Inventory.ItemList.AddLast(copy);
                }

                Console.WriteLine($"\n{unequipped.Name} 착용 해제 완료!");
                Console.WriteLine("\nPress the button");
                Console.ReadKey(true);
                return;
            }
            // 방어구 해제
            if (!(ArmorEqip is NoneArmor) && ArmorEqip.Name == itemName)
            {
                Armor unequipped = ArmorEqip;
                unequipped.Unequip(this);
                ArmorEqip = NoneArmor.noneArmor;

                Armor copy = new Armor(
                    unequipped.Name,
                    unequipped.Info,
                    unequipped.WearableLevel,
                    unequipped.DefPlus,
                    unequipped.SpeedMinus,
                    unequipped.Price,
                    unequipped.Type
                );

                if (!Inventory.HasItem(copy.Name))
                {
                    Inventory.Items[copy.Name] = copy;
                    Inventory.ItemList.AddLast(copy);
                }

                Console.WriteLine($"\n{unequipped.Name} 착용 해제 완료!");
                Console.WriteLine("\nPress the button");
                Console.ReadKey(true);
                return;
            }

            Console.WriteLine($"\n{itemName}은(는) 현재 장착 중이 아닙니다.");
            Console.WriteLine("\nPress the button");
            Console.ReadKey(true);
        }
        //장비 착용UI
        private void EquipMenu()
        {
            bool exit = false;
            while (!exit)
            {
                Console.Clear();
                Console.WriteLine("*** 착용 가능한 장비 목록 ***\n");

                List<IEquippable> equipables = new List<IEquippable>();

                foreach (Item item in Inventory.Items.Values)
                {
                    if (item is IEquippable equippable)
                    {
                        equipables.Add(equippable);
                    }
                }

                if (equipables.Count == 0)
                {
                    Console.WriteLine("착용 가능한 장비가 없습니다.");
                    Console.WriteLine("\nPress the button");
                    Console.ReadKey(true);
                    return;
                }

                for (int i = 0; i < equipables.Count; i++)
                {
                    if (equipables[i] is Weapon weapon)
                    {
                        Console.WriteLine($"{i + 1}. {weapon.Name} - {weapon.Info}");
                        Console.WriteLine($"레벨제한: {weapon.WearableLevel} - 공격력 : {weapon.AttPlus} - 속도감소 : {weapon.SpeedMinus}\n");
                    }
                    else if (equipables[i] is Armor armor)
                    {
                        Console.WriteLine($"{i + 1}. {armor.Name} - {armor.Info}");
                        Console.WriteLine($"레벨제한: {armor.WearableLevel} - 방어력 : {armor.DefPlus} - 속도감소 : {armor.SpeedMinus}\n");
                    }
                    else
                    {
                        var item = (Item)equipables[i];
                        Console.WriteLine($"{i + 1}. {item.Name} - {item.Info}\n");
                    }
                }

                Console.WriteLine("\n0. 나가기");
                Console.Write("\n착용할 번호를 입력하세요: ");
                string input = Console.ReadLine();

                if (!int.TryParse(input, out int choice))
                {
                    Console.Clear();
                    GameManager.InvalidInput();
                    continue;
                }

                if (choice == 0)
                {
                    Console.Clear();
                    exit = true;
                }
                else if (choice < 0 || choice > equipables.Count)
                {
                    Console.Clear();
                    GameManager.InvalidInput();
                }
                else
                {
                    Console.Clear();
                    Item selected = (Item)equipables[choice - 1];
                    EqipItem(selected.Name);
                }
            }
        }
        //장비 해제UI
        private void UnequipMenu()
        {
            Console.Clear();
            Console.WriteLine("*** 장비 해제 메뉴 ***\n");
            Console.WriteLine("\n1. 무기 해제");
            Console.WriteLine("\n2. 방어구 해제");
            Console.WriteLine("\n0. 취소");

            switch (Console.ReadKey(true).Key)
            {
                case ConsoleKey.D1:
                    if (!(WeaponEqip is NoneWeapon))
                    {
                        UneqipItem(WeaponEqip.Name);
                    }
                    else
                    {
                        Console.WriteLine("착용 중인 무기가 없습니다.");
                        Console.WriteLine("\nPress the button");
                        Console.ReadKey(true);
                    }
                    break;
                case ConsoleKey.D2:
                    if (!(ArmorEqip is NoneArmor))
                    {
                        UneqipItem(ArmorEqip.Name);
                    }
                    else
                    {
                        Console.WriteLine("착용 중인 방어구가 없습니다.");
                        Console.WriteLine("\nPress the button");
                        Console.ReadKey(true);
                    }
                    break;
                case ConsoleKey.D0:
                    return;
                default:
                    GameManager.InvalidInput();
                    break;
            }
        }
        //도망 성공 여부
        public bool TryEscape()
        {
            Console.Clear();
            if (GameManager.GetRandom().NextFix(1, 3) == 3)
            {
                Console.WriteLine("도망 성공! 마을로 돌아갑니다.");
                return true;
            }
            else
            {
                Console.WriteLine("도망 실패! 몬스터에게 턴이 넘어갑니다.");
                return false;
            }
        }
    }
}