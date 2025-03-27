using UnityEngine;
using System.Collections.Generic;

public class PowerSourceManager : MonoBehaviour
{
    public static PowerSourceList powerSourceList; // List of all PowerSources

    public Sprite[] powerSourceSprites; // Array of all PowerSource sprites
    public static Dictionary<string, Sprite> spriteDict = new Dictionary<string, Sprite>(); // Dictionary to store PowerSource sprites

    private void Awake()
    {
        // Load PowerSource sprites into dictionary on Awake
        foreach (Sprite sprite in powerSourceSprites)
        {
            spriteDict.Add(sprite.name, sprite);
        }
    }

}