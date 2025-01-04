using System.Collections.Generic;
using System.Linq;
using HarmonyLib;

namespace crazyxyr.SelectLastCarePackage.Patches
{
    [HarmonyPatch(typeof(ImmigrantScreen), "OnProceed")]
    public static class ImmigrantScreenOnProceedPatch //按下打印按钮
    {
        public static bool Prefix(List<ITelepadDeliverable> ___selectedDeliverables)
        {
            var context = SaveGame.Instance.GetComponent<ImmigrantScreenContext>();
            if (___selectedDeliverables == null || ___selectedDeliverables.Count == 0)
            {
                Debug.Log("[最后的补给包-Fix] 没有被选中的物品,跳过这次保存");
                return true;
            }

            var selectedDeliverable = ___selectedDeliverables.First();

            if (selectedDeliverable is CarePackageContainer.CarePackageInstanceData CarePackageContainer)
            {
                context.LastSelectedCarePackageInfo = CarePackageContainer.info;
            }
            context.Skip = false;
            return true;
        }
    }
}