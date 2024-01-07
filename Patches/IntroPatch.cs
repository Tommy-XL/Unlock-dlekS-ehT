using HarmonyLib;
using UnlockDleks.Modules;

namespace UnlockDleks.Patches;

[HarmonyPatch(typeof(IntroCutscene), nameof(IntroCutscene.OnDestroy))]
class IntroCutsceneDestroyPatch
{
    public static void Postfix()
    {
        if (!GameStates.IsInGame) return;
        GameStates.introDestroyed = true;
    }
}

[HarmonyPatch(typeof(IntroCutscene), nameof(IntroCutscene.CoBegin))]
class CoBeginPatch
{
    public static void Prefix()
    {
        GameStates.IsInGame = true;
    }
}

[HarmonyPatch(typeof(AmongUsClient), nameof(AmongUsClient.CoStartGame))]
class OnGameStartedPatch
{
    public static void Postfix()
    {
        GameStates.introDestroyed = false;
    }
}

[HarmonyPatch(typeof(AmongUsClient), nameof(AmongUsClient.OnGameEnd))]
class OnEndGamePatch
{
    public static void Postfix()
    {
        GameStates.IsInGame = false;
    }
}
