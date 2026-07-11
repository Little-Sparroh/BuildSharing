using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

[BepInPlugin(PluginGUID, PluginName, PluginVersion)]
[BepInDependency("sparroh.uilibrary")]
[MycoMod(null, ModFlags.IsClientSide)]
public class BuildSharingPlugin : BaseUnityPlugin
{
    public const string PluginGUID = "sparroh.buildsharing";
    public const string PluginName = "BuildSharing";
    public const string PluginVersion = "1.0.0";

    internal static ManualLogSource Logger;
    public static BuildSharingPlugin Instance;

    private void Awake()
    {
        Logger = base.Logger;
        Instance = this;

        var harmony = new Harmony(PluginGUID);
        harmony.PatchAll();

        Logger.LogInfo($"{PluginName} v{PluginVersion} loaded successfully.");
    }
}
