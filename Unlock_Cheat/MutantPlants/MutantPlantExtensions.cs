using System.Collections.Generic;
using UnityEngine;
using Klei.AI;
using System.Linq;
using HarmonyLib;
using EventSystem2Syntax;
using PeterHan.PLib.Options;
using System.Collections;
using System;
using static PlantSubSpeciesCatalog;

namespace Unlock_Cheat.MutantPlants
{
    internal static class MutantPlantExtensions
    {
        public static void DiscoverSilentlyAndIdentifySubSpecies(PlantSubSpeciesCatalog.SubSpeciesInfo speciesInfo)
        {
            List<PlantSubSpeciesCatalog.SubSpeciesInfo> allSubSpeciesForSpecies = PlantSubSpeciesCatalog.Instance.GetAllSubSpeciesForSpecies(speciesInfo.speciesID);



            foreach (KeyValuePair<Tag, List<SubSpeciesInfo>> kvp in PlantSubSpeciesCatalog.Instance.discoveredSubspeciesBySpecies)
            {


                int count = kvp.Value.RemoveAll(e => e.mutationIDs.Contains("SelfHarvest"));
                Debug.LogFormat ("[测试] {0} 删除了：{1}" ,kvp.Key.Name, count);
            }

          int count1=  PlantSubSpeciesCatalog.Instance.identifiedSubSpecies.RemoveWhere(e => e.Name.Contains("SelfHarvest"));
            Debug.LogFormat("[测试] {0} 删除了：{1}", "identifiedSubSpecies", count1);


            if (allSubSpeciesForSpecies != null && !allSubSpeciesForSpecies.Contains(speciesInfo))
            {
                allSubSpeciesForSpecies.Add(speciesInfo);
                //if (speciesInfo.mutationIDs.Contains("SelfHarvest")) {

                //    HashSet<Tag> identifiedSubSpecies = Traverse.Create(PlantSubSpeciesCatalog.Instance).Field("identifiedSubSpecies").GetValue<HashSet<Tag>>();

                //    // Debug.Log("[测试] 前  "+ PlantSubSpeciesCatalog.Instance.identifiedSubSpecies.Count+"  "+ speciesInfo.ID);
                //    identifiedSubSpecies.Add(speciesInfo.ID);

                //    //Debug.Log("[测试] 后  " + PlantSubSpeciesCatalog.Instance.identifiedSubSpecies.Count + "  " + PlantSubSpeciesCatalog.Instance.IsSubSpeciesIdentified(speciesInfo.ID));

                //    foreach (MutantPlant mutantPlant in Components.MutantPlants)
                //    {
                //        if (mutantPlant.HasTag(speciesInfo.ID))
                //        {
                //            mutantPlant.UpdateNameAndTags();
                //        }
                //    }
                //    return;
                //}

            
                PlantSubSpeciesCatalog.Instance.IdentifySubSpecies(speciesInfo.ID);
                SaveGame.Instance.ColonyAchievementTracker.LogAnalyzedSeed(speciesInfo.speciesID);
            }
        }

        internal static void Mutator(this MutantPlant mutant)
        {

           // List<string> strings = new List<string> { };
            if (mutant != null)
            {

                //strings.Add(Db.Get().PlantMutations.GetRandomMutation(mutant.PrefabID().Name).Id);
                //if (mutant.MutationIDs != null)
                //{
                //    mutant.delattr();
                //    //if (mutant.MutationIDs.Contains("SelfHarvest"))
                //    //{
                //    //    strings.Add("SelfHarvest");

                //    //}


                //}
                mutant.Mutate();

                mutant.ApplyMutator();

            }
        }


