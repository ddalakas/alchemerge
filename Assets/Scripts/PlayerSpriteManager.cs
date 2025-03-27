using UnityEngine;
using System.Collections.Generic;

public class PlayerSpriteManager : MonoBehaviour
{

    public Sprite[] playerSprites; // Array of all Player sprites
    public static Dictionary<string, Sprite> spriteDict = new Dictionary<string, Sprite>(); // Dictionary to store Player sprites

    private void Awake()
    {
        // Load Player sprites into dictionary on Awake
        foreach (Sprite sprite in playerSprites)
        {
            spriteDict.Add(sprite.name, sprite);
        }
    }

    public static Sprite GetPlayerSprite(string spriteName)
    {
        // Get Player sprite by name
        if (spriteDict.ContainsKey(spriteName))
        {
            return spriteDict[spriteName];
        }
        else
        {
            Debug.LogError($"Player sprite {spriteName} not found.");
            return null;
        }
    }

}