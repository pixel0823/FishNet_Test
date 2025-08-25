using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Recipe")]
public class RecipeItem : BaseItem
{
    public List<double> requiredItemIds; // 조합에 필요한 아이템 ID 목록
    public double resultItemId; // 조합 결과 아이템 ID

    public override bool CanUse(UseContext ctx)
    {
        // 조합법은 조합할 때 사용 가능
        return true;
    }

    public override bool Use(UseContext ctx)
    {
        // 조합 로직 구현 (예: 인벤토리에 필요한 아이템이 있는지 확인 후 조합)
        ctx.notify?.Invoke($"{itemName}을(를) 사용하여 아이템을 조합했습니다.");
        return true;
    }
}

