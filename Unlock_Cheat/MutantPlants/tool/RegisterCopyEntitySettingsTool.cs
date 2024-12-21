using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using Unlock_Cheat.MutantPlants;


namespace Unlock_Cheat.MutantPlantsCopySetting
{
    [HarmonyPatch(typeof(PlayerController), nameof(PlayerController.OnPrefabInit))]
    public static class RegisterCopyEntitySettingsTool
    {
        static void Postfix(PlayerController __instance)
        {
            var interfaceTools = new List<InterfaceTool>(__instance.tools);
            var critterCopyTool = new GameObject(typeof(MutantCopySettingsTool).Name);
            critterCopyTool.AddComponent<MutantCopySettingsTool>();

            // Reparent tool to the player controller, then enable/disable to load it
            critterCopyTool.transform.SetParent(__instance.gameObject.transform);
            critterCopyTool.gameObject.SetActive(true);
            critterCopyTool.gameObject.SetActive(false);

            interfaceTools.Add(critterCopyTool.GetComponent<InterfaceTool>());
            __instance.tools = interfaceTools.ToArray();
        }



    }



    [HarmonyPatch(typeof(Assets), nameof(Assets.CreatePrefabs))]
    class ApplySettingsToDefs
    {
        static void Postfix()
        {
            var cmpMap = new ComponentMapper(new()
            {
                (typeof(Uprootable), typeof(MutantCopyButton))
            });

            foreach (var prefab in Assets.Prefabs)
                cmpMap.ApplyMap(prefab.gameObject);
        }
    }




    public class ComponentMapper : ComponentMapper<object>
    {
        /// <inheritdoc cref="ComponentMapper{T}"/>
        public ComponentMapper(List<(Type flagCmp, Type addCmp)> map) : base(map.Select(x => (x.flagCmp, x.addCmp, (object)null)).ToList())
        { }

        /// <inheritdoc cref="ComponentMapper{T}.ApplyMap(GameObject, Func{T, bool})"/>
        public void ApplyMap(GameObject go) => ApplyMap(go, _ => true);
    }

    public class ComponentMapper<T>
    {
        private readonly List<(Type flagCmp, Type addCmp, T filter)> map;

        /// <summary>
        /// Maps detected components to new components that will be added.
        /// Processed first to last, so interface fallbacks should be after specific implementations.
        /// </summary>
        public ComponentMapper(List<(Type flagCmp, Type addCmp, T filter)> map) => this.map = map;

        /// <summary>
        /// Apply component map to GO, adding new components if applicable.
        /// </summary>
        /// <param name="go">The GameObject to work on</param>
        /// <param name="shouldAdd">Function uses filter to determine if the new component should be added to the GO</param>
        public void ApplyMap(GameObject go, Func<T, bool> shouldAdd)
        {
            var typeToAdd = GetTypeToAdd(go, shouldAdd);
            if (typeToAdd != null)
                go.AddComponent(typeToAdd);
        }

        private Type GetTypeToAdd(GameObject go, Func<T, bool> shouldAdd)
        {
            foreach (var (flagCmp, addCmp, filter) in map)
                if (flagCmp != null && HasComponentOrDef(flagCmp, go) && shouldAdd(filter))
                    return addCmp;
            return null;

            bool HasComponentOrDef(Type cmpOrDef, GameObject go) => go.GetComponent(cmpOrDef) ?? go.GetDef(cmpOrDef) != null;
        }
    }
    public static class GameObjectExt
    {
        public static Component GetReflectionComp(this GameObject go, string typeString)
        {
            var type = AccessTools.TypeByName(typeString);
            if (type == null)
                return null;
            else
                return go.GetComponent(type);
        }

        public static StateMachine.BaseDef GetDef(this GameObject go, Type type)
        {
            var smc = go.GetComponent<StateMachineController>();
            if (smc == null)
                return null;

            return smc.cmpdef.defs.FirstOrDefault(x => x.GetType() == type);
        }
    }
}