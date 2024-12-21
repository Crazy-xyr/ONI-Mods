using HarmonyLib;
using KMod;
using PeterHan.PLib.Core;
using PeterHan.PLib.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Unlock_Cheat
{
    public static class Unlock_Cheat
    {

        public static UserMod2 UnlockCheat { get; set; }
        public class Load : UserMod2
        {
            // Token: 0x0600002A RID: 42 RVA: 0x0000239F File Offset: 0x0000059F

            Type[] types;
            Dictionary<string, string> translations;

            public override void OnLoad(Harmony harmony)
            {

                UnlockCheat = this;
                types = this.assembly.GetTypes();
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

                if (SingletonOptions<Options>.Instance.Achievement)
                {
                    foreach (Type type in types.Where(n => n.Namespace == "Unlock_Cheat.AchievementUnlock"))
                    {
                        harmony.CreateClassProcessor(type).Patch();

                    }

                }

                if (SingletonOptions<Options>.Instance.Skin)
                {
                    foreach (Type type in types.Where(n => n.Namespace == "Unlock_Cheat.ItemSkinUnlock"))
                    {
                        harmony.CreateClassProcessor(type).Patch();

                    }

                }
                if (SingletonOptions<Options>.Instance.Conduit)
                {

                    foreach (Type type in types.Where(n => n.Namespace == "Unlock_Cheat.Conduit_mod"))
                    {
                        harmony.CreateClassProcessor(type).Patch();

                    }

                }

                if (SingletonOptions<Options>.Instance.MutantPlant)
                {

                    foreach (Type type in types.Where(n => n.Namespace == "Unlock_Cheat.MutantPlants"))
                    {
                        harmony.CreateClassProcessor(type).Patch();

                    }

                }

                if (SingletonOptions<Options>.Instance.MutantPlant_SelfHarvest && ! SingletonOptions<Options>.Instance.MutantPlant_SelfHarvest_Independent )
                {

                    foreach (Type type in types.Where(n => n.Namespace == "Unlock_Cheat.MutantPlants.SelfHarvest"))
                    {
                        harmony.CreateClassProcessor(type).Patch();

                    }

                }

                // base.OnLoad(harmony);

            }

            private const string SuppressNotifications = "1832319118";

            List<string> banmod = new List<string>() { SuppressNotifications };
            public override void OnAllModsLoaded(Harmony harmony, IReadOnlyList<Mod> mods)
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

                if (SingletonOptions<Options>.Instance.MutantPlant_SelfHarvest && !flag)
                {

                    foreach (Type type in types.Where(n => n.Namespace == "Unlock_Cheat.MutantPlantsCopySetting"))
                    {
                        harmony.CreateClassProcessor(type).Patch();

                    }
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
