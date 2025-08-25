using System.Collections.Generic;
using Game.Items.Enums;

public class Equipment
{
    private readonly Dictionary<ArmorSlot, ItemStack> equipped = new();

    public bool CanEquip(ItemStack stack, out ArmorSlot slot)
    {
        slot = ArmorSlot.None;                  // 1) 기본값을 None으로
        if (stack?.item is ArmorItem armor)
        {
            slot = armor.armorSlot;                  // 2) 아이템이 지정한 슬롯으로 세팅
                                                     // 슬롯 유효성 검사(예: Unknown/None 금지)
            if (slot == ArmorSlot.None) return false;
            // 이미 해당 슬롯에 다른 아이템이 있는 경우 교체 허용 여부 판단 등 추가 규칙 가능
            return true;
        }
        return false;                           // 3) 아머가 아니면 장착 불가
    }

    public bool Equip(ItemStack stack)
    {
        if (stack == null || !(stack.item is ArmorItem armor)) return false;
        if (armor.armorSlot == ArmorSlot.None) return false; // 방어적 체크
                                                             // 교체 로직(기존 장착 반환) 등 확장 가능
        equipped[armor.armorSlot] = stack;
        return true;
    }


    public bool Unequip(ArmorSlot slot)
    {
        return equipped.Remove(slot);
    }

    public IEnumerable<ItemStack> AllEquipped() => equipped.Values;
}
