using UnityEngine;
using System.Collections.Generic;

public class PlayerSpriteManager : MonoBehaviour
{

    public Sprite[] playerSprites; // Array of all Player sprites
    public static Dictionary<string, Sprite> playerSpriteDict = new Dictionary<string, Sprite>(); // Dictionary to store Player sprites

    private void Awake()
    {
        // Load Player sprites into dictionary on Awake
        foreach (Sprite sprite in playerSprites)
        {
            if (!playerSpriteDict.ContainsKey(sprite.name)) // Check if the sprite is not already in the dictionary
            {
                playerSpriteDict.Add(sprite.name, sprite);
            }
        }
    }

    public static Sprite GetPlayerSprite(string spriteName)
    {
        // Get Player sprite by name
        if (playerSpriteDict.ContainsKey(spriteName))
        {
            return playerSpriteDict[spriteName];
        }
        else
        {
            Debug.LogError($"Player sprite {spriteName} not found.");
            return null;
        }
    }

}