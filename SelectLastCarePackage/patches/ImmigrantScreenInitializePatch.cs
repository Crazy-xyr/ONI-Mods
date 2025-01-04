using HarmonyLib;
using System;
using UnityEngine.UI;
using UnityEngine;

namespace crazyxyr.SelectLastCarePackage.Patches2
{
    [HarmonyPatch(typeof(ImmigrantScreen), "Initialize")]
    public static class ImmigrantScreenInitializePatch
    {

        private static GameObject Title;
        public static bool Prefix(ImmigrantScreen __instance)
        {


            if (Title == null)   //Title modification code reference 'Duplicant Stat Selector' by Sgt-Imalas
            {
            Transform transform = __instance.transform.Find("Layout");
            RectTransform rectTransform = global::Util.KInstantiateUI(__instance.transform.Find("Layout/Title").gameObject, transform.gameObject, true).rectTransform();
            rectTransform.SetSiblingIndex(2);

            LayoutElement layoutElement;
            if (rectTransform.TryGetComponent<LayoutElement>(out layoutElement))
            {
                layoutElement.minHeight = 40f;
            }
                try {

                    UnityEngine.Object.Destroy(rectTransform.Find("TitleLabel").gameObject);
                    UnityEngine.Object.Destroy(rectTransform.Find("CloseButton").gameObject);


                } catch (Exception e) { 
                
                Debug.LogWarning(e);
                }
          
             rectTransform.gameObject.name = "TopButtonmodify";

             Title = rectTransform.gameObject;
           

         
            }

            return true;    

        }

        public static void Postfix(ImmigrantScreen __instance)
        {
            ImmigrantScreenMethod.ShowButton(__instance);
        }

    }
}
