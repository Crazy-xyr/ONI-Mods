using HarmonyLib;


namespace Unlock_Cheat.Nosublimate
{
    [HarmonyPatch(typeof(Sublimates), "Sim200ms")]
    public static class Sublimates_Sim200ms
    {
        public static bool Prefix(Sublimates __instance, float dt)
        {
            SimHashes element = __instance.GetComponent<PrimaryElement>().ElementID;
            if (element == SimHashes.SlimeMold || element == SimHashes.ToxicSand || element == SimHashes.BleachStone)
            {
                // Skip original method entirely for these materials
                return false;
            }

            // Allow normal behavior for other materials
            return true;
        }
    }
}
