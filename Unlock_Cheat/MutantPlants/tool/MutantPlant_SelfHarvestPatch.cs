using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unlock_Cheat.MutantPlants.SelfHarvest
{
    [HarmonyPatch(typeof(HarvestDesignatable), "SetHarvestWhenReady")]
    public static class HarvestDesignatable_SetHarvestWhenReady
    {
        public static void Prefix(HarvestDesignatable __instance, bool state)
        {

            MutantPlant mutant = __instance.GetComponent<MutantPlant>();

            if (mutant != null)
            {
                // false  禁用时 要附加自动收获，已有状态的不用管
                //true  启用时,取消自动收获，没有状态的不管
                mutant.SelfHarvest(state);
            }



        }



    }



}
