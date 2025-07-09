
using HarmonyLib;
using KMod;
using PeterHan.PLib.Core;
using PeterHan.PLib.Options;
using STRINGS;
using System;
using System.Collections.Generic;
using System.Linq;
using TUNING;
using UnityEngine;
using static GeoTuner_mod.UI.UISIDESCREENS;
namespace GeoTuner_mod
{

    internal class HarmonyPatches : UserMod2
    {
        public override void OnLoad(Harmony harmony)
        {

            PUtil.InitLibrary(false);
            new POptions().RegisterOptions(this, typeof(Options));
            base.OnLoad(harmony);
           // LocString.CreateLocStringKeys(typeof(UI), "STRINGS.");

#if DEBUG
            ModUtil.RegisterForTranslation(typeof(UI));
#else
            Localization.RegisterForTranslation(typeof(UI));
#endif

       

        }
    }

        internal class Patches
    {


        private static Options Option = SingletonOptions<Options>.Instance;

       
        [HarmonyPatch(typeof(GeoTunerSideScreen), "SetRow")]
        public static class GeoTunerSideScreen_WSetRow
        {
 

            public static void Postfix(GeoTunerSideScreen __instance, int idx, Geyser geyser,bool studied, GeoTuner.Instance ___targetGeotuner)
            {
                GameObject gameObject;
                bool flag = geyser == null;

                if (idx < __instance.rowContainer.childCount)
                {
                    gameObject = __instance.rowContainer.GetChild(idx).gameObject;
                }
                else
                {
                    gameObject = Util.KInstantiateUI(__instance.rowPrefab, __instance.rowContainer.gameObject, true);
                }
                ToolTip[] componentsInChildren = gameObject.GetComponentsInChildren<ToolTip>();
                ToolTip toolTip = componentsInChildren.First<ToolTip>();
                bool usingStudiedTooltip = geyser != null && (flag || studied);
                int geotunedCount = Components.GeoTuners.GetItems(___targetGeotuner.GetMyWorldId()).Count((GeoTuner.Instance x) => x.GetFutureGeyser() == geyser || x.GetAssignedGeyser() == geyser);

                toolTip.OnToolTip = delegate ()
                {
                    if (!usingStudiedTooltip)
                    {
                        return STRINGS.UI.UISIDESCREENS.GEOTUNERSIDESCREEN.UNSTUDIED_TOOLTIP.ToString();
                    }
                    if (geyser != ___targetGeotuner.GetFutureGeyser() && geotunedCount >= 1)
                    {
                        return STRINGS.UI.UISIDESCREENS.GEOTUNERSIDESCREEN.GEOTUNER_LIMIT_TOOLTIP.ToString();
                    }
                    Func<float, float> func = delegate (float emissionPerCycleModifier)
                    {
                        float num3 = 600f / geyser.configuration.GetIterationLength();
                        return emissionPerCycleModifier / num3 / geyser.configuration.GetOnDuration();
                    };
                    
                    Func<float, float, float, float> func2 = delegate (float iterationLength, float massPerCycle, float eruptionDuration)
                    {
                        float num3 = 600f / iterationLength;
                        return massPerCycle / num3 / eruptionDuration;
                    };
                    GeoTunerConfig.GeotunedGeyserSettings settingsForGeyser = ___targetGeotuner.def.GetSettingsForGeyser(geyser);
                    GeoTunerAdjustable  ad = ___targetGeotuner.gameObject.AddOrGet<GeoTunerAdjustable>();
                    float max_geotuned = 1 * Option.Geyser_Ratio;
                    if (ad != null)
                    {
                        max_geotuned *= ad.UserMaxCapacity;

                    }

                    float num = (Geyser.temperatureModificationMethod == Geyser.ModificationMethod.Percentages) ? (settingsForGeyser.template.temperatureModifier * geyser.configuration.geyserType.temperature * max_geotuned) : settingsForGeyser.template.temperatureModifier * max_geotuned;
                    float num2 = func((Geyser.massModificationMethod == Geyser.ModificationMethod.Percentages) ? (settingsForGeyser.template.massPerCycleModifier * geyser.configuration.scaledRate * max_geotuned ) : settingsForGeyser.template.massPerCycleModifier * max_geotuned);
                    func2(geyser.configuration.scaledIterationLength, geyser.configuration.scaledRate, geyser.configuration.scaledIterationLength * geyser.configuration.scaledIterationPercent);
                    string str = ((num > 0f) ? "+" : "") + GameUtil.GetFormattedTemperature(num, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Relative, true, false);
                    string str2 = ((num2 > 0f) ? "+" : "") + GameUtil.GetFormattedMass(num2, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.##}");
                    string newValue = settingsForGeyser.material.ProperName();
                    return (STRINGS.UI.UISIDESCREENS.GEOTUNERSIDESCREEN.STUDIED_TOOLTIP + "\n" + "\n" + STRINGS.UI.UISIDESCREENS.GEOTUNERSIDESCREEN.STUDIED_TOOLTIP_MATERIAL).Replace("{MATERIAL}", newValue) + "\n" + str + "\n" + str2 + "\n" + "\n" + STRINGS.UI.UISIDESCREENS.GEOTUNERSIDESCREEN.STUDIED_TOOLTIP_VISIT_GEYSER;
                };

                MultiToggle component2 = gameObject.GetComponent<MultiToggle>();
                component2.onClick = delegate ()
                {
                    if (geyser == null || geyser.GetComponent<Studyable>().Studied)
                    {
                        if (geyser == ___targetGeotuner.GetFutureGeyser())
                        {
                            return;
                        }
                        IEnumerable<GeoTuner.Instance> items = Components.GeoTuners.GetItems(___targetGeotuner.GetMyWorldId());
                        Func<GeoTuner.Instance, bool> predicate = ((GeoTuner.Instance x) => x.GetAssignedGeyser() == geyser || x.GetFutureGeyser() == geyser);
                        
                        int num = items.Count(predicate);
                        if (geyser != null && num + 1 > 1)
                        {
                            return;
                        }
                        ___targetGeotuner.AssignFutureGeyser(geyser);
                        Traverse.Create(__instance).Method("RefreshOptions").GetValue();

                    }
                };
            }
        }

