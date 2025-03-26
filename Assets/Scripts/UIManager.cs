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

    // Play Phase UI
    public Button settingsButton;
    public Button commitButton;
    public Button codexButton;

    public Image bottomBaseElementImage; // Image for the current player's base element
    public Image topBaseElementImage; // Image for the opponent's base element

    public Sprite fireElementSprite; // Sprite for the fire element
    public Sprite waterElementSprite; // Sprite for the water element
    public Sprite windElementSprite; // Sprite for the wind element
    public Sprite earthElementSprite; // Sprite for the earth element

    // Base Element Picker UI
    public Button windButton;
    public Button fireButton;
    public Button waterButton;
    public Button earthButton;

    // Player Turn UI
    public TextMeshProUGUI playerTurnText;
    public Button playButton;

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
        AddPlayListeners(); // Add listeners to the buttons and execute the appropriate functions
        UpdatePlayerTurnText(); // Update the player turn text
        UpdateCodexButtonState(); // Update the codex button state
    }

    void UpdateCodexButtonState()
    {
        if (CodexManager.Instance != null)
        {
            bool isCodexEnabled = SettingsManager.Instance.IsCodexEnabled(); // Check if codex is enabled
            codexButton.gameObject.SetActive(isCodexEnabled); // Show/Hide codex button
        }
    }
    public void UpdatePlayerTurnText() // Updates the player turn text
    {
        if (TurnManager.isPlayer1Turn)
            playerTurnText.text = "Player 1's Turn";
        else
            playerTurnText.text = "Player 2's Turn";
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

    public void SwitchPlayHUD()
    {
        Sprite tempSprite;
        if (TurnManager.isPlayer1Turn)
        {
            tempSprite = bottomRightSprite.sprite;
            UpdateBottomRightHUD(PlayerManager.player1.health, PlayerManager.player1.attack, PlayerManager.player1.defence, topLeftSprite.sprite);
            UpdateTopLeftHUD(PlayerManager.player2.health, PlayerManager.player2.attack, PlayerManager.player2.defence, tempSprite);

            // Swap base element images
            tempSprite = bottomBaseElementImage.sprite;
            bottomBaseElementImage.sprite = topBaseElementImage.sprite;
            topBaseElementImage.sprite = tempSprite;
        }
        else
        {
            tempSprite = topLeftSprite.sprite;
            UpdateTopLeftHUD(PlayerManager.player1.health, PlayerManager.player1.attack, PlayerManager.player1.defence, bottomRightSprite.sprite);
            UpdateBottomRightHUD(PlayerManager.player2.health, PlayerManager.player2.attack, PlayerManager.player2.defence, tempSprite);
        }
    }

    private int baseElementSelections = 0; // Tracks selections

    public void AddPlayListeners() // Adds listeners to the buttons and executes the appropriate functions
    {
        Canvas playerTurnCanvas = GameObject.Find("Player Turn Canvas").GetComponent<Canvas>();
        Canvas baseElementCanvas = GameObject.Find("Base Element Canvas").GetComponent<Canvas>();
        Canvas playCanvas = GameObject.Find("PlayCanvas").GetComponent<Canvas>();

        GameObject soundManager = GameObject.Find("SoundManager");
        if (soundManager != null)
        {
            SoundManager soundManagerObj = soundManager.GetComponent<SoundManager>();

            // Assign button sounds
            settingsButton.onClick.AddListener(() => soundManagerObj.PlaySFX(SoundManager.instance.buttonClickSFX));
            commitButton.onClick.AddListener(() => soundManagerObj.PlaySFX(SoundManager.instance.buttonClickSFX));
            codexButton.onClick.AddListener(() => soundManagerObj.PlaySFX(SoundManager.instance.buttonClickSFX));

            // Base Element Selection
            void SelectBaseElement(Player.element baseElement) // Function to select base element
            {
                baseElementSelections++; // Increment selections counter
                soundManagerObj.PlaySFX(SoundManager.instance.buttonClickSFX); // Play button click sound

                if (TurnManager.isPlayer1Turn)
                {
                    PlayerManager.player1.baseElement = baseElement; // Assign base element to player 1
                }
                else
                {
                    PlayerManager.player2.baseElement = baseElement; // Assign base element to player 2
                }

                TurnManager.SwitchTurn(); // Switch turn

                baseElementCanvas.enabled = false; // Hide Base Element Picker

                if (baseElementSelections == 2) // If both players have selected base elements
                {
                    playCanvas.enabled = true;
                }
                else
                    playerTurnCanvas.enabled = true;   // Show Player Turn Canvas
                UpdatePlayerTurnText();
                AssignBaseElementSprite(baseElement, TurnManager.isPlayer1Turn ? bottomBaseElementImage : topBaseElementImage);
            }

            // Listener for each base element button
            windButton.onClick.AddListener(() => SelectBaseElement(Player.element.Wind));
            fireButton.onClick.AddListener(() => SelectBaseElement(Player.element.Fire));
            waterButton.onClick.AddListener(() => SelectBaseElement(Player.element.Water));
            earthButton.onClick.AddListener(() => SelectBaseElement(Player.element.Earth));



            // Play Button: Only transition after both players select base elements
            playButton.onClick.AddListener(() =>
            {
                soundManagerObj.PlaySFX(SoundManager.instance.buttonClickSFX); // Play button click sound
                playerTurnCanvas.enabled = false;
                if (baseElementSelections < 2) // If both players have not selected base elements
                    baseElementCanvas.enabled = true;
                else
                    playCanvas.enabled = true;
            });
        }
    }
    public void AssignBaseElementSprite(Player.element baseElement, Image image) // Assigns the base element image to the player
    {
        switch (baseElement)
        {
            case Player.element.Fire:
                image.sprite = fireElementSprite;
                break;
            case Player.element.Water:
                image.sprite = waterElementSprite;
                break;
            case Player.element.Wind:
                image.sprite = windElementSprite;
                break;
            case Player.element.Earth:
                image.sprite = earthElementSprite;
                break;
        }
    }
}