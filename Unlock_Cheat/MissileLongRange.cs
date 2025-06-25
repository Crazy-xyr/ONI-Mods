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
        private static Options option = SingletonOptions<Options>.Instance;

        //[HarmonyPatch(typeof(MissileLongRangeConfig.DamageEventPayload), MethodType.Constructor , new Type[] { typeof(int) } )]
        //public class MissileLongRange_damage
        //{
        //    public static bool Prefix(MissileLongRangeConfig.DamageEventPayload __instance)
        //    {
        //        __instance.damage = option.MissileLongRange_damage;
        //        return false;
        //    }
        //}


        [HarmonyPatch(typeof(LargeImpactorStatus), "DealDamage")]
        public class LargeImpactorStatus_DealDamage
        {
            public static bool Prefix(LargeImpactorStatus __instance,ref int damage)
            {
                damage = option.MissileLongRange_damage; ;
                return true;
            }

        }
    }
}
