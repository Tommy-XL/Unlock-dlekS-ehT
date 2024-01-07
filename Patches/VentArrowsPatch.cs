using HarmonyLib;
using UnlockDleks.Modules;

namespace UnlockDleks.Patches;

[HarmonyPatch(typeof(Vent), nameof(Vent.SetButtons))]
public static class VentSetButtonsPatch
{
    public static bool ShowButtons = false;
    // Fix arrows buttons in vent on Dleks map and "Index was outside the bounds of the array" errors
    private static bool Prefix(Vent __instance, [HarmonyArgument(0)] ref bool enabled)
    {
        // if map is Dleks
        if (GameStates.DleksIsActive && GameStates.introDestroyed)
        {
            enabled = false;
            if (GameStates.IsMeeting)
                ShowButtons = false;
        }
        return true;
    }
    public static void Postfix(Vent __instance, [HarmonyArgument(0)] bool enabled)
    {
        if (!GameStates.DleksIsActive) return;
        if (enabled || !GameStates.introDestroyed) return;

        var setActive = ShowButtons || !PlayerControl.LocalPlayer.inVent && !GameStates.IsMeeting;
        switch (__instance.Id)
        {
            case 0:
            case 1:
            case 2:
            case 3:
            case 5:
            case 6:
                __instance.Buttons[0].gameObject.SetActive(setActive);
                __instance.Buttons[1].gameObject.SetActive(setActive);
                break;
            case 7:
            case 12:
            case 13:
                __instance.Buttons[0].gameObject.SetActive(setActive);
                break;
            case 4:
            case 8:
            case 9:
            case 10:
            case 11:
                __instance.Buttons[1].gameObject.SetActive(setActive);
                break;
        }
    }
}
[HarmonyPatch(typeof(Vent), nameof(Vent.TryMoveToVent))]
class VentTryMoveToVentPatch
{
    // Update arrows buttons when player move to vents
    private static void Postfix(Vent __instance, [HarmonyArgument(0)] Vent otherVent)
    {
        if (__instance == null || otherVent == null || !GameStates.DleksIsActive) return;

        VentSetButtonsPatch.ShowButtons = true;
        VentSetButtonsPatch.Postfix(otherVent, false);
        VentSetButtonsPatch.ShowButtons = false;
    }
}
[HarmonyPatch(typeof(Vent), nameof(Vent.UpdateArrows))]
class VentUpdateArrowsPatch
{
    // Fixes "Index was outside the bounds of the array" errors when arrows updates in vent on Dleks map
    private static bool Prefix()
    {
        // if map is not Dleks
        return !GameStates.DleksIsActive;
    }
}