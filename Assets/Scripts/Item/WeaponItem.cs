using Game.Items.Enums;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Weapon")]
public class WeaponItem : BaseItem
{
    [Header("Weapon")]
    public WeaponType weaponType; // 무기 타입
    public int damage; // 공격력
    public float attackSpeed; // 공격 속도
    public float range; // 사거리 (근접 무기는 짧은 거리, 원거리는 긴 거리)
    public float durability; // 내구도

    public override bool CanUse(UseContext ctx)
    {
        // 무기는 일반적으로 장착하거나 공격할 때 사용
        return true;
    }

    public override bool Use(UseContext ctx)
    {
        // 공격 로직 구현 (예: 적에게 피해 주기)
        ctx.notify?.Invoke($"{itemName}으로 공격!");
        return true;
    }
}
