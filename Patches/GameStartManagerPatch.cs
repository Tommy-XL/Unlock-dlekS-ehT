using HarmonyLib;
using System.Linq;
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
    [HarmonyPatch(typeof(GameStartManager), nameof(GameStartManager.Start))]
    [HarmonyPriority(Priority.First)]
    [HarmonyPrefix]
    public static void GameStartManagerStart_Prefix(GameStartManager __instance)
    {
        if (__instance.AllMapIcons.ToArray().Any(x => x.Name == MapNames.Dleks)) return;

        __instance.AllMapIcons.Insert((int)MapNames.Dleks, new MapIconByName
        {
            Name = MapNames.Dleks,
            MapIcon = Utils.LoadSprite("UnlockDleks.Resources.Images.DleksBanner-Wordart.png", 160f),
        });
    }
    [HarmonyPatch(typeof(GameStartManager), nameof(GameStartManager.Start))]
    [HarmonyPostfix]
    public static void GameStartManagerStart_Postfix(GameStartManager __instance)
    {
        if (__instance == null) return;

        LateTask.New(() =>
        {
            var normalOptions = GameOptionsManager.Instance.currentNormalGameOptions;
            var hideNSeekOptions = GameOptionsManager.Instance.currentHideNSeekGameOptions;

            if (GameStates.IsNormalGame && normalOptions?.MapId == 3)
                normalOptions.MapId = 0;

            else if (GameStates.IsHideNSeek && hideNSeekOptions?.MapId == 3)
                hideNSeekOptions.MapId = 0;

        }, AmongUsClient.Instance.AmHost ? 1f : 4f, "Set Skeld Icon For Dleks Map");
    }
    [HarmonyPatch(typeof(GameStartManager), nameof(GameStartManager.UpdateMapImage))]
    [HarmonyPrefix]
    public static bool Prefix_UpdateMapImage(GameStartManager __instance)
    {
        if (GameOptionsMapPickerPatch.SetDleks)
        {
            __instance.MapImage.sprite = Utils.LoadSprite("UnlockDleks.Resources.Images.DleksBanner-Wordart.png", 160f);
            return false;
        }
        return true;
    }

    [HarmonyPatch(nameof(GameStartManager.BeginGame)), HarmonyPostfix]
    public static void Postfix_BeginGame(GameStartManager __instance)
    {
        if (__instance == null || !GameOptionsMapPickerPatch.SetDleks) return;

        if (GameStates.IsNormalGame)
            GameOptionsManager.Instance.currentNormalGameOptions.MapId = 3;

        else if (GameStates.IsHideNSeek)
            GameOptionsManager.Instance.currentHideNSeekGameOptions.MapId = 3;
    }
}
