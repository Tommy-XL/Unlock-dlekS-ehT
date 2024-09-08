using HarmonyLib;
using UnityEngine;
using UnlockDleks.Modules;

namespace UnlockDleks.Patches;


[HarmonyPatch(typeof(GameStartManager))]
public static class GameStartManagerPatch
{
    [HarmonyPatch(nameof(GameStartManager.Update)), HarmonyPrefix]
    public static void Prefix_Update(GameStartManager __instance)
    {
        if (__instance == null) return;
        __instance.MinPlayers = 1;
    }
    // Vanilla players getting error when trying get dleks map icon
    [HarmonyPatch(nameof(GameStartManager.Start)), HarmonyPostfix]
    public static void Postfix_AllMapIcons(GameStartManager __instance)
    {
        if (__instance == null) return;

        if (GameStates.IsNormalGame && Main.NormalOptions.MapId == 3)
        {
            Main.NormalOptions.MapId = 0;
            __instance.UpdateMapImage(MapNames.Skeld);
        }
        else if (GameStates.IsHideNSeek && Main.HideNSeekOptions.MapId == 3)
        {
            Main.HideNSeekOptions.MapId = 0;
            __instance.UpdateMapImage(MapNames.Skeld);
        }

        MapIconByName DleksIncon = Object.Instantiate(__instance, __instance.gameObject.transform).AllMapIcons[0];
        DleksIncon.Name = MapNames.Dleks;
        DleksIncon.MapImage = Utils.LoadSprite($"UnlockDleks.Resources.Images.DleksBanner.png", 100f);
        DleksIncon.NameImage = Utils.LoadSprite($"UnlockDleks.Resources.Images.DleksBanner-Wordart.png", 100f);

        __instance.AllMapIcons.Add(DleksIncon);
    }
    [HarmonyPatch(nameof(GameStartManager.BeginGame)), HarmonyPostfix]
    public static void Postfix_BeginGame(GameStartManager __instance)
    {
        if (__instance == null) return;

        if (GameOptionsMapPickerPatch.SetDleks)
        {
            if (GameStates.IsNormalGame)
                Main.NormalOptions.MapId = 3;

            else if (GameStates.IsHideNSeek)
                Main.HideNSeekOptions.MapId = 3;
        }
    }
}
