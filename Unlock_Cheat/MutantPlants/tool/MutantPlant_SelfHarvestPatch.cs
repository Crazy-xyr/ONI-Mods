using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STRINGS;

namespace Unlock_Cheat.MutantPlants.SelfHarvestPatch
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

    [HarmonyPatch(typeof(HarvestDesignatable), "OnRefreshUserMenu")]
    public static class HarvestDesignatable_OnRefreshUserMenu
    {
        public static  bool Prefix(HarvestDesignatable __instance)
        {

            KMonoBehaviour kMonoBehaviour = __instance;

            if (kMonoBehaviour != null && __instance.showUserMenuButtons)
            {
                KIconButtonMenu.ButtonInfo button = __instance.harvestWhenReady ? new KIconButtonMenu.ButtonInfo("action_harvest", Languages.UI.USERMENUACTIONS.CANCEL_HARVEST_WHEN_READY.NAME, delegate ()
                {
                    __instance.OnClickCancelHarvestWhenReady();
                    PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Negative, Languages.UI.USERMENUACTIONS.CANCEL_HARVEST_WHEN_READY.PLANT_SELFHARVEST, kMonoBehaviour.transform, 1.5f, false);
                }, global::Action.NumActions, null, null, null, Languages.UI.USERMENUACTIONS.CANCEL_HARVEST_WHEN_READY.TOOLTIP, true) : new KIconButtonMenu.ButtonInfo("action_harvest", Languages.UI.USERMENUACTIONS.HARVEST_WHEN_READY.NAME, delegate ()
                {
                    __instance.OnClickHarvestWhenReady();
                    PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Plus, Languages.UI.USERMENUACTIONS.HARVEST_WHEN_READY.PLANT_DO_NOT_SELFHARVEST, kMonoBehaviour.transform, 1.5f, false);
                }, global::Action.NumActions, null, null, null, Languages.UI.USERMENUACTIONS.HARVEST_WHEN_READY.TOOLTIP, true);
                Game.Instance.userMenu.AddButton(kMonoBehaviour.gameObject, button, 1f);

                return false;

            }
            else
            {
                return true;

            }

        }
    }

}
