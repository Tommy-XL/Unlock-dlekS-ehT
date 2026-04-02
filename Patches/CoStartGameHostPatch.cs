//using HarmonyLib;
//using Il2CppInterop.Runtime.InteropTypes;
//using System.Reflection;
//using UnityEngine;
//using UnityEngine.ResourceManagement.AsyncOperations;
//using UnlockDleks.Modules;

//namespace UnlockDleks.Patches;

//[HarmonyPatch]
//class CoStartGameHostPatch
//{
//    static MethodBase TargetMethod()
//    {
//        return Utils.GetStateMachineMoveNext<AmongUsClient>(nameof(AmongUsClient.CoStartGameHost))!;
//    }

//    static bool Prefix(Il2CppObjectBase __instance, ref bool __result)
//    {
//        var sm = new StateMachineWrapper<AmongUsClient>(__instance);

//        // __instance.__1__state != 0
//        if (sm.State != 0)
//        {
//            return true;
//        }
//        // __instance.__1__state = -1;
//        sm.State = -1;

//        if (LobbyBehaviour.Instance)
//        {
//            LobbyBehaviour.Instance.Despawn();
//        }

//        if (ShipStatus.Instance)
//        {
//            // __instance.__2__current = null;
//            sm.SetParameter<AsyncOperationHandle<GameObject>>("__2__current", default);
//            // __instance.__1__state = 2;
//            sm.State = 2;

//            __result = true;
//            return false;
//        }

//        var num2 = Mathf.Clamp(GameOptionsManager.Instance.CurrentGameOptions.MapId, 0, Constants.MapNames.Length - 1);
//        var client = sm.Instance; // __4__this
//        var handle = client.ShipPrefabs[num2].InstantiateAsync();

//        // __instance.__4__this.ShipLoadingAsyncHandle = ...
//        client.ShipLoadingAsyncHandle = handle;
//        // __instance.__2__current = ...
//        sm.SetParameter("__2__current", handle);

//        // __instance.__1__state = 1;
//        sm.State = 1;

//        __result = true;
//        return false;
//    }
//}

using BepInEx.Unity.IL2CPP.Utils.Collections;
using HarmonyLib;
using Il2CppSystem.Collections;
using InnerNet;
using UnityEngine;
using System;

namespace UnlockDleks.Patches;

[HarmonyPatch]
class StartGameHostPatch
{
    [HarmonyPatch(typeof(AmongUsClient), nameof(AmongUsClient.CoStartGameHost))]
    [HarmonyPrefix]
    [HarmonyPriority(Priority.First)]
    public static bool CoStartGameHost_Prefix(AmongUsClient __instance, ref IEnumerator __result)
    {
        __result = CoStartGameHostPatch().WrapToIl2Cpp();
        return false;

        System.Collections.IEnumerator CoStartGameHostPatch()
        {
            DestroyableSingleton<LoadingBarManager>.Instance.ToggleLoadingBar(true);
            DestroyableSingleton<LoadingBarManager>.Instance.SetLoadingPercent(0f, StringNames.LoadingBarGameStart);

            if (LobbyBehaviour.Instance)
                LobbyBehaviour.Instance.Despawn();

            if (!ShipStatus.Instance)
            {
                int index = Mathf.Clamp(GameOptionsManager.Instance.CurrentGameOptions.MapId, 0, Constants.MapNames.Length - 1);
                __instance.ShipLoadingAsyncHandle = __instance.ShipPrefabs[index].InstantiateAsync();

                while (!__instance.ShipLoadingAsyncHandle.IsDone)
                {
                    float progress = __instance.ShipLoadingAsyncHandle.PercentComplete;
                    float displayPercent = Mathf.Lerp(0f, 10f, progress);
                    DestroyableSingleton<LoadingBarManager>.Instance.SetLoadingPercent(displayPercent, StringNames.LoadingBarGameStart);
                    yield return null;
                }

                GameObject result = __instance.ShipLoadingAsyncHandle.Result;
                ShipStatus.Instance = result.GetComponent<ShipStatus>();
                __instance.Spawn(ShipStatus.Instance);
            }
            DateTime start = DateTime.Now;
            while (true)
            {
                var flag = true;
                var maxTime = 10;
                var totalSeconds = (float)(DateTime.Now - start).TotalSeconds;

                if (GameOptionsManager.Instance.CurrentGameOptions.MapId == 5 || GameOptionsManager.Instance.CurrentGameOptions.MapId == 4)
                    maxTime = 15;

                lock (__instance.allClients)
                {
                    for (var index = 0; index < __instance.allClients.Count; ++index)
                    {
                        ClientData clientData = __instance.allClients[index];
                        if (clientData.Id != __instance.ClientId && !clientData.IsReady)
                        {
                            if (totalSeconds < (double)maxTime)
                                flag = false;
                            else
                            {
                                __instance.SendLateRejection(clientData.Id, DisconnectReasons.ClientTimeout);
                                clientData.IsReady = true;
                                __instance.OnPlayerLeft(clientData, DisconnectReasons.ClientTimeout);
                            }
                        }
                    }
                }
                if (totalSeconds > 1f && totalSeconds < maxTime)
                {
                    DestroyableSingleton<LoadingBarManager>.Instance.ToggleLoadingBar(true);
                    DestroyableSingleton<LoadingBarManager>.Instance.SetLoadingPercent((float)(totalSeconds / (double)maxTime * 100f), StringNames.LoadingBarGameStartWaitingPlayers);
                }
                if (flag) break;
                yield return new WaitForEndOfFrame();
            }
            DestroyableSingleton<LoadingBarManager>.Instance.ToggleLoadingBar(false);
            DestroyableSingleton<RoleManager>.Instance.SelectRoles();
            ShipStatus.Instance.Begin();
            __instance.SendClientReady();
            yield break;
        }
    }
}
