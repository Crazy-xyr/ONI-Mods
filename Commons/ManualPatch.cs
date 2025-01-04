using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crazyxyr.Commons
{
    public static  class ManualPatch
    {
        public static   Harmony harmony;
        public static Type[] types;
        public static void ManualPatch_init(Harmony harmony, Type[] types)
        {


            ManualPatch.harmony = harmony;
            ManualPatch.types = types;


        }

        public static void ManualPatch_NS(string spacename) {


            foreach (Type type in types.Where(n => n.Namespace == spacename))
            {
                harmony.CreateClassProcessor(type).Patch();

            }

        
        }

    }
}
