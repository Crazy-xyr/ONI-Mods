using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PeterHan.PLib.Options;
using UnityEngine;
using HarmonyLib;

namespace Unlock_Cheat.MissileLongRange
{
    internal class MissileLongRange
    {

        [HarmonyPatch(typeof(LargeImpactorStatus), "DealDamage")]
        public class LargeImpactorStatus_DealDamage
        {
            public static bool Prefix(LargeImpactorStatus __instance,ref int damage)
            {
                damage = Unlock_Cheat.Options.MissileLongRange_damage; ;
                return true;
            }

        }
    }
}
