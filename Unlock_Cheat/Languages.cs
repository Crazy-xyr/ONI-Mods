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
                    public static LocString NAME = "����";

                    public static LocString TOOLTIP = "�����ӻ�ֲ���������.";
                }

                public class IDENTIFY_MUTATION
                {
                    public static LocString NAME = "����";

                    public static LocString TOOLTIP = "�������Ӳ���Ҫ" + STRINGS.UI.FormatAsLink("ֲ�������", "GENETICANALYSISSTATION");
                }
                public class HARVEST_WHEN_READY
                {
                    public static LocString NAME = "���ø������ջ�";

                    public static LocString TOOLTIP = "������ֲ�����ʱ�������˻��ջ�,ȡ���Զ��ջ�";

                    public static LocString PLANT_DO_NOT_SELFHARVEST = "�����Զ�����(��Ҫ���������Ч)";

                    public static LocString Reload = "��Ҫ���������Ч";

                }
                public class CANCEL_HARVEST_WHEN_READY
                {
                    public static LocString NAME = "���ø������ջ�";

                    public static LocString TOOLTIP = "�����˲��Զ��ջ�����ֲ��,��Ϊ������Զ�����";

                    public static LocString PLANT_SELFHARVEST = "�����Զ�����";

                }
                public class SELFHARVEST
                {
                    public static LocString NAME = "����ֲ���Զ��ջ�";

                    public static LocString MutationNAME = "ֲ���Զ��ջ�";


                    public static LocString TOOLTIP = "ֲ�����ʱ�Զ�����";

                    public static LocString CANCEL_NAME = "����ֲ���Զ��ջ�";

                    public static LocString CANCEL_TOOLTIP = "ֲ�����ʱ�����Զ�����(�л�״̬����Ҫ�������һ��)";


                }
            }
            public class USERTEXT
            {

                public static LocString NO_OWNED = "���Ƥ������δӵ��,ֻ��ʹ��Ȩ";
                public static LocString LAST_OWNED = "���Ƥ����������ӵ�е�����һ�����ֽ���û����";
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