using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using HarmonyLib;
using crazyxyr.Commons;
using UnityEngine;

namespace crazyxyr.SelectLastCarePackage.Patches
{
    [HarmonyPatch(typeof(ImmigrantScreen), "OnRejectAll")]
    public static class ImmigrantScreenOnRejectAllPatch //按下拒绝全部按钮
    {
        private static float _lastTime;

        public static bool Prefix(ImmigrantScreen __instance)
        {
            if (ModUtils.HasRefreshMod())
            {
                Debug.Log("启用了刷新选人Mod，跳过");
                return true;
            }

            Debug.Log("没有启用刷新选人Mod，刷新");

            if (Time.realtimeSinceStartup - _lastTime < 0.3)
            {
                Debug.Log("-------------" + Time.realtimeSinceStartup + "-----------lastTime:" + _lastTime);
                return false;
            }

            _lastTime = Time.realtimeSinceStartup;
            Traverse instance = Traverse.Create(__instance);
            List<ITelepadDeliverableContainer> deliverableContainerList = null;
            deliverableContainerList = instance.Field("containers").GetValue<List<ITelepadDeliverableContainer>>();
            deliverableContainerList.ForEach(c => UnityEngine.Object.Destroy(c.GetGameObject()));
            deliverableContainerList.Clear();
            instance.Method("InitializeContainers").GetValue();
            ImmigrantScreenMethod.ShowButton(__instance);
             _lastTime = Time.realtimeSinceStartup;
            return false;
        }


  

    }




}