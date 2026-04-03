using HarmonyLib;
using System.Linq;
using System.Reflection;
using UnityEngine.SceneManagement;
using UnlockDleks.Modules;

namespace UnlockDleks.Patches;

// From: https://github.com/AU-Avengers/TOU-Mira/blob/main/TownOfUs/Patches/AprilFools/DleksMapOptionPickerPatches.cs
[HarmonyPatch]
public static class GameOptionsMapPickerPatch
{
    public static bool SetDleks = false;
    public static StringNames MapNameDleks => StringNames.MapNameSkeld;
    //private static MapSelectButton DleksButton;

    [HarmonyPatch(typeof(GameOptionsMapPicker), nameof(GameOptionsMapPicker.SelectMap), typeof(int))]
    [HarmonyPrefix]
    public static void Prefix_SelectMap([HarmonyArgument(0)] ref int mapId)
    {
        if (!SetDleks && mapId == 3)
            mapId = 0;
    }
    [HarmonyPatch(typeof(GameOptionsMapPicker), nameof(GameOptionsMapPicker.SetupMapButtons))]
    [HarmonyPrefix]
    public static void Postfix_Prefix(GameOptionsMapPicker __instance)
    {
        if (__instance.AllMapIcons.ToArray().Any(x => x.Name == MapNames.Dleks)) return;

        __instance.AllMapIcons.Insert((int)MapNames.Dleks, new MapIconByName
        {
            Name = MapNames.Dleks,
            MapImage = Utils.LoadSprite("UnlockDleks.Resources.Images.DleksBanner.png", 100f),
            MapIcon = Utils.LoadSprite("UnlockDleks.Resources.Images.DleksBanner-Icon.png", 95f),
            NameImage = Utils.LoadSprite("UnlockDleks.Resources.Images.DleksBanner-Wordart.png", 160f),
        });
    }
    [HarmonyPatch(typeof(GameOptionsMapPicker), nameof(GameOptionsMapPicker.SetupMapButtons))]
    [HarmonyPostfix]
    public static void Postfix_Initialize(CreateGameMapPicker __instance)
    {
        if (SceneManager.GetActiveScene().name == "FindAGame") return;

        const int dleksPos = 3;

        __instance.mapButtons[dleksPos].Button.OnClick.RemoveAllListeners();
        __instance.mapButtons[dleksPos].Button.OnClick.AddListener((System.Action)(() =>
        {
            __instance.SelectMap(__instance.AllMapIcons[0]);

            if (__instance.selectedButton)
                __instance.selectedButton.Button.SelectButton(false);

            __instance.selectedButton = __instance.mapButtons[dleksPos];
            __instance.selectedButton.Button.SelectButton(true);
            __instance.selectedMapId = dleksPos;

            SetDleks = true;
            
            if (GameStates.IsNormalGame)
                GameOptionsManager.Instance.currentNormalGameOptions.MapId = 0;
            else if (GameStates.IsHideNSeek)
                GameOptionsManager.Instance.currentHideNSeekGameOptions.MapId = 0;

            __instance.MapImage.sprite = Utils.LoadSprite("UnlockDleks.Resources.Images.DleksBanner.png", 100f);
            __instance.MapName.sprite = Utils.LoadSprite("UnlockDleks.Resources.Images.DleksBanner-Wordart.png", 100f);
        }));

        if (__instance.mapButtons[dleksPos] != null)
        {
            if (SetDleks)
            {
                if (__instance.selectedButton)
                    __instance.selectedButton.Button.SelectButton(false);

                __instance.selectedButton = __instance.mapButtons[dleksPos];
                __instance.selectedButton.Button.SelectButton(true);
                __instance.selectedMapId = dleksPos;

                __instance.MapImage.sprite = Utils.LoadSprite("UnlockDleks.Resources.Images.DleksBanner.png", 100f);
                __instance.MapName.sprite = Utils.LoadSprite("UnlockDleks.Resources.Images.DleksBanner-Wordart.png", 100f);
            }
            else
                __instance.mapButtons[dleksPos].Button.SelectButton(false);
        }
    }
    [HarmonyPatch(typeof(GameOptionsMapPicker), nameof(GameOptionsMapPicker.FixedUpdate))]
    [HarmonyPrefix]
    [Obfuscation(Exclude = true)]
    public static bool Prefix_FixedUpdate(GameOptionsMapPicker __instance)
    {
        if (__instance == null) return true;
        if (__instance.MapName == null) return false;

        SetDleks = __instance.selectedMapId == 3;

        if (__instance.selectedMapId == 3)
        {
            if (SceneManager.GetActiveScene().name == "FindAGame")
            {
                __instance.SelectMap(0);
                SetDleks = false;
            }
            return false;
        }

        return true;
    }
}