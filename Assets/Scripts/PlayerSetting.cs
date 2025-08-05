

using System;
using UnityEditor;

public enum EControlType
{
    Mouse,
    KeyboardMouse
}

public class PlayerSetting
{
    public static EControlType controlType;

    internal static void SetScriptingDefineSymbolsForGroup(BuildTargetGroup selectedBuildTargetGroup, string changedDefines)
    {
        throw new NotImplementedException();
    }
}
