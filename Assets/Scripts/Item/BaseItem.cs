using System;
using UnityEngine;
using System.Collections.Generic;
using Game.Items.Enums;



public abstract class BaseItem : ScriptableObject
{
    [Header("기본 정보")]
    public string itemId; // 아이템 ID
    public string itemName; // 아이템 이름
    [TextArea] public string description; // 아이템 설명
    public Sprite icon; // 아이템 아이콘
    public ItemType itemType; // 아이템 타입


    public virtual bool CanUse(UseContext ctx) => false;
    public virtual bool Use(UseContext ctx) { return false; }
}

