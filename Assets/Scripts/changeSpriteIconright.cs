using UnityEngine;
using UnityEngine.UI;

public class CombatPhaseSpriteHandlerLeft : MonoBehaviour
{
    private Image playerImage;
    private Player player; // Reference to the player instance

    void Awake()
    {
        playerImage = GetComponent<Image>();
        player = PlayerManager.player2;
        AssignSprite();
    }

    void AssignSprite()
    {
        string path = "profiles/right/";
        Sprite loadedSprite = Resources.Load<Sprite>(path+"earth");
        if (player == null || playerImage == null)
            return;
        
        switch (player.baseElement)
        {
            case Player.element.Earth:
                loadedSprite = Resources.Load<Sprite>(path+"earth");
                break;
            case Player.element.Fire:
                 loadedSprite = Resources.Load<Sprite>(path + "fire");
                break;
            case Player.element.Water:
                 loadedSprite = Resources.Load<Sprite>(path + "water");
                break;
            case Player.element.Wind:
                 loadedSprite = Resources.Load<Sprite>(path + "wind");
                break;
        }
        if (loadedSprite != null)
        {
            playerImage.sprite = loadedSprite;
        }
        else
        {
            Debug.LogError($"Failed to load sprite: {path}");
        }
    }
}
