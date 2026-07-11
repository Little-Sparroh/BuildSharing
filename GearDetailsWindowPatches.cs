using HarmonyLib;
using UnityEngine;

[HarmonyPatch(typeof(GearDetailsWindow), "Awake")]
public static class GearDetailsWindow_Awake_Patch
{
    [HarmonyPostfix]
    static void Postfix(GearDetailsWindow __instance)
    {
        EnsureSharingButtons(__instance);
    }

    internal static void EnsureSharingButtons(GearDetailsWindow window)
    {
        if (window == null) return;
        var existing = window.GetComponent<SharingButtons>();
        if (existing == null)
        {
            existing = window.gameObject.AddComponent<SharingButtons>();
        }
        existing.window = window;
    }
}

[HarmonyPatch(typeof(GearDetailsWindow), "OnOpen")]
public static class GearDetailsWindow_OnOpen_Patch
{
    [HarmonyPostfix]
    static void Postfix(GearDetailsWindow __instance)
    {
        GearDetailsWindow_Awake_Patch.EnsureSharingButtons(__instance);
    }
}
