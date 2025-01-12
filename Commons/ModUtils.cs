using System.Collections.Generic;
using HarmonyLib;
using KMod;
using static STRINGS.UI.FRONTEND;

namespace crazyxyr.Commons
{
    public static class ModUtils
    {
        private static readonly List<Label> Mods = Global.Instance.modManager.mods.FindAll(mod => mod.IsEnabledForActiveDlc())
            .ConvertAll(mod => mod.label);

        private const string RefreshModId = "1724518038";
        private const string NewRefreshModId = "2317581286";

        public static bool HasRefreshMod()
        {
            return HasMod(RefreshModId) || HasMod(NewRefreshModId);
        }


        public static bool HasMod(string id)
        {
            return Mods.Exists(label => id == label.id);
        }
        public static bool HasModbydlc(IReadOnlyList<Mod> mods, List<string> banmod )
        {


            bool flag = false;

            foreach (Mod mod in mods)
            {

                if (mod.IsEnabledForActiveDlc() && banmod.Contains(mod.label.id))
                {

                    flag = true;
                    break;
                }

            }
            return flag;


        }

    }

}
