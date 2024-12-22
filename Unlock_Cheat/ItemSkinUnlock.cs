using Database;
using HarmonyLib;
using Klei.CustomSettings;
using KMod;
using Newtonsoft.Json;
using System.Text;
using UnityEngine;
using STRINGS;
using static Operational;
using static KleiInventoryScreen;
using System;

namespace Unlock_Cheat.ItemSkinUnlock
{


    public class ItemSkin_Unlock
    {
        [HarmonyPatch(typeof(PermitItems))]
        [HarmonyPatch("GetOwnedCount")]
        public class PermitItems_GetOwnedCount
        {
            // Token: 0x06000041 RID: 65 RVA: 0x00002744 File Offset: 0x00000944
            public static void Postfix(PermitResource permit,ref int __result)
            {   

                __result += 1;
            }
        }


        [HarmonyPatch(typeof(PermitResource))]
        [HarmonyPatch("IsOwnableOnServer")]
        public class PermitResource_IsOwnableOnServer
        {
            // Token: 0x06000041 RID: 65 RVA: 0x00002744 File Offset: 0x00000944
            public static bool Prefix( ref bool __result)
            {
                __result = true;

                return false;
            }
        }


        [HarmonyPatch(typeof(KleiInventoryScreen), "GetPermitPrintabilityState")]

        public class KleiInventoryScreen_GetPermitPrintabilityState
        {

            public static void Postfix(KleiInventoryScreen __instance,ref KleiInventoryScreen.PermitPrintabilityState __result, PermitResource permit)
            {

                if (__result == KleiInventoryScreen.PermitPrintabilityState.AlreadyOwned)
                {
                    int __count = PermitItems.GetOwnedCount(permit);
                    if (__count > 1)
                    {
                        return;
                    }
                    ulong num;
                    ulong num2;
                    PermitItems.TryGetBarterPrice(permit.Id, out num, out num2);

                    if (KleiItems.GetFilamentAmount() < num)
                    {
                        __result = KleiInventoryScreen.PermitPrintabilityState.TooExpensive;
                        return;
                    }
                }
            }
        }


       [HarmonyPatch(typeof(KleiInventoryScreen), "RefreshBarterPanel")]
        public class KleiInventoryScreen_RefreshBarterPanel
        {
            // Token: 0x06000041 RID: 65 RVA: 0x00002744 File Offset: 0x00000944
            public static void Postfix(KleiInventoryScreen __instance)
            {

                if (__instance.barterSellButton.isInteractable == false){
                     return;
                }

                PermitResource SelectedPermit = Traverse.Create(__instance).Property("SelectedPermit").GetValue<PermitResource>();
                if (SelectedPermit != null) {

                    bool flag = PermitItems.GetOwnedCount(SelectedPermit) > 1;

                    if (flag) { 
                        return; 
                    }
                    HierarchyReferences component2 = __instance.barterSellButton.GetComponent<HierarchyReferences>();
                    LocText reference2 = component2.GetReference<LocText>("CostLabel");                   
                    __instance.barterSellButton.isInteractable = flag;
                    __instance.barterSellButton.GetComponent<ToolTip>().SetSimpleTooltip(Languages.UI.USERTEXT.NO_OWNED);
                    reference2.SetText("");
                    reference2.color = Color.white;
                }


            }
        }
    }

 
}
