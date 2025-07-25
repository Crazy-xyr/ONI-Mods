using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unlock_Cheat.CircuitOverloaded
{
    internal class CircuitOverloaded
    {
        [HarmonyPatch(typeof(CircuitManager))]
        [HarmonyPatch("CheckCircuitOverloaded")]  //电路过载检查
        public class CircuitManager_FCheckCircuitOverloaded_Patch
        {
            public static bool Prefix(CircuitManager __instance)
            {
                return false;
            }
        }
    }
}
