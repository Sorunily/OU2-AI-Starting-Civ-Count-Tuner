using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using System;
using System.Reflection;

namespace AIStartingCivCountTuner
{
    [BepInPlugin("AIStartingCivCountTuner", "Starting Civs Tweaks", "1.0.0")]
    public class StartingCivsPlugin : BaseUnityPlugin
    {
        internal static ConfigEntry<int> DesiredAICivs;
        internal static ConfigEntry<bool> ClampToMax;

        private void Awake()
        {
            DesiredAICivs = Config.Bind("General", "DesiredAICivilizationCount", 33,
                "How many AI civilizations should start the game?");
            ClampToMax = Config.Bind("General", "ClampToMaxCivilizationCount", true,
                "If true, will clamp to the game's MaxCivilizationCount; if false, will force even if higher.");

            var harmony = new Harmony("your.id.startingcivs");
            harmony.PatchAll();
            Logger.LogInfo($"Starting Civs Tweaks loaded. Target AI civs = {DesiredAICivs.Value}");
        }
    }

    [HarmonyPatch]
    internal static class InitialSituation_SetAICivilizations_Patch
    {
        static MethodBase TargetMethod()
        {
            var t = AccessTools.TypeByName("OU2.Engine.World.Generation.InitialSituation");
            if (t == null) throw new Exception("Type not found: OU2.Engine.World.Generation.InitialSituation");
            var m = AccessTools.Method(t, "SetAICivilizations", new Type[0]);
            if (m == null) throw new Exception("Method not found: InitialSituation.SetAICivilizations()");
            return m;
        }

        static void Postfix(object __instance)
        {
            try
            {
                var prop = AccessTools.Property(__instance.GetType(), "AICivilizationCount");
                if (prop != null && prop.CanWrite)
                {
                    int newCount = StartingCivsPlugin.DesiredAICivs.Value;

                    if (StartingCivsPlugin.ClampToMax.Value)
                    {
                        var maxProp = AccessTools.Property(__instance.GetType(), "MaxCivilizationCount");
                        if (maxProp != null && maxProp.CanRead)
                        {
                            int max = (int)maxProp.GetValue(__instance, null);
                            if (newCount > max) newCount = max;
                        }
                    }

                    prop.SetValue(__instance, newCount, null);
                }
                else
                {
                    var fld = AccessTools.Field(__instance.GetType(), "AICivilizationCount");
                    if (fld == null) fld = AccessTools.Field(__instance.GetType(), "_AICivilizationCount");
                    if (fld == null) throw new Exception("AICivilizationCount property/field not found.");

                    int newCount = StartingCivsPlugin.DesiredAICivs.Value;

                    if (StartingCivsPlugin.ClampToMax.Value)
                    {
                        var maxProp = AccessTools.Property(__instance.GetType(), "MaxCivilizationCount");
                        if (maxProp != null && maxProp.CanRead)
                        {
                            int max = (int)maxProp.GetValue(__instance, null);
                            if (newCount > max) newCount = max;
                        }
                    }

                    fld.SetValue(__instance, newCount);
                }
            }
            catch (Exception e)
            {
                BepInEx.Logging.Logger.CreateLogSource("StartingCivsPlugin")
                    .LogError($"Failed to set AI civ count: {e}");
            }
        }
    }
}
