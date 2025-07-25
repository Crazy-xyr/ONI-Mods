using HarmonyLib;


namespace Unlock_Cheat.Nosublimate
{
    [HarmonyPatch(typeof(Sublimates), "Sim200ms")]
    public static class Sublimates_Sim200ms
    {
        public static bool Prefix(Sublimates __instance, float dt)
        {
            SimHashes elementID = __instance.GetComponent<PrimaryElement>().ElementID;
            return elementID != SimHashes.SlimeMold && elementID != SimHashes.ToxicSand && elementID != SimHashes.ToxicMud && elementID != SimHashes.BleachStone && elementID != SimHashes.DirtyWater;
        }
    }
}
