using Random = System.Random;

public class PowerSourceGenerator
{
    private static readonly Random random = new(); // Random number generator

    public static string[] GeneratePowerSources()
    {
        string[] powerSources = { "Fire", "Water", "Earth", "Wind" }; // Array of PowerSources

        // Generate two random power sources
        int firstPowerSourceIndex = random.Next(powerSources.Length); // Random index for the first PowerSource
        int secondPowerSourceIndex = random.Next(powerSources.Length); // Random index for the second PowerSource

        return new string[] // Return an array of two random PowerSources
        {
            powerSources[firstPowerSourceIndex], // First PowerSource
            powerSources[secondPowerSourceIndex] // Second PowerSource
        };
    }
}
