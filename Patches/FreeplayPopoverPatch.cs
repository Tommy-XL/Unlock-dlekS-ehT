using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UnlockDleks.Patches;

// From: https://github.com/Gurge44/Submerged-For-EHR/blob/8edcbb8e9738feb89447160018dc2909fd037f64/Submerged/UI/Patches/MapSelectButtonPatches.cs#L27

[HarmonyPatch]
public static class FreeplayPopoverPatch
{
    private static FreeplayPopover _lastInstance;

    [HarmonyPatch(typeof(FreeplayPopover), nameof(FreeplayPopover.Show))]
    [HarmonyPrefix]
    [HarmonyPriority(Priority.Last)] // Firt need load Submerget
    public static void Prefix_AdjustFreeplayMenuPatch(FreeplayPopover __instance)
    {
        // Prevent double loaded
        if (_lastInstance == __instance) return;
        _lastInstance = __instance;

        FreeplayPopoverButton skeldButton = __instance.buttons[0];
        FreeplayPopoverButton cloneButton = __instance.buttons[4]; // Fungle
        FreeplayPopoverButton dleksButton = Object.Instantiate(cloneButton, cloneButton.transform.parent);

        dleksButton.name = "DleksButton";
        dleksButton.map = MapNames.Dleks;
        var dleksSpriteRenderer = dleksButton.GetComponent<SpriteRenderer>();
        dleksSpriteRenderer.sprite = skeldButton.GetComponent<SpriteRenderer>().sprite;
        dleksSpriteRenderer.flipX = true;
        dleksButton.OnPressEvent = cloneButton.OnPressEvent;

        cloneButton.transform.position = new(__instance.buttons[0].transform.position.x,
            cloneButton.transform.position.y,
            cloneButton.transform.position.z);
        dleksButton.transform.position = new(__instance.buttons[1].transform.position.x,
            dleksButton.transform.position.y,
            dleksButton.transform.position.z);

        SwapPositionsTroll(cloneButton, dleksButton);

        __instance.buttons = new List<FreeplayPopoverButton>(__instance.buttons) { dleksButton }.ToArray();

        // if Submerget are loaded
        if (__instance.buttons.Count >= 7)
        {
            SwapPositionsTroll(__instance.buttons[6], __instance.buttons[3]); // Dleks swap Airship
            SwapPositionsTroll(__instance.buttons[5], cloneButton); // Submerget swap Fubgle

            cloneButton.transform.localPosition = new(0f, -1.5f, cloneButton.transform.position.z); // Set new position for Fungle
        }
        else
            SwapPositionsTroll(__instance.buttons[5], __instance.buttons[3]); // Dleks swap Airship
    }
    [HarmonyPatch(typeof(FreeplayPopover), nameof(FreeplayPopover.Show))]
    [HarmonyPostfix]
    public static void Postfix_AdjustFreeplayMenuPatch(FreeplayPopover __instance)
    {
        foreach (var button in __instance.buttons)
        {
            Main.Logger.LogInfo($"{button.name}");
        }
    }

    private static void SwapPositionsTroll(Component one, Component two)
    {
        (one.transform.position, two.transform.position) = (two.transform.position, one.transform.position);
    }
}
