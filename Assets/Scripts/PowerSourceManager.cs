using UnityEngine;
using System.Collections.Generic;

[DefaultExecutionOrder(0)] // Ensure this script runs after PowerSourceDataLoader
public class PowerSourceManager : MonoBehaviour
{
    public static PowerSourceList powerSourceList; // List of all PowerSources
    public Sprite[] powerSourceSprites; // Array of all PowerSource sprites
    public static Dictionary<string, Sprite> spriteDict = new Dictionary<string, Sprite>(); // Dictionary to store PowerSource sprites
    public static Dictionary<string, PowerSourceData> powerSourceDict = new Dictionary<string, PowerSourceData>(); // Dictionary to store PowerSource data

    private void Awake()
    {
        // Load PowerSource sprites into Power Source Sprite dictionary
        foreach (Sprite sprite in powerSourceSprites)
        {
            if (!spriteDict.ContainsKey(sprite.name)) // Check if the sprite is not already in the dictionary
            {
                spriteDict.Add(sprite.name, sprite);
            }
        }

        // Load PowerSource data into Power Source Data dictionary
        foreach (PowerSourceData ps in powerSourceList.PowerSources)
        {
            if (!powerSourceDict.ContainsKey(ps.powerSourceName)) // Check if the Power Source data is not already in the dictionary
            {
                powerSourceDict.Add(ps.powerSourceName, ps);
            }
        }
        DontDestroyOnLoad(gameObject); // Prevent this object from being destroyed when loading a new scene
    }

    public static Sprite GetPowerSourceSprite(string spriteName)
    {
        // Get PowerSource sprite by name
        if (spriteDict.ContainsKey(spriteName))
        {
            return spriteDict[spriteName];
        }
        else
        {
            Debug.LogError($"PowerSource sprite {spriteName} not found.");
            return null;
        }
    }
    public static PowerSourceData GetPowerSourceData(string powerSourceName)
    {
        // Get PowerSource data by name
        if (powerSourceDict.ContainsKey(powerSourceName))
        {
            return powerSourceDict[powerSourceName];
        }
        else
        {
            Debug.LogError($"PowerSource data {powerSourceName} not found.");
            return null;
        }
    }
}