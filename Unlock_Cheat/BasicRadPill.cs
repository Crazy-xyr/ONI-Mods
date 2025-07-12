using HarmonyLib;
using Klei.AI;
using UnityEngine;

namespace Unlock_Cheat.BasicRadPill
{
    internal class BasicRadPill
    {

        [HarmonyPatch(typeof(MedicinalPillWorkable), "CanBeTakenBy")]
        public static class MedicinalPillWorkable_CanBeTakenBy
        {
            public static void Postfix(ref bool __result, ref MedicinalPill ___pill, GameObject consumer)
            {
                if (___pill.info.id == "BasicRadPill")
                {
                    AmountInstance amountInstance = consumer.GetAmounts().Get(Db.Get().Amounts.RadiationBalance.Id);
                    if (amountInstance != null && amountInstance.value < Unlock_Cheat.Options.BasicRadPill_MinRAD)
                    {
                        __result = false;
                    }
                }
            }
        }
    }
}
