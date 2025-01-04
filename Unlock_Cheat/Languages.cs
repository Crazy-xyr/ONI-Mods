using System.Collections.Generic;
using System.IO;
namespace Unlock_Cheat
{
    public static class Languages
    {
        public class UI
        {
            public class USERMENUACTIONS
            {
                public class MUTATOR
                {
                    public static LocString NAME = "变异";

                    public static LocString TOOLTIP = "将种子或植物随机变异.";
                }

                public class IDENTIFY_MUTATION
                {
                    public static LocString NAME = "分析";

                    public static LocString TOOLTIP = "分析种子不需要" + STRINGS.UI.FormatAsLink("植物分析仪", "GENETICANALYSISSTATION");
                }
                public class HARVEST_WHEN_READY
                {
                    public static LocString NAME = "启用复制人收获";

                    public static LocString TOOLTIP = "当这株植物成熟时，复制人会收获,取消自动收获";

                    public static LocString PLANT_DO_NOT_SELFHARVEST = "禁用自动掉落(需要保存加载生效)";

                    public static LocString Reload = "需要保存加载生效";

                }
                public class CANCEL_HARVEST_WHEN_READY
                {
                    public static LocString NAME = "禁用复制人收获";

                    public static LocString TOOLTIP = "复制人不自动收获这株植物,改为成熟后自动掉落";

                    public static LocString PLANT_SELFHARVEST = "启用自动掉落";

                }
                public class SELFHARVEST
                {
                    public static LocString NAME = "启用植物自动收获";

                    public static LocString MutationNAME = "植物自动收获";


                    public static LocString TOOLTIP = "植物成熟时自动掉落";

                    public static LocString CANCEL_NAME = "禁用植物自动收获";

                    public static LocString CANCEL_TOOLTIP = "植物成熟时不会自动掉落(切换状态后需要保存加载一次)";


                }
            }
            public class USERTEXT
            {

                public static LocString NO_OWNED = "这个皮肤你暂未拥有,只有使用权";
                public static LocString LAST_OWNED = "这个皮肤数量是你拥有的最后的一件，分解后就没有了";
            }

         }

        internal static bool TryLoadTranslations(out Dictionary<string, string> translations)
        {
            string path = Unlock_Cheat.UnlockCheat.path;
            string path2 = "Translations";
            Localization.Locale locale = Localization.GetLocale();
            string path3 = Path.Combine(path, path2, ((locale != null) ? locale.Code : "zh") + ".po");
            if (File.Exists(path3))
            {
                translations =Localization.LoadStringsFile(path3, false);
                return true;
            }
            translations = null;
            return false;
        }

    }

}