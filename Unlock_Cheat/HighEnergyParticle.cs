using HarmonyLib;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace Unlock_Cheat.HighEnergyParticle_Patch
{
    internal class HighEnergyParticle_Patch {
        [HarmonyPatch(typeof(HighEnergyParticle), "MovingUpdate")]

        internal class HighEnergyParticle_MovingUpdate
        {
            public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                Debug.Log(" === HighEnergyParticle Transpiler applied === ");
                return instructions.Manipulator(
                    instr => instr.opcode == OpCodes.Ldc_R4 && ((float)instr.operand) == 0.1f,  // 匹配条件
                    instr => instr.operand = 0f  // 修改动作：替换 operand 为 0.0f
                );
            }

		}

        [HarmonyPatch(typeof(HighEnergyParticleRedirector), "LaunchParticle")]
        internal class HighEnergyParticleRedirector_LaunchParticle
        {
            public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                Debug.Log(" === LaunchParticle Transpiler applied === ");
                return instructions.Manipulator(
                    instr => instr.opcode == OpCodes.Ldc_R4 && ((float)instr.operand) == 0.1f,  // 匹配条件
                    instr => instr.operand = 0f  // 修改动作：替换 operand 为 0.0f
                );
            }

        }
    }

}