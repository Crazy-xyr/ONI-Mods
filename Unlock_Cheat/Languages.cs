using System.Collections.Generic;
using System.IO;
namespace Unlock_Cheat
{
    public static class Languages
    {
        public class UI
        {
            // Token: 0x02000011 RID: 17
            public class USERMENUACTIONS
            {
                // Token: 0x02000012 RID: 18
                public class MUTATOR
                {
                    // Token: 0x0400005B RID: 91
                    public static LocString NAME = "变异";

                    // Token: 0x0400005C RID: 92
                    public static LocString TOOLTIP = "将种子或植物随机变异.";
                }

                // Token: 0x02000013 RID: 19
                public class IDENTIFY_MUTATION
                {
                    // Token: 0x0400005D RID: 93
                    public static LocString NAME = "分析";

                    // Token: 0x0400005E RID: 94
                    public static LocString TOOLTIP = "分析种子不需要" + STRINGS.UI.FormatAsLink("植物分析仪", "GENETICANALYSISSTATION");
                }

                public class SELFHARVEST
                {
                    // Token: 0x0400005D RID: 93
                    public static LocString NAME = "自动收获";

                    // Token: 0x0400005E RID: 94
                    public static LocString TOOLTIP = "植物成熟时自动掉落" ;
                }
            }
        }

        internal static bool TryLoadTranslations(out Dictionary<string, string> translations)
        {
            string path = Unlock_Cheat.UnlockCheat.path;
            string path2 = "Translations";
            Localization.Locale locale = Localization.GetLocale();
            string path3 = Path.Combine(path, path2, ((locale != null) ? locale.Code : null) + ".po");
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