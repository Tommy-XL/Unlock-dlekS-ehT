using HarmonyLib;

namespace UnlockDleks.Patches;

[HarmonyPatch(typeof(StringOption), nameof(StringOption.Start))]
class AutoSelectDleksPatch
{
    private static void Postfix(StringOption __instance)
    {
        if (__instance.Title == StringNames.GameMapName)
        {
            // vanilla clamps this to not auto select dleks
            __instance.Value = GameOptionsManager.Instance.CurrentGameOptions.MapId;
        }
    }
}
