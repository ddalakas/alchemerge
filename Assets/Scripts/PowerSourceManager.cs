using UnityEngine;
using System.Collections.Generic;

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
            spriteDict.Add(sprite.name, sprite);
        }

        // Load PowerSource data into Power Source Data dictionary
        foreach (PowerSourceData ps in powerSourceList.PowerSources)
        {
            powerSourceDict.Add(ps.powerSourceName, ps);
        }
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