using System.Reflection;
using HarmonyLib;
using KMod;
using crazyxyr.Commons;
using System.Collections.Generic;

namespace crazyxyr.SelectLastCarePackage
{
    public class ModLoader : UserMod2
    {
        public override void OnLoad(Harmony harmony)
        {

            ManualPatch.ManualPatch_init(harmony, this.assembly.GetTypes());
            ManualPatch.ManualPatch_NS("crazyxyr.SelectLastCarePackage.Patches");
#if DEBUG
            ModUtil.RegisterForTranslation(typeof(Languages));
#else
            Localization.RegisterForTranslation(typeof(Languages));
#endif

        }

        public override void OnAllModsLoaded(Harmony harmony, IReadOnlyList<Mod> mods)
        {
            if ( !ModUtils.HasModbydlc(mods, new List<string>() {"2856555858"}))
            {
                ManualPatch.ManualPatch_NS("crazyxyr.SelectLastCarePackage.Patches2");
                Debug.Log("[���Ĳ�����-Fix] ˢ�°�ť����");
            }

            else
            {
                Debug.LogFormat("[���Ĳ�����-Fix] ˢ�°�ť��ť��mod_workshop_id: {0} ����", "2856555858");
            }
            foreach (MethodBase method in harmony.GetPatchedMethods())
            {
                Debug.LogFormat("[���Ĳ�����-Fix] �޲��ˣ�{0}.{1}", method.DeclaringType.FullName, method.Name);
            }
        }
    }
}