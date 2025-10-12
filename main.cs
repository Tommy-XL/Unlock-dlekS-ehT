using AmongUs.GameOptions;
using BepInEx;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using System;
using System.Reflection;

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

    public const string PluginVersion = "1.4.0";
    public const string PluginDisplayVersion = "1.4.0";
    public const string PluginGuid = "com.tommyxl.unlockdleksehT";

    public Main Instance;

    public Harmony Harmony { get; } = new Harmony(PluginGuid);
    public Version version = Version.Parse(PluginVersion);

    public override void Load()
    {
        Instance = this;

        Harmony.PatchAll();
    }
}