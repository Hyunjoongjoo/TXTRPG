using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TXTRPG
{
    public static class ItemRepository
    {
        public static List<Weapon> WeaponList { get; private set; }
        public static List<Armor> ArmorList { get; private set; }
        public static List<Potion> PotionList { get; private set; }

        static ItemRepository()
        {

            WeaponList = new List<Weapon>();           
            ArmorList = new List<Armor>();
            PotionList = new List<Potion>();
            Items();
        }
        private static void Items()
        {

            //소드종류
            WeaponList.Add(new Weapon("롱소드","시작 소드", 0, 5, 25, 50, WeaponType.Sword));
            WeaponList.Add(new Weapon("바스타드 소드","초급 소드", 5, 10, 25, 100, WeaponType.Sword));
            WeaponList.Add(new Weapon("클레이모어","중급 소드", 10, 15, 25, 150, WeaponType.Sword));
            WeaponList.Add(new Weapon("플랑베르쥬","상급 소드", 15, 20, 25, 200, WeaponType.Sword));
            WeaponList.Add(new Weapon("츠바이한더","최상급 소드", 20, 25, 25, 250, WeaponType.Sword));
            //도끼종류
            WeaponList.Add(new Weapon("핸드 액스", "시작 도끼", 0, 4, 20, 50, WeaponType.Axe));
            WeaponList.Add(new Weapon("배틀 액스", "초급 도끼", 5, 9, 20, 100, WeaponType.Axe));
            WeaponList.Add(new Weapon("그레이트 액스", "중급 도끼", 10, 14, 20, 150, WeaponType.Axe));
            WeaponList.Add(new Weapon("워액스", "상급 도끼", 15, 19, 20, 200, WeaponType.Axe));
            WeaponList.Add(new Weapon("드래곤 액스", "최상급 도끼", 20, 24, 20, 250, WeaponType.Axe));
            //활종류
            WeaponList.Add(new Weapon("숏보우", "시작 활", 0, 2, 10, 50, WeaponType.Bow));
            WeaponList.Add(new Weapon("롱보우", "초급 활", 5, 6, 10, 100, WeaponType.Bow));
            WeaponList.Add(new Weapon("철궁", "중급 활", 10, 10, 10, 150, WeaponType.Bow));
            WeaponList.Add(new Weapon("리커브 보우", "상급 활", 15, 14, 10, 200, WeaponType.Bow));
            WeaponList.Add(new Weapon("컴파운드 보우", "최상급 활", 20, 18, 10, 250, WeaponType.Bow));
            //둔기종류
            WeaponList.Add(new Weapon("슬레지해머", "시작 망치, 오함마", 0, 10, 50, 50, WeaponType.Hammer));
            WeaponList.Add(new Weapon("아이언해머", "초급 망치", 5, 20, 50, 100, WeaponType.Hammer));
            WeaponList.Add(new Weapon("배틀해머", "중급 망치", 10, 30, 50, 150, WeaponType.Hammer));
            WeaponList.Add(new Weapon("워해머", "상급 망치", 15, 40, 50, 200, WeaponType.Hammer));
            WeaponList.Add(new Weapon("그라비티 해머", "최상급 망치", 20, 50, 50, 250, WeaponType.Hammer));
            //가죽방어구
            ArmorList.Add(new Armor("하이드 아머", "생가죽으로 만든 아머", 0, 1, 0, 50, ArmorType.Leather));
            ArmorList.Add(new Armor("하드 레더 아머", "단단하게 만든 가죽 갑옷", 5, 2, 0, 100, ArmorType.Leather));
            ArmorList.Add(new Armor("보일드 레더 아머", "더욱 단단하게 만든 가죽 갑옷", 10, 3, 0, 150, ArmorType.Leather));
            ArmorList.Add(new Armor("라멜라 레더 아머", "가죽을 겹처서 갑옷", 15, 4, 0, 200, ArmorType.Leather));
            ArmorList.Add(new Armor("리인포스드 레더 아머", "가죽에 마법처리를 해서 만든 갑옷", 20, 5, 0, 250, ArmorType.Leather));
            //사슬방어구
            ArmorList.Add(new Armor("브론즈 체인", "청동 사슬 갑옷", 0, 5, 10, 50, ArmorType.Chain));
            ArmorList.Add(new Armor("아이언 체인", "철 사슬 갑옷", 5, 10, 10, 100, ArmorType.Chain));
            ArmorList.Add(new Armor("스틸 체인", "강철 사슬 갑옷", 10, 15, 10, 150, ArmorType.Chain));
            ArmorList.Add(new Armor("미스릴 체인", "미스릴 사슬 갑옷", 15, 20, 10, 200, ArmorType.Chain));
            ArmorList.Add(new Armor("아다만디움 체인", "아다만디움 사슬 갑옷", 20, 25, 10, 250, ArmorType.Chain));
            //비늘방어구
            ArmorList.Add(new Armor("브론즈 스케일", "청동 비늘 갑옷", 0, 8, 20, 50, ArmorType.Scale));
            ArmorList.Add(new Armor("아이언 스케일", "철 비늘 갑옷", 5, 13, 20, 100, ArmorType.Scale));
            ArmorList.Add(new Armor("스틸 스케일", "강철 비늘 갑옷", 10, 18, 20, 150, ArmorType.Scale));
            ArmorList.Add(new Armor("미스릴 스케일", "미스릴 비늘 갑옷", 15, 23, 20, 200, ArmorType.Scale));
            ArmorList.Add(new Armor("아다만디움 스케일", "아다만디움 비늘 갑옷", 20, 28, 20, 250, ArmorType.Scale));
            //판금방어구
            ArmorList.Add(new Armor("아이언 플레이트", "기본 판금 갑옷", 0, 10, 30, 50, ArmorType.Plate));
            ArmorList.Add(new Armor("스틸 플레이트", "강철 판금 갑옷", 5, 20, 35, 100, ArmorType.Plate));
            ArmorList.Add(new Armor("미스릴 플레이트", "미스릴 판금 갑옷", 10, 30, 40, 150, ArmorType.Plate));
            ArmorList.Add(new Armor("아다만디움 플레이트", "아다만디움 판금 갑옷", 15, 40, 40, 200, ArmorType.Plate));
            //포션
            PotionList.Add(new Potion("하급 포션", "체력 30% 회복", 20, 0.3f));
            PotionList.Add(new Potion("중급 포션", "체력 50% 회복", 50, 0.5f));
            PotionList.Add(new Potion("상급 포션", "체력 80% 회복", 100, 0.8f));
            PotionList.Add(new Potion("최상급 포션", "체력 100% 회복", 200, 1.0f));
        }
    }
}
