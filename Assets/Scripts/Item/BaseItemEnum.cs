namespace Game.Items.Enums
{
    public enum ItemType
    {
        None = 0, // 기본값
        Weapon = 10, // 무기
        Armor = 20, // 방어구
        Food = 30, // 음식
        Tool = 40,   // 도구
        Material = 50,   // 재료
        Recipe = 60, // 조합법
        Miscellaneous = 100   // 기타
    }

    public enum ArmorSlot
    {
        None = 0, // 기본값
        Head = 10, // 머리
        Chest = 20, // 가슴
        Legs = 30, // 다리
        Hands = 40,// 손
        Feet = 50 // 발
    }

    public enum WeaponType
    {
        None = 0, // 기본값
        Melee = 10, // 근접 무기
        Ranged = 20 // 원거리 무기
    }

    public enum ToolKind
    {
        None = 0, // 기본값
        Axe = 10, // 도끼
        Pickaxe = 20, // 곡괭이
        Shovel = 30, // 삽
        Hoe = 40 // 괭이
    }
}

