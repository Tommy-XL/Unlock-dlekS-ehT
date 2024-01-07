using BepInEx;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using System;
using System.Reflection;

[assembly: AssemblyFileVersion(UnloacDleks.Main.PluginVersion)]
[assembly: AssemblyInformationalVersion(UnloacDleks.Main.PluginVersion)]
[assembly: AssemblyVersion(UnloacDleks.Main.PluginVersion)]
namespace UnloacDleks;

[BepInPlugin(PluginGuid, "UnloacDleks", PluginVersion)]
[BepInIncompatibility("com.0xdrmoe.townofhostenhanced")]
[BepInProcess("Among Us.exe")]

public class Main : BasePlugin
{
    public static readonly string ModName = "Unloac Dleks";
    public static readonly string ForkId = "Unloac Dleks";

    public const string PluginVersion = "1.0.0";
    public const string PluginDisplayVersion = "1.0.0";
    public const string PluginGuid = "com.tommyxl.unloacdlekSehT";

    public static Main Instance;

    public Harmony Harmony { get; } = new Harmony(PluginGuid);
    public static Version version = Version.Parse(PluginVersion);
    public static BepInEx.Logging.ManualLogSource Logger;

    public override void Load()
    {
        Instance = this;

        Harmony.PatchAll();
    }
}