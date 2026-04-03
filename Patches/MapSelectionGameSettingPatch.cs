using HarmonyLib;
using System.Linq;

namespace UnlockDleks.Patches;

[HarmonyPatch]
public static class MapSelectionGameSettingPatch
{
    [HarmonyPriority(Priority.VeryLow)]
    [HarmonyPatch(typeof(MapSelectionGameSetting), nameof(MapSelectionGameSetting.GetValueString))]
    [HarmonyPrefix]
    public static void AddToActualOptions(MapSelectionGameSetting __instance)
    {
        if (__instance.Values.All(x => (int)x != (int)GameOptionsMapPickerPatch.MapNameDleks))
        {
            var list = __instance.Values.ToList();
            list.Insert((int)MapNames.Dleks, GameOptionsMapPickerPatch.MapNameDleks);
            __instance.Values = list.ToArray();
        }
    }
}
