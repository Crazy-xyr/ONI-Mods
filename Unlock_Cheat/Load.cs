using HarmonyLib;
using KMod;
using PeterHan.PLib.Core;
using PeterHan.PLib.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using crazyxyr.Commons;

namespace Unlock_Cheat
{
    public static class Unlock_Cheat
    {

        public static UserMod2 UnlockCheat { get; set; }
        public class Load : UserMod2
        {
            // Token: 0x0600002A RID: 42 RVA: 0x0000239F File Offset: 0x0000059F

            Dictionary<string, string> translations;
            private const string SuppressNotifications = "1832319118";

            public override void OnLoad(Harmony harmony)
            {

                UnlockCheat = this;
                ManualPatch.ManualPatch_init(harmony, this.assembly.GetTypes());
                PUtil.InitLibrary(false);
                new POptions().RegisterOptions(this, typeof(Options));
#if DEBUG
                ModUtil.RegisterForTranslation(typeof(Languages));
#else
            Localization.RegisterForTranslation(typeof(Languages));
#endif

                if (Languages.TryLoadTranslations(out translations))
                {

                    Localization.OverloadStrings(translations);

                    Debug.Log("[Unlock_Cheat] 翻译加载成功 "+ translations.Count);

                }
                else {

                    Debug.Log("[Unlock_Cheat] 翻译加载失败");

                }
                Options option = SingletonOptions<Options>.Instance;
                if (option.Achievement) ManualPatch.ManualPatch_NS("Unlock_Cheat.AchievementUnlock");
                if (option.Skin) ManualPatch.ManualPatch_NS("Unlock_Cheat.ItemSkinUnlock");
                if (option.Conduit) ManualPatch.ManualPatch_NS("Unlock_Cheat.Conduit_mod");
                if (option.MutantPlant) ManualPatch.ManualPatch_NS("Unlock_Cheat.MutantPlants");
                if (DlcManager.IsExpansion1Active()) ManualPatch.ManualPatch_NS("Unlock_Cheat.Harvest");

                if ( option.MutantPlant_SelfHarvest ) ManualPatch.ManualPatch_NS("Unlock_Cheat.MutantPlants.SelfHarvestPatch");

                if (DlcManager.IsAllContentSubscribed(new string[] {"EXPANSION1_ID" ,"DLC4_ID"}  )) ManualPatch.ManualPatch_NS("Unlock_Cheat.MissileLongRange");
            }

            public override void OnAllModsLoaded(Harmony harmony, IReadOnlyList<Mod> mods)
            {

                if (SingletonOptions<Options>.Instance.MutantPlant && !ModUtils.HasModbydlc(mods, new List<string>() { SuppressNotifications }))
                {
                    ManualPatch.ManualPatch_NS("Unlock_Cheat.MutantPlants.CopySettingPatch");
                    Debug.Log("[Unlock_Cheat] 植物复制按钮启用");
                }
                else
                {
                    Debug.LogFormat("[Unlock_Cheat] 植物复制按钮被mod_steamid: {0} 启用", SuppressNotifications);
                }
                foreach (MethodBase method in harmony.GetPatchedMethods())
                {
                    Debug.LogFormat("[Unlock_Cheat] 修补了：{0}.{1}", method.DeclaringType.FullName, method.Name);
                }

            }

        }

    }
}
