using MelonLoader;
using HarmonyLib;
using Il2Cpp;
using one_more_enemy_press_turn_06;
using Il2Cppnewdata_H;

[assembly: MelonInfo(typeof(OneMoreEnemyPressTurn06), "One more enemy Press Turn [Bosses only] (ver. 0.6)", "1.0.0", "Matthiew Purple")]
[assembly: MelonGame("アトラス", "smt3hd")]

namespace one_more_enemy_press_turn_06;
public class OneMoreEnemyPressTurn06 : MelonMod
{
    // After initiating a phase
    [HarmonyPatch(typeof(nbMainProcess), nameof(nbMainProcess.nbSetPressMaePhase))]
    private class Patch
    {
        public static void Postfix()
        {
            short activeunit = nbMainProcess.nbGetMainProcessData().activeunit; // Get the formindex of the first active demon

            // If that demon is an enemy, they have a boss and their side and the enemies don't already have 8 press turns
            if (activeunit >= 4 && Utility.hasABossOnTheirSide() && nbMainProcess.nbGetMainProcessData().press4_p < 8)
            {
                nbMainProcess.nbGetMainProcessData().press4_p++; // Add 1 full press turn
                nbMainProcess.nbGetMainProcessData().press4_ten++; // Add 1 total press turn
            }
        }
    }

    static public List<ushort> fakeBosses = new List<ushort>()
    {
        318, // Will o' Wisp (Tutorial)
        319, // Preta (Tutorial)
        260, // Incubus (Nihilo)
        261, // Koppa Tengu (Nihilo)
        313  // Succubus (Nihilo)
    };

    private class Utility
    {
        // Checks if there is a boss on the enemy's side
        public static bool hasABossOnTheirSide()
        {
            foreach (datUnitWork_t item in nbMainProcess.nbGetMainProcessData().enemyunit)
            {
                // If it's an actual boss (and not a mini-boss) who's still alive
                if (item.id >= 256 && item.hp != 0 && !fakeBosses.Contains(item.id)) return true;

                // Special case for Succubus (313) because she gets only the additional press turn ONLY when she's alone (not as Berith's ally)
                if (item.id == 313 && item.hp != 0 && nbMainProcess.nbGetMainProcessData().enemyunit.Length == 1) return true;
            }

            return false;
        }
    }
}
