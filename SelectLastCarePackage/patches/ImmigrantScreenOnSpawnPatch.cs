using HarmonyLib;
using STRINGS;
using crazyxyr.Commons;
namespace crazyxyr.SelectLastCarePackage.Patches
{
    [HarmonyPatch(typeof(ImmigrantScreen), "OnSpawn")]
    public static class ImmigrantScreenOnSpawnPatch // 显示选人界面
    {
        public static void Postfix(KButton ___rejectButton )
        {
            if (ModUtils.HasRefreshMod()) return;

            ___rejectButton.GetComponentInChildren<LocText>().SetText(
                    Localization.GetLocale() != null &&
                    Localization.GetLocale().Lang == Localization.Language.Chinese
                        ? Languages.REROLL
                        : UI.IMMIGRANTSCREEN.SHUFFLE
                );
     
        }
    }
}