using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unlock_Cheat.MopTool_Patch
{
    [HarmonyPatch(typeof(MopTool), "OnPrefabInit")]
    public class MopTool_Patch
    {
        public static void Postfix()
        {
            MopTool.maxMopAmt = float.PositiveInfinity;
        }
    }
}
