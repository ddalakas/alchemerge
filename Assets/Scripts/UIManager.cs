using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Numerics;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance; // Singleton pattern ensuring only one instance with public access

    [Header("Bottom Right Player HUD")] // Current player sees their HUD in the bottom right corner

    public TextMeshProUGUI bottomRightAttackText;
    public TextMeshProUGUI bottomRightDefenceText;
    public TextMeshProUGUI bottomRightHealthText;
    public Image bottomRightSprite;
    public TextMeshProUGUI bottomRightPlayerText;


    [Header("Top Left Player HUD")] // Current player sees the opponent's HUD in the top left corner
    public TextMeshProUGUI topLeftAttackText;
    public TextMeshProUGUI topLeftDefenceText;
    public TextMeshProUGUI topLeftHealthText;
    public Image topLeftSprite;

    public TextMeshProUGUI topLeftPlayerText;

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
        UpdateBottomRightHUD("Player 1", PlayerManager.player1.health, PlayerManager.player1.attack, PlayerManager.player1.defence, bottomRightSprite.sprite);
        UpdateTopLeftHUD("Player 2", PlayerManager.player2.health, PlayerManager.player2.attack, PlayerManager.player2.defence, topLeftSprite.sprite);
        AddPlayListeners(); // Add listeners to the buttons and execute the appropriate functions
        UpdatePlayerTurnText(); // Update the player turn text
    }
    public void UpdatePlayerTurnText() // Updates the player turn text
    {
        if (TurnManager.isPlayer1Turn)
            playerTurnText.text = "Player 1's Turn";
        else
            playerTurnText.text = "Player 2's Turn";
    }

    public void UpdateStats(int attack, int defence, int health) // Updates the player's stats on the screen
    {
        bottomRightAttackText.text = $"{attack}";
        bottomRightDefenceText.text = $"{defence}";
        bottomRightHealthText.text = $"{health}";
    }

    public void UpdateBottomRightHUD(string playerText, int health, int attack, int defence, Sprite sprite) // Updates the bottom right HUD with the player's stats
    {
        bottomRightPlayerText.text = playerText;
        bottomRightAttackText.text = $"{attack}";
        bottomRightDefenceText.text = $"{defence}";
        bottomRightHealthText.text = $"{health}";
        bottomRightSprite.sprite = sprite;
    }

    public void UpdateTopLeftHUD(string playerText, int health, int attack, int defence, Sprite sprite) // Updates the top left HUD with the opponent's stats
    {
        topLeftPlayerText.text = playerText;
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
            UpdateBottomRightHUD("Player 1", PlayerManager.player1.health, PlayerManager.player1.attack, PlayerManager.player1.defence, topLeftSprite.sprite);
            UpdateTopLeftHUD("Player 2", PlayerManager.player2.health, PlayerManager.player2.attack, PlayerManager.player2.defence, tempSprite);
        }
        else
        {
            tempSprite = topLeftSprite.sprite;
            UpdateTopLeftHUD("Player 1", PlayerManager.player1.health, PlayerManager.player1.attack, PlayerManager.player1.defence, bottomRightSprite.sprite);
            UpdateBottomRightHUD("Player 2", PlayerManager.player2.health, PlayerManager.player2.attack, PlayerManager.player2.defence, tempSprite);
        }

        // Swap base element images
        tempSprite = bottomBaseElementImage.sprite;
        bottomBaseElementImage.sprite = topBaseElementImage.sprite;
        topBaseElementImage.sprite = tempSprite;

        UnityEngine.Vector3 temp = bottomRightSprite.rectTransform.localScale;
        temp.x = temp.x * -1; // Flip the sprite horizontally
        bottomRightSprite.rectTransform.localScale = temp;

        temp = topLeftSprite.rectTransform.localScale;
        temp.x = temp.x * -1; // Flip the sprite horizontally
        topLeftSprite.rectTransform.localScale = temp;
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

            // Settings Button
            settingsButton.onClick.AddListener(() => soundManagerObj.PlaySFX(SoundManager.instance.buttonClickSFX)); // Play button click SFX

            // Commit Button
            commitButton.onClick.AddListener(() =>
            {
                soundManagerObj.PlaySFX(SoundManager.instance.buttonClickSFX);
                TurnManager.SwitchTurn(); // Switch turn
                Debug.Log("It is now " + (TurnManager.isPlayer1Turn ? "Player 1's" : "Player 2's") + " turn.");

                UpdatePlayerTurnText(); // Update the player turn text
                SwitchPlayHUD(); // Switch the HUD between Player1 and Player2
                PowerSource temp = FindAnyObjectByType<PowerSource>(); // Find a PowerSource object
                temp.nameText.text = ""; // Clear the PowerSource name text

                PlayingFieldManager.Instance.FlipPlayingField(); // Flip the playing field
                playCanvas.enabled = false; // Hide Play Canvas
                playerTurnCanvas.enabled = true;   // Show Player Turn Canvas  
            }
            );

            // Codex Button
            codexButton.onClick.AddListener(() => soundManagerObj.PlaySFX(SoundManager.instance.buttonClickSFX));

            // Base Element Selection
            void SelectBaseElement(Player.element baseElement) // Function to select base element
            {
                baseElementSelections++; // Increment selections counter
                soundManagerObj.PlaySFX(SoundManager.instance.buttonClickSFX); // Play button click sound

                if (TurnManager.isPlayer1Turn)
                {
                    Debug.Log("Player 1 selected " + baseElement);
                    PlayerManager.player1.baseElement = baseElement; // Assign base element to player 1
                }
                else
                {
                    Debug.Log("Player 2 selected " + baseElement);
                    PlayerManager.player2.baseElement = baseElement; // Assign base element to player 2
                }
                AssignBaseElementSprite(baseElement, TurnManager.isPlayer1Turn ? bottomBaseElementImage : topBaseElementImage);
                TurnManager.SwitchTurn(); // Switch turn
                UpdatePlayerTurnText(); // Update the player turn text

                baseElementCanvas.enabled = false; // Hide Base Element Picker
                playerTurnCanvas.enabled = true;   // Show Player Turn Canvas
            }

            // Listener for each base element button
            windButton.onClick.AddListener(() => SelectBaseElement(Player.element.Wind));
            fireButton.onClick.AddListener(() => SelectBaseElement(Player.element.Fire));
            waterButton.onClick.AddListener(() => SelectBaseElement(Player.element.Water));
            earthButton.onClick.AddListener(() => SelectBaseElement(Player.element.Earth));

            // Play Button
            playButton.onClick.AddListener(() =>
            {
                soundManagerObj.PlaySFX(SoundManager.instance.buttonClickSFX); // Play button click sound
                playerTurnCanvas.enabled = false;
                if (baseElementSelections < 2) // If both players have not selected base elements
                    baseElementCanvas.enabled = true;
                else
                {
                    playCanvas.enabled = true; // Show Play Canvas
                    SceneTransition sceneTransition = FindAnyObjectByType<SceneTransition>(); // Find the SceneTransition component
                    StartCoroutine(sceneTransition.FadeInScreen()); // Fade in the screen
                    UpdatePlayerTurnText(); // Update the player turn text
                }
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