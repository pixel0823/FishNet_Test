using Game.Items.Enums;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Armor")]
public class ArmorItem : BaseItem
{
    [Header("Armor")]
    public ArmorSlot armorSlot; // 방어구 착용 부위
    public int defense; // 방어력
    public float durability; // 내구도

    public override bool CanUse(UseContext ctx)
    {
        // 방어구는 장착할 때 사용
        return true;
    }

    public override bool Use(UseContext ctx)
    {
        // 방어구 장착 로직 구현
        // 예: 플레이어의 방어구 슬롯에 이 아이템을 장착
        ctx.notify?.Invoke($"{itemName} 장착됨.");
        return true;
    }
}

