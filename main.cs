using BepInEx;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using System;
using System.Reflection;
using System.Collections;
using UnityEngine;
using BepInEx.Unity.IL2CPP.Utils.Collections;

[assembly: AssemblyFileVersion(UnlockDleks.Main.PluginVersion)]
[assembly: AssemblyInformationalVersion(UnlockDleks.Main.PluginVersion)]
[assembly: AssemblyVersion(UnlockDleks.Main.PluginVersion)]
namespace UnlockDleks;

[BepInPlugin(PluginGuid, "UnlockDleks", PluginVersion)]
[BepInIncompatibility("com.0xdrmoe.townofhostenhanced")]
[BepInProcess("Among Us.exe")]

public class Main : BasePlugin
{
    public static readonly string ModName = "Unlock Dleks";
    public static readonly string ForkId = "Unlock Dleks";

    public const string PluginVersion = "2.1.0";
    public const string PluginDisplayVersion = "2.1.0";
    public const string PluginGuid = "com.tommyxl.unlockdleksehT";

    public static Main Instance;
    private Coroutines coroutines;

    public Harmony Harmony { get; } = new Harmony(PluginGuid);
    public Version version = Version.Parse(PluginVersion);
    public static BepInEx.Logging.ManualLogSource Logger;

    public override void Load()
    {
        Instance = this;
        coroutines = AddComponent<Coroutines>();
        Logger = BepInEx.Logging.Logger.CreateLogSource("UnlockDleks");

        Harmony.PatchAll();
    }

    public Coroutine StartCoroutine(IEnumerator coroutine)
    {
        if (coroutine == null) return null;
        return coroutines.StartCoroutine(coroutine.WrapToIl2Cpp());
    }

    public void StopCoroutine(IEnumerator coroutine)
    {
        if (coroutine == null) return;
        coroutines.StopCoroutine(coroutine.WrapToIl2Cpp());
    }

    public void StopCoroutine(Coroutine coroutine)
    {
        if (coroutine == null) return;
        coroutines.StopCoroutine(coroutine);
    }

    public void StopAllCoroutines()
    {
        coroutines.StopAllCoroutines();
    }
    public class Coroutines : MonoBehaviour { }
}