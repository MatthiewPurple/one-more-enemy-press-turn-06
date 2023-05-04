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

    // List of enemies with an ID greater than 255 but that aren't bosses
    static public List<ushort> fakeBosses = new List<ushort>()
    {
        318, // Will o' Wisp (Tutorial)
        319, // Preta (Tutorial)
        260, // Incubus (Nihilo)
        261, // Koppa Tengu (Nihilo)
        359, // Virtue (White Rider)
        360, // Power (Red Rider)
        361, // Legion (Black Rider)
        358  // Loa (Pale Rider)
    };

    private class Utility
    {
        // Checks if there is a boss on the enemy's side
        public static bool hasABossOnTheirSide()
        {
            foreach (datUnitWork_t item in nbMainProcess.nbGetMainProcessData().enemyunit)
            {
                // If it's an actual boss (and not a mini-boss) who's still alive
                if (item.id >= 256 && !fakeBosses.Contains(item.id) && item.hp != 0) return true;

                // Special case for the Succubus chest boss
                else if (item.id == 117 && nbMainProcess.nbGetMainProcessData().encno == 990) return true;
            }

            return false;
        }
    }
}
