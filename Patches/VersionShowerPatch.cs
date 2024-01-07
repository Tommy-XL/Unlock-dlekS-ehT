using HarmonyLib;

namespace UnlockDleks.Patches;

[HarmonyPatch(typeof(VersionShower), nameof(VersionShower.Start))]
public static class VersionShowerPatches
{
    private static void Postfix(VersionShower __instance)
    {
        __instance.text.text += $" + <color=#67D0A0>Unlock Dleks v{Main.PluginDisplayVersion}</color>";
    }
}
