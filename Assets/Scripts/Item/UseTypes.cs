using System;
using UnityEngine;

public interface IUseAction
{
    void Apply(UseContext ctx);
}

[Serializable]
public class StatBuff
{
    public string statName; // 버프를 적용할 스탯 이름 (예: "Health", "Strength" 등)
    public float amount; // 버프 양 (예: +10, -5 등)
    public float duration; // 버프 지속 시간 (초 단위)
}

public class UseContext
{
    public GameObject user; // 아이템을 사용하는 게임 오브젝트
    public GameObject target; // 아이템이 적용되는 대상 (예: 적, 아군 등)
    public Action<string> notify; // UI 메시지 연결 등
}