using HarmonyLib;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppSystem.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;

namespace UnlockDleks.Patches;

//[HarmonyPatch(typeof(GameSettingMenu), nameof(GameSettingMenu.InitializeOptions))]
//public static class GameSettingMenuPatch
//{
//    // Add Dleks to map selection
//    public static void Postfix([HarmonyArgument(0)] Il2CppReferenceArray<Transform> items)
//    {
//        items
//            .FirstOrDefault(i => i.gameObject.activeSelf && i.name.Equals("MapName", StringComparison.OrdinalIgnoreCase))
//            ?.GetComponent<KeyValueOption>()
//            ?.Values
//            // using .Insert will convert managed values and break the struct resulting in crashes
//            ?.System_Collections_IList_Insert((int)MapNames.Dleks, new KeyValuePair<string, int>(Constants.MapNames[(int)MapNames.Dleks], (int)MapNames.Dleks));
//    }
//}