        [HarmonyPatch(typeof(GeoTunerConfig), "ConfigureBuildingTemplate")]

        private static class Patch_AirConditionerConfig_ConfigureBuildingTemplate
        {
            public static void Postfix(GameObject go)
            {
                go.AddOrGet<GeoTunerAdjustable>();
            }
        }


    

        [HarmonyPatch(typeof(GeoTuner.Instance), "RefreshModification")]

        private static class Patch_GeoTunerInstance_RefreshModification
        {
            public static bool Prefix(GeoTuner.Instance __instance)
            {

           


                Geyser assignedGeyser = __instance.GetAssignedGeyser();

                GeoTunerAdjustable ad = __instance.gameObject.AddOrGet<GeoTunerAdjustable>();
                if (ad == null)
                {
                   // global::Debug.Log("协调RefreshModificationad 为空");

                    return true;
                }


                if (assignedGeyser != null)
                {
                    GeoTunerConfig.GeotunedGeyserSettings settingsForGeyser = __instance.def.GetSettingsForGeyser(assignedGeyser);
                    __instance.currentGeyserModification = settingsForGeyser.template;
                    __instance.currentGeyserModification.originID = __instance.originID;
                    __instance.enhancementDuration = settingsForGeyser.duration;
                    __instance.currentGeyserModification.massPerCycleModifier = __instance.currentGeyserModification.massPerCycleModifier * Option.Geyser_Ratio * ad.UserMaxCapacity;
                    __instance.currentGeyserModification.temperatureModifier = __instance.currentGeyserModification.temperatureModifier * Option.Geyser_Ratio * ad.UserMaxCapacity;
                    assignedGeyser.Trigger(1763323737, null);
                }


                Traverse.Create(typeof(GeoTuner)).Method("RefreshStorageRequirements", new Type[] { typeof(GeoTuner.Instance) }).GetValue(new object[] { __instance });
                Traverse.Create(typeof(GeoTuner)).Method("DropStorageIfNotMatching", new Type[] { typeof(GeoTuner.Instance) }).GetValue(new object[] { __instance });

                return false;
            }


        }


        [HarmonyPatch(typeof(GeoTuner.Instance), "RefreshModification")]

        private static class Patch_GeoTuner_RefreshModification
        {
            public static void Postfix(GeoTuner.Instance __instance)
            {


                Geyser assignedGeyser = __instance.GetAssignedGeyser();
                if (assignedGeyser == null)
                {

                    return;
                }
                GeoTunerAdjustable ad = __instance.gameObject.AddOrGet<GeoTunerAdjustable>();
                if (ad == null)
                {
                    //global::Debug.Log("RefreshModification 为空");

                    return;
                }
                __instance.manualDelivery.AbortDelivery("Switching to new delivery request");

                float quantity = Option.Geotuners_Ratio * ad.UserMaxCapacity;
                __instance.storage.capacityKg *= quantity;
                __instance.manualDelivery.capacity *= quantity;
                __instance.manualDelivery.refillMass *= quantity;
                __instance.manualDelivery.MinimumMass *= quantity;

            }


        }

        [HarmonyPatch(typeof(Geyser), "GetDescriptors")]

        private static class Patch_Geyser_GetDescriptors
        {
            public static void Postfix(Geyser __instance, GameObject go, ref List<Descriptor> __result)
            {
                List<GeoTuner.Instance> items = Components.GeoTuners.GetItems(__instance.gameObject.GetMyWorldId());
                List<GeoTuner.Instance> GeoTuners = items.FindAll((GeoTuner.Instance x) => x.GetAssignedGeyser() == __instance);
                int num = GeoTuners.Count;
                bool flag = num > 0;
                if (!flag)
                {
                    return;

                }
                float count=0;
                foreach (GeoTuner.Instance i in GeoTuners)
                {
                     count += i.GetComponent<GeoTunerAdjustable>().UserMaxCapacity;
                }


                string text = string.Format(STRINGS.UI.BUILDINGEFFECTS.TOOLTIPS.GEYSER_PRODUCTION_GEOTUNED, ElementLoader.FindElementByHash(__instance.configuration.GetElement()).name, 
                    GameUtil.GetFormattedMass(__instance.configuration.GetEmitRate(), GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"), 
                    GameUtil.GetFormattedTemperature(__instance.configuration.GetTemperature(), GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false));
                text += "\n" + string.Format(UI.UISIDESCREENS.GEOTUNERADJUSTABLE.TOOLTIP, count, num);
                text += "\n" + UI.UISIDESCREENS.GEOTUNERADJUSTABLE.NODETAILS;

                string arg = ElementLoader.FindElementByHash(__instance.configuration.GetElement()).tag.ProperName();

                __result[0] = new Descriptor(string.Format(STRINGS.UI.BUILDINGEFFECTS.GEYSER_PRODUCTION, arg, GameUtil.GetFormattedMass(__instance.configuration.GetEmitRate(),
                    GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"), GameUtil.GetFormattedTemperature(__instance.configuration.GetTemperature(),
                    GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)), text, Descriptor.DescriptorType.Effect, false);
            }
        }
    }
}
