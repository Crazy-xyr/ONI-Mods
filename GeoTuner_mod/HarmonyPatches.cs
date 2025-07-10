
using HarmonyLib;
using KMod;
using PeterHan.PLib.Core;
using PeterHan.PLib.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
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

            if (SingletonOptions<Options>.Instance.energyConsumer)
            {
                Patches.Patch_EnergyConsumer_WattsNeededWhenActive.Patch(harmony); 
            }

         }
    }

        internal class Patches
    {


        private static Options Option = SingletonOptions<Options>.Instance;

       
        [HarmonyPatch(typeof(GeoTunerSideScreen), "SetRow")]
        public static class GeoTunerSideScreen_SetRow
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


                if (!Option.Broker_Vanilla)
                {

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
        }

        [HarmonyPatch(typeof(GeoTunerConfig), "ConfigureBuildingTemplate")]

        private static class GeoTunerConfig_ConfigureBuildingTemplate
        {
            public static void Postfix(GameObject go)
            {
                go.AddOrGet<GeoTunerAdjustable>();
            }
        }


    

        [HarmonyPatch(typeof(GeoTuner.Instance), "RefreshModification")]

        private static class GeoTunerInstance_RefreshModification_Pre
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

        private static class GeoTunerInstance_RefreshModification_Post
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

        private static class Geyser_GetDescriptors
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

                Func<float, float> func = delegate (float emissionPerCycleModifier)
                {
                    float num8 = 600f / __instance.configuration.GetIterationLength();
                    return emissionPerCycleModifier / num8 / __instance.configuration.GetOnDuration();
                };

                string text = string.Format(STRINGS.UI.BUILDINGEFFECTS.TOOLTIPS.GEYSER_PRODUCTION_GEOTUNED, ElementLoader.FindElementByHash(__instance.configuration.GetElement()).name, 
                    GameUtil.GetFormattedMass(__instance.configuration.GetEmitRate(), GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"), 
                    GameUtil.GetFormattedTemperature(__instance.configuration.GetTemperature(), GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false));
                float num2;
                float num3;
                string str;
                string str2;
                string text2;
                string text3 = "";
                float count = 0;
                float count1 = 0;
                foreach (GeoTuner.Instance instance in GeoTuners)
                {
                    num2 = (Geyser.temperatureModificationMethod == Geyser.ModificationMethod.Percentages) ? (instance.currentGeyserModification.temperatureModifier * __instance.configuration.geyserType.temperature) : instance.currentGeyserModification.temperatureModifier;
                    num3 = func((Geyser.massModificationMethod == Geyser.ModificationMethod.Percentages) ? (instance.currentGeyserModification.massPerCycleModifier * __instance.configuration.scaledRate) : instance.currentGeyserModification.massPerCycleModifier);
                    str = ((num2 > 0f) ? "+" : "") + GameUtil.GetFormattedTemperature(num2, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Relative, true, false);
                    str2 = ((num3 > 0f) ? "+" : "") + GameUtil.GetFormattedMass(num3, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.##}");
                    count1 = instance.GetComponent<GeoTunerAdjustable>().UserMaxCapacity;
                    text2 = "\n    • " + count1+ STRINGS.UI.UISIDESCREENS.GEOTUNERSIDESCREEN.STUDIED_TOOLTIP_GEOTUNER_MODIFIER_ROW_TITLE.ToString();
                    text2 = text2 + str2 + " " + str;
                    count += count1;
                    text3 += text2;
                }
                text += "\n" + string.Format(UI.UISIDESCREENS.GEOTUNERADJUSTABLE.TOOLTIP, count, num);
                text += "\n" + text3;

                string arg = ElementLoader.FindElementByHash(__instance.configuration.GetElement()).tag.ProperName();

                __result[0] = new Descriptor(string.Format(STRINGS.UI.BUILDINGEFFECTS.GEYSER_PRODUCTION, arg, GameUtil.GetFormattedMass(__instance.configuration.GetEmitRate(),
                    GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"), GameUtil.GetFormattedTemperature(__instance.configuration.GetTemperature(),
                    GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)), text, Descriptor.DescriptorType.Effect, false);
            }
        }



        public static class Patch_EnergyConsumer_WattsNeededWhenActive
        {

            public static void Patch(Harmony harmony)
            {   
                    MethodInfo method = typeof(EnergyConsumer).GetProperty("WattsNeededWhenActive", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).GetGetMethod();
                    MethodInfo method2 = typeof(Patch_EnergyConsumer_WattsNeededWhenActive).GetMethod("Prefix");
                    harmony.Patch(method, new HarmonyMethod(method2), null, null, null);
            }
            public static bool Prefix(EnergyConsumer __instance, Building ___building, ref float __result)
            {

                if (___building.Def.PrefabID != "GeoTuner")
                {
                    return true;
                }
                GeoTunerAdjustable component = __instance.GetComponent<GeoTunerAdjustable>();
                if (component == null)
                {
                    return true;
                }
                __result = ___building.Def.EnergyConsumptionWhenActive * component.UserMaxCapacity * Option.Geotuners_Ratio;
                __instance.BaseWattageRating = __result;
                return false;
            }
        }
    }
}
