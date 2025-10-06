using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TXTRPG
{
    
    public class Weapon : Item, IEquippable, IWearableLevel
    {
        public int AttPlus { get; private set; }
        public WeaponType Type { get; private set;}
        public int SpeedMinus { get; private set; }
        public int WearableLevel { get; private set; }

        public Weapon(string name, string info, int wearableLevel, int attPlus, int speedMinus, int price, WeaponType type) : base(name,info,price)
        {
            WearableLevel = wearableLevel;
            AttPlus = attPlus;
            SpeedMinus = speedMinus;      
            Type = type;

        }
        public override string ToString()
        {
            return ($"{Name} - {Info} - 레벨제한 : {WearableLevel} - 공격력 : {AttPlus} - 속도감소 : {SpeedMinus} - 가격 : {Price} G");
        }
        public void Equip(Player player)
        {
            player.IncreaseAtt(AttPlus); //공격력 보정
        }

        public void Unequip(Player player)
        {
            player.IncreaseAtt(-AttPlus); //복구
        }
    }
    public class NoneWeapon : Weapon
    {
        public NoneWeapon()
            : base("없음", "무기를 장착하지 않음", 0, 0, 0, 0, WeaponType.None) { }
    }
}
