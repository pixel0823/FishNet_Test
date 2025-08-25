using Game.Items.Enums;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Tool")]
public class ToolItem : BaseItem
{
    public ToolKind toolKind; // 도구 종류
    public float efficiency; // 효율성 (예: 자원 채취 속도)
    public float durability; // 내구도

    public override bool CanUse(UseContext ctx)
    {
        // 도구는 자원 채취 등에 사용 가능
        return true;
    }

    public override bool Use(UseContext ctx)
    {
        // 자원 채취 로직 구현
        ctx.notify?.Invoke($"{itemName}으로 자원을 채취했습니다.");
        return true;
    }
}

