using HarmonyLib;
using System;
using UnityEngine;
using UnlockDleks.Modules;

namespace UnlockDleks.Patches;

[HarmonyPatch(typeof(GameOptionsMapPicker))]
public static class GameOptionsMapPickerPatch
{
    public static bool SetDleks = false;
    private static MapSelectButton DleksButton;
    [HarmonyPatch(nameof(GameOptionsMapPicker.Initialize))]
    [HarmonyPostfix]
    public static void Postfix_Initialize(GameOptionsMapPicker __instance)
    {
        int DleksPos = 3;

        MapSelectButton[] AllMapButton = __instance.transform.GetComponentsInChildren<MapSelectButton>();

        if (AllMapButton != null)
        {
            GameObject dlekS_ehT = UnityEngine.Object.Instantiate(AllMapButton[0].gameObject, __instance.transform);
            dlekS_ehT.transform.position = AllMapButton[DleksPos].transform.position;
            dlekS_ehT.transform.SetSiblingIndex(DleksPos + 2);
            MapSelectButton dlekS_ehT_MapButton = dlekS_ehT.GetComponent<MapSelectButton>();
            DleksButton = dlekS_ehT_MapButton;
            dlekS_ehT_MapButton.MapIcon.transform.localScale = new Vector3(-1f, 1f, 1f);
            dlekS_ehT_MapButton.Button.OnClick.RemoveAllListeners();
            dlekS_ehT_MapButton.Button.OnClick.AddListener((Action)(() =>
            {
                __instance.SelectMap(__instance.AllMapIcons[0]);

                if (__instance.selectedButton)
                {
                    __instance.selectedButton.Button.SelectButton(false);
                }
                __instance.selectedButton = dlekS_ehT_MapButton;
                __instance.selectedButton.Button.SelectButton(true);
                __instance.selectedMapId = 3;

                if (GameStates.IsNormalGame)
                    Main.NormalOptions.MapId = 0;
                else if (GameStates.IsHideNSeek)
                    Main.HideNSeekOptions.MapId = 0;

                //__instance.MapImage.transform.localScale = new Vector3(-1f, 1f, 1f);
                //__instance.MapName.transform.localScale = new Vector3(-1f, 1f, 1f);

                __instance.MapImage.sprite = Utils.LoadSprite($"UnlockDleks.Resources.Images.DleksBanner.png", 100f);
                __instance.MapName.sprite = Utils.LoadSprite($"UnlockDleks.Resources.Images.DleksBanner-Wordart.png", 100f);
            }));

            for (int i = DleksPos; i < AllMapButton.Length; i++)
            {
                AllMapButton[i].transform.localPosition += new Vector3(0.625f, 0f, 0f);
            }

            if (DleksButton != null)
            {
                if (SetDleks)
                {
                    if (__instance.selectedButton)
                    {
                        __instance.selectedButton.Button.SelectButton(false);
                    }
                    DleksButton.Button.SelectButton(true);
                    __instance.selectedButton = DleksButton;
                    __instance.selectedMapId = 3;

                    //__instance.MapImage.transform.localScale = new Vector3(-1f, 1f, 1f);
                    //__instance.MapName.transform.localScale = new Vector3(-1f, 1f, 1f);

                    __instance.MapImage.sprite = Utils.LoadSprite($"UnlockDleks.Resources.Images.DleksBanner.png", 100f);
                    __instance.MapName.sprite = Utils.LoadSprite($"UnlockDleks.Resources.Images.DleksBanner-Wordart.png", 100f);
                }
                else
                {
                    DleksButton.Button.SelectButton(false);
                }
            }
        }
    }

    [HarmonyPatch(nameof(GameOptionsMapPicker.FixedUpdate))]
    [HarmonyPrefix]
    public static bool Prefix_FixedUpdate(GameOptionsMapPicker __instance)
    {
        if (__instance == null) return true;

        if (DleksButton != null)
        {
            if (__instance.selectedMapId == 3)
            {
                SetDleks = true;
            }
            else
            {
                SetDleks = false;
            }
        }

        if (__instance.selectedMapId == 3)
            return false;

        return true;
    }
}