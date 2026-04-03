using HarmonyLib;
using UnlockDleks.Modules;

namespace UnlockDleks.Patches;

// From: https://github.com/AU-Avengers/TOU-Mira/blob/main/TownOfUs/Patches/AprilFools/DleksMapOptionPickerPatches.cs
[HarmonyPatch]
public static class CreateGameOptionsPatch
{
    [HarmonyPatch(typeof(CreateGameOptions), nameof(CreateGameOptions.MapChanged))]
    [HarmonyPrefix]
    public static bool MapChangedPrefix(CreateGameOptions __instance)
    {
        if (__instance.mapPicker.GetSelectedID() is (int)MapNames.Dleks)
        {
            __instance.mapBanner.flipX = false;
            __instance.rendererBGCrewmates.sprite = __instance.bgCrewmates[0];
            __instance.mapBanner.sprite = Utils.LoadSprite("UnlockDleks.Resources.Images.DleksBanner-Wordart.png", 100f);
            __instance.TurnOffCrewmates();
            __instance.currentCrewSprites = __instance.skeldCrewSprites;
            __instance.SetCrewmateGraphic(__instance.capacityOption.Value - 1f);
            return false;
        }

        return true;
    }
    [HarmonyPatch(typeof(CreateGameOptions), nameof(CreateGameOptions.Start))]
    [HarmonyPrefix]
    public static void SetupMapBackground(CreateGameOptions __instance)
    {
        if (__instance.currentCrewSprites == null)
        {
            __instance.mapBanner.sprite = Utils.LoadSprite("UnlockDleks.Resources.Images.DleksBanner-Wordart.png", 100f);
        }
        __instance.currentCrewSprites ??= __instance.skeldCrewSprites;
        __instance.mapTooltips[3] = StringNames.ToolTipSkeld;
    }
}
