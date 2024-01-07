using HarmonyLib;

namespace UnlockDleks.Patches;

[HarmonyPatch(typeof(KeyValueOption), nameof(KeyValueOption.OnEnable))]
class AutoSelectDleksPatch
{
    private static void Postfix(KeyValueOption __instance)
    {
        if (__instance.Title == StringNames.GameMapName)
        {
            // vanilla clamps this to not auto select dleks
            __instance.Selected = GameOptionsManager.Instance.CurrentGameOptions.MapId;
        }
    }
}
