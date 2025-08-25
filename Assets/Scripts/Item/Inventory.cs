using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    public List<ItemStack> slots;

    public Inventory(int capacity)
    {
        slots = new List<ItemStack>(capacity);
        for (int i = 0; i < capacity; i++) slots.Add(null);
    }

    public bool Add(BaseItem item, int amount)
    {
        if (item == null || amount <= 0) return false;
        int remaining = amount;

        // 1) 기존 스택 채우기 (스택 가능 아이템만)
        if (ItemStackRules.CanStack(item))
        {
            for (int i = 0; i < slots.Count && remaining > 0; i++)
            {
                var s = slots[i];
                if (s != null && CanMergeStacks(s, item))
                {
                    int maxStack = ItemStackRules.GetMaxStack(item);
                    if (s.count < maxStack)
                    {
                        int move = Mathf.Min(maxStack - s.count, remaining);
                        s.count += move;
                        remaining -= move;
                    }
                }
            }
        }

        // 2) 빈 칸에 새 스택 생성
        for (int i = 0; i < slots.Count && remaining > 0; i++)
        {
            if (slots[i] == null)
            {
                int capacityForThisSlot = ItemStackRules.GetMaxStack(item); // 1 또는 99
                int move = Mathf.Min(capacityForThisSlot, remaining);
                slots[i] = new ItemStack(item, move);
                remaining -= move;
            }
        }

        return remaining == 0;
    }

    public bool RemoveAt(int index, int amount)
    {
        if (index < 0 || index >= slots.Count) return false;
        var s = slots[index];
        if (s == null || amount <= 0 || s.count < amount) return false;

        s.count -= amount;
        if (s.count <= 0) slots[index] = null;
        return true;
    }

    public bool UseAt(int index, UseContext ctx)
    {
        if (index < 0 || index >= slots.Count) return false;
        var s = slots[index];
        if (s == null || s.item == null) return false;

        if (!s.item.CanUse(ctx)) return false;
        bool used = s.item.Use(ctx);

        // 소비형(스택 가능 아이템)만 1 감소
        if (used && ItemStackRules.CanStack(s.item))
        {
            s.count -= 1;
            if (s.count <= 0) slots[index] = null;
        }
        return used;
    }

    // 동일 아이템 스택 병합 판단: 같은 SO 참조 + 인스턴스 상태(옵션/내구/업그레이드)가 동일해야 안전
    private bool CanMergeStacks(ItemStack s, BaseItem item)
    {
        if (s.item != item) return false;

        // 예: 강화/시드/내구가 모두 동일해야 병합 허용
        // 필요 정책에 맞게 완화/강화 가능
        // 현재는 단순 SO 동일만 체크하고, 인스턴스 상태가 있는 아이템(장비류)은 어차피 스택 불가(=1)이므로 추가 체크 불필요
        return true;
    }
}
