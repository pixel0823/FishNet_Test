using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Food")]
public class FoodItem : BaseItem
{
    public int healthRestore; // 회복량
    public List<StatBuff> buffs; // 추가 버프 목록

    public override bool CanUse(UseContext ctx)
    {
        // 음식은 사용 가능 (예: 체력 회복)
        return true;
    }

    public override bool Use(UseContext ctx)
    {
        // 체력 회복 로직 구현
        ctx.notify?.Invoke($"{itemName}을(를) 먹고 {healthRestore}만큼 회복했습니다.");

        // 버프 적용
        foreach (var buff in buffs)
        {
            // 버프 적용 로직 구현 (예: 플레이어의 스탯 증가)
            ctx.notify?.Invoke($"{buff.statName}이(가) {buff.amount}만큼 증가했습니다. 지속 시간: {buff.duration}초");
        }
        // 음식 아이템은 소비되므로 인벤토리에서 제거하는 로직 필요
        // 예: ctx.user.GetComponent<Inventory>().RemoveItem(this);
        // 이 예제에서는 단순히 메시지만 출력
        ctx.notify?.Invoke($"{itemName}이(가) 소비되었습니다.");
        // 실제로는 인벤토리 시스템과 연동하여 아이템을 제거해야 함
        return true;
    }
}