        internal static void SelfHarvest(this MutantPlant mutant)
        {

            bool flag = (mutant.MutationIDs != null && mutant.MutationIDs.Contains("SelfHarvest"));

                mutant.SelfHarvest(flag);
            }
        internal static void SelfHarvest(this MutantPlant mutant, bool allowedHarvest = false)
        {

            if (mutant != null)
            {

                List<string> strings = null;

                if (allowedHarvest && mutant.MutationIDs != null && mutant.MutationIDs.Contains("SelfHarvest"))
                {


                    strings = mutant.MutationIDs;
                    strings.Remove("SelfHarvest");
                    mutant.SetSubSpecies(new List<string> { });
                    mutant.ApplyMutator(false);
                    mutant.Analyze();
                    //Attributes attributes = mutant.GetAttributes();
                    //attributes.Remove(new AttributeModifier(Db.Get().Amounts.OldAge.maxAttribute.Id, -0.999999f, Strings.Get(new StringKey("STRINGS.CREATURES.PLANT_MUTATIONS." + "heavyFruit".ToUpper() + ".NAME")), true, false, true));
                    //MutantPlantExtensions.DiscoverSilentlyAndIdentifySubSpecies(mutant.GetSubSpeciesInfo());
                     

                    if (SingletonOptions<Options>.Instance.MutantPlant_SelfHarvest_Independent)
                    {

                        KMonoBehaviour kMonoBehaviour = mutant;
                        if (kMonoBehaviour != null)
                        {
                            PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Negative, Languages.UI.USERMENUACTIONS.HARVEST_WHEN_READY.PLANT_DO_NOT_SELFHARVEST, kMonoBehaviour.transform, 1.5f, false);

                        }

                    }

                   
                }

                else if (!allowedHarvest)
                {
                   
                    strings = new List<string> { };
                   if (mutant.MutationIDs!=null && mutant.MutationIDs.Contains("SelfHarvest"))
                    {
                       return;
                   }
                   //else if (mutant.MutationIDs!=null )
                   // {
                   //     mutant.delattr();

                   // }

                    strings.Add("SelfHarvest");
                    mutant.SetSubSpecies(strings);
                    mutant.ApplyMutator(false);
                    mutant.Analyze();

                    //mutant.ApplyMutations();
                    // Attributes attributes = mutant.GetAttributes();
                    //attributes.Add(new AttributeModifier(Db.Get().Amounts.OldAge.maxAttribute.Id, -0.999999f, Strings.Get(new StringKey("STRINGS.CREATURES.PLANT_MUTATIONS." + "heavyFruit".ToUpper() + ".NAME")), true, false, true));
                    //MutantPlantExtensions.DiscoverSilentlyAndIdentifySubSpecies(mutant.GetSubSpeciesInfo());
                    if (SingletonOptions<Options>.Instance.MutantPlant_SelfHarvest_Independent)
                    {
                        KMonoBehaviour kMonoBehaviour = mutant;
                        if (kMonoBehaviour != null)
                        {
                            PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Plus, Languages.UI.USERMENUACTIONS.CANCEL_HARVEST_WHEN_READY.PLANT_SELFHARVEST, kMonoBehaviour.transform, 1.5f, false);
                        } 
                    }                            
                }

            }
          
        }
        internal static void ApplyMutator(this MutantPlant mutant,bool pop = true)
        {
            if (mutant != null)
            {



                if (mutant.MutationIDs != null && mutant.MutationIDs.Count > 0)
                {
                    KMonoBehaviour kMonoBehaviour = mutant;

                    if (kMonoBehaviour != null && pop)
                    {
                        PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Resource, Languages.UI.USERMENUACTIONS.HARVEST_WHEN_READY.Reload, kMonoBehaviour.transform, 3f, false);

                    }
                // mutant.delattr();


                }
               // mutant.SetSubSpecies(mutationIDs);
                mutant.ApplyMutations();
                mutant.AddTag(GameTags.MutatedSeed);
                if (mutant.HasTag(GameTags.Plant))
                {
                    MutantPlantExtensions.DiscoverSilentlyAndIdentifySubSpecies(mutant.GetSubSpeciesInfo());
                }
                else
                {
                    PlantSubSpeciesCatalog.Instance.DiscoverSubSpecies(mutant.GetSubSpeciesInfo(), mutant);
                }

                PlantBranchGrower.Instance smi = mutant.GetSMI<PlantBranchGrower.Instance>();
                if (!smi.IsNullOrStopped())
                {
                    smi.ActionPerBranch(delegate (GameObject go)
                    {
                        MutantPlant mutantPlant;
                        if (go.TryGetComponent<MutantPlant>(out mutantPlant))
                        {
                            mutant.CopyMutationsTo(mutantPlant);
                            mutantPlant.ApplyMutations();
                            MutantPlantExtensions.DiscoverSilentlyAndIdentifySubSpecies(mutantPlant.GetSubSpeciesInfo());
                        }
                    });
                }
                DetailsScreen.Instance.Trigger(-1514841199, null);

            }
        }

        internal static void IdentifyMutation(this MutantPlant mutant)
        {
            if (mutant != null)
            {
                mutant.Analyze();

                PlantSubSpeciesCatalog.Instance.IdentifySubSpecies(mutant.SubSpeciesID);
                SaveGame.Instance.ColonyAchievementTracker.LogAnalyzedSeed(mutant.SpeciesID);

                DetailsScreen.Instance.Trigger(-1514841199, null);
            }
        }
        public static void delattr(this MutantPlant mutant)
        {
            if (mutant.IsOriginal)
            {
                return;
            }
            KBatchedAnimController component = mutant.GetComponent<KBatchedAnimController>();
            component.TintColour = Color.white;
            foreach (string mutationID in mutant.MutationIDs)
            {
                PlantMutation mutation = Db.Get().PlantMutations.Get(mutationID);
                Attributes attributes = mutant.GetAttributes();

                mutation.RemoveFrom(attributes);

                if (mutation.symbolOverrideInfo != null && mutation.symbolOverrideInfo.Count > 0)
                {
                    SymbolOverrideController component2 = mutant.GetComponent<SymbolOverrideController>();
                    if (component2 != null)
                    {
                        foreach (PlantMutation.SymbolOverrideInfo symbolOverrideInfo in mutation.symbolOverrideInfo)
                        {
                            KAnim.Build.Symbol symbol = Assets.GetAnim(symbolOverrideInfo.sourceAnim).GetData().build.GetSymbol(symbolOverrideInfo.sourceSymbol);
                            component2.RemoveSymbolOverride(symbolOverrideInfo.targetSymbolName, 0);
                        }
                    }
                }
                component.TintColour = Color.white;

                List<string> symbolTintTargets = Traverse.Create(mutation).Field("symbolTintTargets").GetValue<List<string>>();
                List<string> symbolScaleTargets = Traverse.Create(mutation).Field("symbolScaleTargets").GetValue<List<string>>();

                    for (int i = 0; i < mutant.transform.childCount; i++)
                    {

                        GameObject gameObject = mutant.transform.GetChild(i).gameObject;

                        if ( gameObject.name.EndsWith("_BGFX")|| gameObject.name.EndsWith("_FGFX"))
                        {
                            GameObject.Destroy(gameObject);
                        }

                }

                if (symbolTintTargets.Count > 0)
                {
                    for (int i = 0; i < symbolTintTargets.Count; i++)
                    {
                        component.SetSymbolTint(symbolTintTargets[i], Color.white);
                    }
                }
                if (symbolScaleTargets.Count > 0)
                {
                    for (int j = 0; j < symbolScaleTargets.Count; j++)
                    {
                        component.SetSymbolScale(symbolScaleTargets[j],1f);
                    }
                }

            }

            mutant.SetSubSpecies(null);

        }

    }

}
