using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ItemStack
{
    public BaseItem item;   // SO 참조
    public int count = 1;

    // 변동 상태(예: 내구도/강화/랜덤옵션)는 개별 Instance에 보관
    public float durability;  // Armor/Weapon일 때만 의미
    public int upgradeLevel;
    public int seed;        // 옵션 롤링 시드 등

    public ItemStack(BaseItem item, int count = 1)
    {
        this.item = item;
        this.count = count;
        if (item is ArmorItem ai) durability = ai.durability;
    }

    public bool IsFull => item != null && count >= ItemStackRules.GetMaxStack(item);
    public int FreeSpace => item == null ? 0 : (ItemStackRules.GetMaxStack(item) - count);
    public bool CanMerge(ItemStack other) => other != null && item == other.item && !IsFull;

}

