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
        public Dictionary<string, Item> InventoryDic { get; private set; } //검색용 딕셔너리
        public LinkedList<Item> InventoryList { get; private set; } //출력용 링크드리스트
        public Armor ArmorEqip { get; private set; }
        public Weapon WeaponEqip { get; private set; }
        private bool gameClear = false; //게임 클리어 여부 확인
        //능력치합산용
        public override int Att
        {
            get
            {
                int totalAtt = BaseAtt;
                if (WeaponEqip != null)
                    totalAtt += WeaponEqip.AttPlus;
                return totalAtt;
            }
        }
        public override int Def
        {
            get
            {
                int totalDef = BaseDef;
                if (ArmorEqip != null)
                    totalDef += ArmorEqip.DefPlus;
                return totalDef;
            }
        }
        public override int Speed
        {
            get
            {
                int totalSpeed = BaseSpeed;
                if (WeaponEqip != null)
                    totalSpeed -= WeaponEqip.SpeedMinus;
                if (ArmorEqip != null)
                    totalSpeed -= ArmorEqip.SpeedMinus;
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
            InventoryDic = new Dictionary<string, Item>();
            InventoryList = new LinkedList<Item>();
            //빈장비로 시작
            WeaponEqip = new NoneWeapon();
            ArmorEqip = new NoneArmor();
        }

        public bool SpendGold(int amount)
        {
            if (Gold >= amount)
            {
                Gold -= amount;
                return true;
            }
            return false;
        }
        public void GainGold(int amount) { Gold += amount; }
        public void IncreaseAtt(int amount) { BaseAtt += amount; }
        public void IncreaseDef(int amount) { BaseDef += amount; }
        public void DecreaseSpeed(int amount) { BaseSpeed -= amount; }
        private void AddToInventory(Item item)
        {
            // 포션만 스택(Quantity) 처리
            if (item is Potion p)
            {
                if (InventoryDic.ContainsKey(p.Name))
                {
                    ((Potion)InventoryDic[p.Name]).Quantity += Math.Max(1, p.Quantity);
                }
                else
                {
                    if (p.Quantity <= 0) p.Quantity = 1; // 안전장치
                    InventoryDic[p.Name] = p;
                    InventoryList.AddLast(p);
                }
            }
            else
            {
                // 무기/방어구 등은 이름 1개만(딕셔너리 특성) — 이미 있으면 추가 안 함
                if (!InventoryDic.ContainsKey(item.Name))
                {
                    InventoryDic[item.Name] = item;
                    InventoryList.AddLast(item);
                }
            }
        }

        private void RemoveFromInventory(Item item)
        {
            if (item == null) return;

            if (item is Potion p)
            {
                // 포션의 경우 Quantity가 0 이하면 제거
                if (InventoryDic.ContainsKey(p.Name))
                {
                    var invPot = (Potion)InventoryDic[p.Name];
                    if (invPot.Quantity <= 0)
                    {
                        InventoryDic.Remove(p.Name);
                        InventoryList.Remove(invPot);
                    }
                }
            }
            else
            {
                if (InventoryDic.ContainsKey(item.Name))
                {
                    var invItem = InventoryDic[item.Name];
                    InventoryDic.Remove(item.Name);
                    InventoryList.Remove(invItem);
                }
            }
        }
        //포션사용
        public void UseItem(Item item)
        {
            Console.Clear();
            if (item is IUsable usable)
            {
                usable.Use(this);
                RemoveItem(item);
            }
        }
        //보상
        public void GetReward(Monster monster)
        {
            Console.Clear();
            int gainedExp = monster.ExpReward;
            int gainedGold = monster.GoldReward;
            Exp += monster.ExpReward;
            Gold += monster.GoldReward;
            while (Exp >= MaxExp)
            {
                Exp -= MaxExp;
                LevelUp();
            }
            Console.Clear();
            Console.WriteLine($"\n전투 보상!");
            Console.WriteLine($"\n획득 경험치 : {gainedExp}");
            Console.WriteLine($"\n획득 골드 : {gainedGold} G");
            Console.WriteLine($"\n현재 플레이어가 가진 골드 : {Gold} G");
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

                Dictionary<string, int> itemCounts = new Dictionary<string, int>();
                foreach (var item in InventoryList)
                {
                    if (item is IQuantity qItem)
                    {
                        if (itemCounts.ContainsKey(item.Name))
                            itemCounts[item.Name] += qItem.Quantity;
                        else
                            itemCounts[item.Name] = qItem.Quantity;
                    }
                    else
                    {
                        if (itemCounts.ContainsKey(item.Name))
                            itemCounts[item.Name]++;
                        else
                            itemCounts[item.Name] = 1;
                    }
                }

                foreach (var item in itemCounts)
                {
                    if (item.Value > 1)
                        Console.WriteLine($"- {item.Key} x{item.Value}");
                    else
                        Console.WriteLine($"- {item.Key}");
                }

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
            Console.WriteLine("\nPress the button");
            Console.ReadKey(true);
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
            if (!InventoryDic.ContainsKey(itemName))
            {
                Console.Clear();
                Console.WriteLine($"\n{itemName}은 인벤토리에 없습니다.");
                Console.WriteLine("\nPress the button");
                Console.ReadKey(true);
                return;
            }

            Item item = InventoryDic[itemName];

            // 레벨 제한 확인
            if (item is IWearableLevel wl && Level < wl.WearableLevel)
            {
                Console.Clear();
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
                    var prev = WeaponEqip;
                    prev.Unequip(this);

                    if (InventoryDic.ContainsKey(prev.Name))
                    {
                        if (prev is IQuantity qPrev)
                            ((IQuantity)InventoryDic[prev.Name]).Quantity += qPrev.Quantity;
                    }
                    else
                    {
                        InventoryDic[prev.Name] = prev;
                        InventoryList.AddLast(prev);
                    }

                    Console.WriteLine($"\n착용 중이던 무기 {prev.Name}을(를) 해제했습니다.");
                }
                //새 무기 장착
                WeaponEqip = weapon;
                weapon.Equip(this);

                //인벤토리에서 제거
                InventoryDic.Remove(itemName);

                Console.Clear();
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
                    var prev = ArmorEqip;
                    prev.Unequip(this);

                    if (InventoryDic.ContainsKey(prev.Name))
                    {
                        if (prev is IQuantity qPrev)
                            ((IQuantity)InventoryDic[prev.Name]).Quantity += qPrev.Quantity;
                    }
                    else
                    {
                        InventoryDic[prev.Name] = prev;
                        InventoryList.AddLast(prev);
                    }

                    Console.WriteLine($"\n착용 중이던 방어구 {prev.Name}을(를) 해제했습니다.");
                }

                ArmorEqip = armor;
                armor.Equip(this);
                InventoryDic.Remove(itemName);

                Console.Clear();
                Console.WriteLine($"\n{armor.Name} 장착 완료!");
                Console.WriteLine("\nPress the button");
                Console.ReadKey(true);
                return;
            }

            Console.Clear();
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
                var unequipped = WeaponEqip;
                unequipped.Unequip(this);
                WeaponEqip = new NoneWeapon();

                // ✅ 새 복사본으로 인벤토리에 추가
                var copy = new Weapon(
                    unequipped.Name, unequipped.Info,
                    (unequipped as Weapon).WearableLevel,
                    (unequipped as Weapon).AttPlus,
                    (unequipped as Weapon).SpeedMinus,
                    unequipped.Price,
                    (unequipped as Weapon).Type
                );

                if (!InventoryDic.ContainsKey(copy.Name))
                {
                    InventoryDic[copy.Name] = copy;
                    InventoryList.AddLast(copy);
                }

                Console.Clear();
                Console.WriteLine($"\n{unequipped.Name} 착용 해제 완료!");
                Console.WriteLine("\nPress the button");
                Console.ReadKey(true);
                return;
            }
            // 방어구 해제
            if (!(ArmorEqip is NoneArmor) && ArmorEqip.Name == itemName)
            {
                var unequipped = ArmorEqip;
                unequipped.Unequip(this);
                ArmorEqip = new NoneArmor();

                var copy = new Armor(
                    unequipped.Name, unequipped.Info,
                    (unequipped as Armor).WearableLevel,
                    (unequipped as Armor).DefPlus,
                    (unequipped as Armor).SpeedMinus,
                    unequipped.Price,
                    (unequipped as Armor).Type
                );

                if (!InventoryDic.ContainsKey(copy.Name))
                {
                    InventoryDic[copy.Name] = copy;
                    InventoryList.AddLast(copy);
                }

                Console.Clear();
                Console.WriteLine($"\n{unequipped.Name} 착용 해제 완료!");
                Console.WriteLine("\nPress the button");
                Console.ReadKey(true);
                return;
            }

            Console.Clear();
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

                var equipables = InventoryDic.Values.OfType<IEquippable>().ToList();

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
                    var selected = (Item)equipables[choice - 1];
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
                        UneqipItem(WeaponEqip.Name);
                    else
                        Console.WriteLine("착용 중인 무기가 없습니다.");
                    break;
                case ConsoleKey.D2:
                    if (!(ArmorEqip is NoneArmor))
                        UneqipItem(ArmorEqip.Name);
                    else
                        Console.WriteLine("착용 중인 방어구가 없습니다.");
                    break;
                case ConsoleKey.D0:
                    return;
                default:
                    GameManager.InvalidInput();
                    break;
            }
        }
        //인벤토리에 아이템 추가,제거
        public void AddItem(Item item)
        {
            if (item is IQuantity qItem)
            {
                if (InventoryDic.ContainsKey(item.Name))
                {
                    ((IQuantity)InventoryDic[item.Name]).Quantity += qItem.Quantity;
                }
                else
                {
                    InventoryDic[item.Name] = item;
                    InventoryList.AddLast(item);
                }
            }
            else
            {
                if (!InventoryDic.ContainsKey(item.Name))
                {
                    InventoryDic[item.Name] = item;
                    InventoryList.AddLast(item);
                }
            }
        }
        public void RemoveItem(Item item)
        {
            if (item is IQuantity qItem)
            {
                if (qItem.Quantity <= 0 && InventoryDic.ContainsKey(item.Name))
                {
                    InventoryDic.Remove(item.Name);
                    InventoryList.Remove(item);
                }
            }
            else
            {
                if (InventoryDic.ContainsKey(item.Name))
                {
                    InventoryDic.Remove(item.Name);
                    InventoryList.Remove(item);
                }
            }
        }
        //도망 성공 여부
        public bool TryEscape()
        {
            Console.Clear();
            if (GameManager.GetRandom().NextFix(1, 3) == 3)
            {
                Console.WriteLine("도망 성공! 마을로 돌아갑니다.");
                Console.WriteLine("\nPress the button");
                Console.ReadKey(true);
                return true;
            }
            else
            {
                Console.WriteLine("도망 실패! 몬스터에게 턴이 넘어갑니다.");
                Console.WriteLine("\nPress the button");
                Console.ReadKey(true);
                return false;
            }
        }
    }
}