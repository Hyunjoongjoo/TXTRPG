using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TXTRPG
{
    
    public class Armor : Item , IEquippable , IWearableLevel
    {
        public int DefPlus {  get; private set; }
        public int SpeedMinus { get; private set; }
        public ArmorType Type { get; private set; }
        public int WearableLevel { get; private set; }
        public Armor(string name, string info, int wearableLevel, int defPlus, int speedMinus, int price, ArmorType type) : base(name, info, price)
        {
            WearableLevel = wearableLevel;
            DefPlus = defPlus;
            SpeedMinus = speedMinus;
            Type = type;

        }
        //마지막에 되서야 사용 안된걸 알게됨..
        public override string ToString()
        {
            return ($"{Name} - {Info} - 레벨제한 : {WearableLevel} - 방어력 : {DefPlus} - 속도감소 : {SpeedMinus} - 가격 : {Price} G");
        }
        //장비 착용에 따라 증가되는 능력치
        public void Equip(Player player)
        {
            player.IncreaseDef(DefPlus);
        }

        public void Unequip(Player player)
        {
            player.IncreaseDef(-DefPlus);
        }
    }
    public class NoneArmor : Armor
    {
        public NoneArmor()
            : base("없음", "방어구를 장착하지 않음", 0, 0, 0, 0, ArmorType.None) { }
    }
}
