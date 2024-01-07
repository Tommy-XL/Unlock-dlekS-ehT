using HarmonyLib;
using UnityEngine;

namespace UnlockDleks.Patches;

[HarmonyPatch(typeof(ModManager), nameof(ModManager.LateUpdate))]
class ModManagerLateUpdatePatch
{
    public static void Prefix(ModManager __instance)
    {
        __instance.ShowModStamp();
    }
    public static void Postfix(ModManager __instance)
    {
        var offset_y = HudManager.InstanceExists ? 1.6f : 1.4f;
        __instance.ModStamp.transform.position = AspectPosition.ComputeWorldPosition(
            __instance.localCamera, AspectPosition.EdgeAlignments.RightTop,
            new Vector3(0.4f, offset_y, __instance.localCamera.nearClipPlane + 0.1f));
    }
}
