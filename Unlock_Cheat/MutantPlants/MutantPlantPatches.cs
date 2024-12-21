using HarmonyLib;
using System;
using UnityEngine;
using Klei.AI;
using Database;
using PeterHan.PLib.Options;

namespace Unlock_Cheat.MutantPlants
{
    internal class MutantPlantPatches
    {

        public static global::Action MaxAction { get {

                global::Action action;
                if (!Enum.TryParse<global::Action>("NumActions", out action))
                {
                    Array values = Enum.GetValues(typeof(global::Action));
                    if (values.Length > 0)
                    {
                        action = (global::Action)values.GetValue(values.Length - 1);
                    }
                    else
                    {
                        action = global::Action.NumActions;
                    }
                }
                return  action;
                }

        }
        private static void OnRefreshUserMenu(MutantPlant mutant)
        {
            KPrefabID kprefabID;
            if (mutant != null && mutant.TryGetComponent<KPrefabID>(out kprefabID))
            {
                if ((mutant.IsOriginal && !kprefabID.HasTag(GameTags.PlantBranch)) || kprefabID.HasTag(GameTags.Seed) || kprefabID.HasTag(GameTags.CropSeed) || kprefabID.HasTag(GameTags.Plant))
                {
                    KIconButtonMenu.ButtonInfo button = new KIconButtonMenu.ButtonInfo("action_select_research", Languages.UI.USERMENUACTIONS.MUTATOR.NAME, new System.Action(mutant.Mutator), MaxAction, null, null, null, Languages.UI.USERMENUACTIONS.MUTATOR.TOOLTIP, true);
                    Game.Instance.userMenu.AddButton(mutant.gameObject, button, 1f);

                }

                if (SingletonOptions<Options>.Instance.MutantPlant_SelfHarvest_Independent && kprefabID.HasTag(GameTags.Plant))
                {

                    KIconButtonMenu.ButtonInfo button1 = new KIconButtonMenu.ButtonInfo("action_harvest", Languages.UI.USERMENUACTIONS.SELFHARVEST.NAME, new System.Action(mutant.SelfHarvest), MaxAction, null, null, null, Languages.UI.USERMENUACTIONS.SELFHARVEST.TOOLTIP, true);
                    Game.Instance.userMenu.AddButton(mutant.gameObject, button1, 1f);

                }

                if (!mutant.IsOriginal && !mutant.IsIdentified)
                {
                    KIconButtonMenu.ButtonInfo button2 = new KIconButtonMenu.ButtonInfo("action_select_research", Languages.UI.USERMENUACTIONS.IDENTIFY_MUTATION.NAME, new System.Action(mutant.IdentifyMutation), MaxAction, null, null, null, Languages.UI.USERMENUACTIONS.IDENTIFY_MUTATION.TOOLTIP, true);
                    Game.Instance.userMenu.AddButton(mutant.gameObject, button2, 1f);
                }
            }
        }

        private static void OnCopySettings(MutantPlant newdata,object data)
        {
            GameObject gameObject = (GameObject)data;
            bool flag = !(gameObject == null);
            if (flag)
            {
                MutantPlant component = gameObject.GetComponent<MutantPlant>();
                bool flag2 = !(component == null);
                if (flag2)
                {
                    newdata.Mutator(component.MutationIDs);


                }
            }
        }

        private static readonly EventSystem.IntraObjectHandler<MutantPlant> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<MutantPlant>(delegate (MutantPlant component, object data)
        {
            MutantPlantPatches.OnRefreshUserMenu(component);
        });

        private static readonly EventSystem.IntraObjectHandler<MutantPlant> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<MutantPlant>(delegate (MutantPlant component, object data)
        {
            MutantPlantPatches.OnCopySettings(component,data);
        });

        [HarmonyPatch(typeof(MutantPlant), "OnSpawn")]
        public static class MutantPlant_OnSpawn
        {
            public static void Postfix(MutantPlant __instance)
            {
                __instance.Subscribe<MutantPlant>(493375141, MutantPlantPatches.OnRefreshUserMenuDelegate);
                __instance.Subscribe<MutantPlant>(-905833192, MutantPlantPatches.OnCopySettingsDelegate);
            }
        }

        [HarmonyPatch(typeof(MutantPlant), "OnCleanUp")]
        public static class MutantPlant_OnCleanUp
        {
            public static void Prefix(MutantPlant __instance)
            {
                __instance.Unsubscribe<MutantPlant>(493375141, MutantPlantPatches.OnRefreshUserMenuDelegate, false);
                __instance.Unsubscribe<MutantPlant>(-905833192, MutantPlantPatches.OnCopySettingsDelegate, false);

            }
        }

        [HarmonyPatch(typeof(PlantMutation), "AttributeModifier")]
        public static class PlantMutation_AttributeModifier
        {
            public static void Prefix(PlantMutation __instance, Klei.AI.Attribute attribute, ref float amount)
            {
                if (attribute == Db.Get().PlantAttributes.MinRadiationThreshold || attribute == Db.Get().PlantAttributes.MinLightLux) {

                    amount = 0;
                }
            }
        }

        [HarmonyPatch(typeof(PlantMutation), "ApplyFunctionalTo")]
        public static class PlantMutation_ApplyFunctionalTo
        {
            public static void Postfix(PlantMutation __instance, MutantPlant target)
            {

                    SeedProducer component = target.GetComponent<SeedProducer>();

                    if (component != null && component.seedInfo.productionType == SeedProducer.ProductionType.Sterile)
                    {
                        component.Configure(component.seedInfo.seedId, SeedProducer.ProductionType.Harvest, 1);
                    }

            }
        }

        [HarmonyPatch(typeof(PlantMutations), MethodType.Constructor, new Type[] { typeof(ResourceSet) } )]
        public static class PlantMutation_PlantMutations
        {
            public static void Postfix(PlantMutations __instance)
            {

                PlantMutation plantMutation = new PlantMutation("SelfHarvest", Languages.UI.USERMENUACTIONS.SELFHARVEST.NAME, Languages.UI.USERMENUACTIONS.SELFHARVEST.TOOLTIP);
                plantMutation.ForceSelfHarvestOnGrown();
                plantMutation.originalMutation = true;
                __instance.Add(plantMutation);

            }
        } 

    }
}
