using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance; // Singleton pattern ensuring only one instance with public access

    [Header("Bottom Right Player HUD")] // Current player sees their HUD in the bottom right corner

    public TextMeshProUGUI bottomRightAttackText;
    public TextMeshProUGUI bottomRightDefenceText;
    public TextMeshProUGUI bottomRightHealthText;
    public Image bottomRightSprite;


    [Header("Top Left Player HUD")] // Current player sees the opponent's HUD in the top left corner
    public TextMeshProUGUI topLeftAttackText;
    public TextMeshProUGUI topLeftDefenceText;
    public TextMeshProUGUI topLeftHealthText;
    public Image topLeftSprite;

    public Button settingsButton; // Reference to the settings button

    private void Awake()
    {
        if (Instance == null)
            Instance = this; // Set the instance to this object
        else
            Destroy(gameObject); // Destroy the object the script is attached to
    }

    void Start()
    {
        UpdateBottomRightHUD(PlayerManager.player1.health, PlayerManager.player1.attack, PlayerManager.player1.defence, bottomRightSprite.sprite);
        UpdateTopLeftHUD(PlayerManager.player2.health, PlayerManager.player2.attack, PlayerManager.player2.defence, topLeftSprite.sprite);
        AddListeners(); // Add listeners
    }
    void Update()
    {
        // Check if mouse is clicked
        if (Input.GetMouseButtonDown(0)) // Left mouse button click
        {
            SwitchHUD();
        }
    }

    public void UpdateBottomRightHUD(int health, int attack, int defence, Sprite sprite) // Updates the bottom right HUD with the player's stats
    {
        bottomRightAttackText.text = $"{attack}";
        bottomRightDefenceText.text = $"{defence}";
        bottomRightHealthText.text = $"{health}";
        bottomRightSprite.sprite = sprite;
    }

    public void UpdateTopLeftHUD(int health, int attack, int defence, Sprite sprite) // Updates the top left HUD with the opponent's stats
    {
        topLeftAttackText.text = $"{attack}";
        topLeftDefenceText.text = $"{defence}";
        topLeftHealthText.text = $"{health}";
        topLeftSprite.sprite = sprite;
    }

    public void SwitchHUD()
    {
        Sprite tempSprite;
        if (TurnManager.isPlayer1Turn)
        {
            tempSprite = bottomRightSprite.sprite;
            UpdateBottomRightHUD(PlayerManager.player1.health, PlayerManager.player1.attack, PlayerManager.player1.defence, topLeftSprite.sprite);
            UpdateTopLeftHUD(PlayerManager.player2.health, PlayerManager.player2.attack, PlayerManager.player2.defence, tempSprite);
        }
        else
        {
            tempSprite = topLeftSprite.sprite;
            UpdateTopLeftHUD(PlayerManager.player1.health, PlayerManager.player1.attack, PlayerManager.player1.defence, bottomRightSprite.sprite);
            UpdateBottomRightHUD(PlayerManager.player2.health, PlayerManager.player2.attack, PlayerManager.player2.defence, tempSprite);
        }
    }

    public void AddListeners()
    {
        GameObject soundManager = GameObject.Find("SoundManager");
        Debug.Log(soundManager ? "SoundManager found" : "SoundManager not found");
        if (soundManager != null)
        {
            settingsButton.onClick.AddListener(() => soundManager.GetComponent<SoundManager>().PlaySFX(
                SoundManager.instance.buttonClickSFX
            ));
        }
    }
}
