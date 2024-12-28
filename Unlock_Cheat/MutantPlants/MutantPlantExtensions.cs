using System.Collections.Generic;
using UnityEngine;
using Klei.AI;
using System.Linq;
using HarmonyLib;
using EventSystem2Syntax;
using PeterHan.PLib.Options;

namespace Unlock_Cheat.MutantPlants
{
    internal static class MutantPlantExtensions
    {
        public static void DiscoverSilentlyAndIdentifySubSpecies(PlantSubSpeciesCatalog.SubSpeciesInfo speciesInfo)
        {
            List<PlantSubSpeciesCatalog.SubSpeciesInfo> allSubSpeciesForSpecies = PlantSubSpeciesCatalog.Instance.GetAllSubSpeciesForSpecies(speciesInfo.speciesID);
            if (allSubSpeciesForSpecies != null && !allSubSpeciesForSpecies.Contains(speciesInfo))
            {
                allSubSpeciesForSpecies.Add(speciesInfo);
                if (speciesInfo.mutationIDs.Contains("SelfHarvest")) {

                    HashSet<Tag> identifiedSubSpecies = Traverse.Create(PlantSubSpeciesCatalog.Instance).Field("identifiedSubSpecies").GetValue<HashSet<Tag>>();

                    // Debug.Log("[测试] 前  "+ PlantSubSpeciesCatalog.Instance.identifiedSubSpecies.Count+"  "+ speciesInfo.ID);
                    identifiedSubSpecies.Add(speciesInfo.ID);

                    //Debug.Log("[测试] 后  " + PlantSubSpeciesCatalog.Instance.identifiedSubSpecies.Count + "  " + PlantSubSpeciesCatalog.Instance.IsSubSpeciesIdentified(speciesInfo.ID));

                    foreach (MutantPlant mutantPlant in Components.MutantPlants)
                    {
                        if (mutantPlant.HasTag(speciesInfo.ID))
                        {
                            mutantPlant.UpdateNameAndTags();
                        }
                    }
                    return;
                }
                PlantSubSpeciesCatalog.Instance.IdentifySubSpecies(speciesInfo.ID);
                SaveGame.Instance.ColonyAchievementTracker.LogAnalyzedSeed(speciesInfo.speciesID);
            }
        }

        internal static void Mutator(this MutantPlant mutant)
        {

            List<string> strings = new List<string> { };
            if (mutant != null)
            {

                //mutant.Mutate();
                strings.Add(Db.Get().PlantMutations.GetRandomMutation(mutant.PrefabID().Name).Id);
                if (mutant.MutationIDs != null)
                {

                    if (mutant.MutationIDs.Contains("SelfHarvest"))
                    {
                        strings.Add("SelfHarvest");

                    }
                    mutant.delattr();
                }   
                mutant.SetSubSpecies(strings);

                ApplyMutator(mutant);
            }
        }

        internal static void Mutator(this MutantPlant mutant, List<string> mutationIDs)
        {
            if (mutant != null)
            {
                if (mutant.MutationIDs != null)
                {
                    mutant.delattr();

                }          
                //if (mutationIDs != null && mutationIDs.Count == 1 && mutationIDs.Contains("SelfHarvest")) {

                //    mutant.SelfHarvest(false);
                //    return;
                //}
                mutant.SetSubSpecies(mutationIDs);
                ApplyMutator(mutant);
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
                    mutant.Analyze();
                    mutant.Mutator(strings);

                    // MutantPlantExtensions.DiscoverSilentlyAndIdentifySubSpecies(mutant.GetSubSpeciesInfo());
                    // Attributes attributes = mutant.GetAttributes();
                    //attributes.Remove(new AttributeModifier(Db.Get().Amounts.OldAge.maxAttribute.Id, -0.999999f, Strings.Get(new StringKey("STRINGS.CREATURES.PLANT_MUTATIONS." + "heavyFruit".ToUpper() + ".NAME")), true, false, true));

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
                   
                    strings =  mutant.MutationIDs ?? new List<string> { };
                   if (strings.Contains("SelfHarvest"))
                    {
                       return;
                   }

                    strings.Add("SelfHarvest");
                    mutant.Analyze();
                    mutant.Mutator(strings);
                    //mutant.ApplyMutations();
                   // Attributes attributes = mutant.GetAttributes();
                   // attributes.Add(new AttributeModifier(Db.Get().Amounts.OldAge.maxAttribute.Id, -0.999999f, Strings.Get(new StringKey("STRINGS.CREATURES.PLANT_MUTATIONS." + "heavyFruit".ToUpper() + ".NAME")), true, false, true));
                   // MutantPlantExtensions.DiscoverSilentlyAndIdentifySubSpecies(mutant.GetSubSpeciesInfo());
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
        internal static void ApplyMutator( MutantPlant mutant)
        {
            if (mutant != null)
            {
                          
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
