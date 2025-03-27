using UnityEngine;
using Random = System.Random;

public class PowerSourceGenerator
{
    private static readonly Random random = new(); // Random number generator

    public static string[] GeneratePowerSources(Player player)
    {
        // Get all available PowerSources
        int i = 0;
        while (player.unlockedPowerSources[i] != null)
        {
            i++; // Count the number of unlocked PowerSources
        }
        // Generate two random indices
        int firstIndex = random.Next(i);
        Debug.Log($"First index: {firstIndex}");
        int secondIndex = random.Next(i);
        Debug.Log($"Second index: {secondIndex}");

        return new string[] { player.unlockedPowerSources[firstIndex], player.unlockedPowerSources[secondIndex] }; // Return the two PowerSources
    }
}
