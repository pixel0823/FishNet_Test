using UnityEngine;

[CreateAssetMenu(menuName = "Items/Miscellaneous")]
public class MiscellaneousItem : BaseItem
{
    public override bool CanUse(UseContext ctx)
    {
        // 기타 아이템은 상황에 따라 다름
        return false;
    }

    public override bool Use(UseContext ctx)
    {
        // 기타 아이템 사용 로직 구현
        ctx.notify?.Invoke($"{itemName} 사용됨.");
        return true;
    }
}

