﻿using HarmonyLib;
using UnityEngine;

namespace UnlockDleks.Patches;

// Thanks Galster (https://github.com/Galster-dev)
[HarmonyPatch(typeof(AmongUsClient._CoStartGameHost_d__37), nameof(AmongUsClient._CoStartGameHost_d__37.MoveNext))]
class CoStartGameHostPatch
{
    private static bool Prefix(AmongUsClient._CoStartGameHost_d__37 __instance, ref bool __result)
    {
        if (__instance.__1__state != 0)
        {
            return true;
        }

        __instance.__1__state = -1;
        if (LobbyBehaviour.Instance)
        {
            LobbyBehaviour.Instance.Despawn();
        }

        if (ShipStatus.Instance)
        {
            __instance.__2__current = null;
            __instance.__1__state = 2;
            __result = true;
            return false;
        }

        // removed dleks check as it's always false
        var num2 = Mathf.Clamp(GameOptionsManager.Instance.CurrentGameOptions.MapId, 0, Constants.MapNames.Length - 1);
        __instance.__2__current = __instance.__4__this.ShipLoadingAsyncHandle = __instance.__4__this.ShipPrefabs[num2].InstantiateAsync();
        __instance.__1__state = 1;

        __result = true;
        return false;
    }
}
