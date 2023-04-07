using MelonLoader;
using HarmonyLib;
using Il2Cpp;
using one_more_enemy_press_turn_06;

[assembly: MelonInfo(typeof(OneMoreEnemyPressTurn06), "One more enemy Press Turn (ver. 0.6)", "1.0.0", "Matthiew Purple")]
[assembly: MelonGame("アトラス", "smt3hd")]

namespace one_more_enemy_press_turn_06;
public class OneMoreEnemyPressTurn06 : MelonMod
{
    [HarmonyPatch(typeof(nbMainProcess), nameof(nbMainProcess.nbSetPressMaePhase))]
    private class Patch
    {
        public static void Postfix()
        {
            short activeunit = nbMainProcess.nbGetMainProcessData().activeunit;
            if (activeunit >= 4 && nbMainProcess.nbGetMainProcessData().press4_p < 8)
            {
                nbMainProcess.nbGetMainProcessData().press4_p++;
                nbMainProcess.nbGetMainProcessData().press4_ten++;
            }
        }
    }
}
