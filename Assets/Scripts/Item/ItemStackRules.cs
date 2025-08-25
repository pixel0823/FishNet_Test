using Game.Items.Enums;

public static class ItemStackRules
{
    public static int GetMaxStack(BaseItem item)
    {
        switch (item.itemType)
        {
            case ItemType.Weapon:
            case ItemType.Armor:
            case ItemType.Tool:
                return 1;
            default:
                return 99;
        }
    }

    public static bool CanStack(BaseItem item) => GetMaxStack(item) > 1;
}
