using UnityEngine;

[CreateAssetMenu(menuName = "Items/Material")]
public class MaterialItem : BaseItem
{
    public override bool CanUse(UseContext ctx)
    {
        // 재료 아이템은 일반적으로 사용하지 않음
        return false;
    }

    public override bool Use(UseContext ctx)
    {
        // 사용 불가
        ctx.notify?.Invoke($"{itemName}은(는) 사용할 수 없습니다.");
        return false;
    }
}

