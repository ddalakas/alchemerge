using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

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
    public bool commitClickedBefore = false; // Tracks if the commit button has been clicked before
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

    // Power Source Selection UI
    public Button powerSource1Button;
    public Image powerSource1ButtonImage;
    public Button powerSource2Button;
    public Image powerSource2ButtonImage;
    public string[] powerSourceChoices; // Used to store the current PowerSource options

    // References to game objects
    GameObject settingsManagerObj;
    GameObject codexManagerObj;
    GameObject soundManagerObj;

    SettingsManager settingsManager;
    CodexManager codexManager;
    public SoundManager soundManager; // Reference to the SoundManager

    public GameObject playGrid; // Find the Play Grid object

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this; // Set the instance to this object
            playGrid = GameObject.Find("PlayGrid");
        }
        else
            Destroy(gameObject); // Destroy the object the script is attached to
    }

    void Start()
    {
        UpdateBottomRightHUD("Player 1", PlayerManager.player1.health, PlayerManager.player1.attack, PlayerManager.player1.defence, bottomRightSprite.sprite);
        UpdateTopLeftHUD("Player 2", PlayerManager.player2.health, PlayerManager.player2.attack, PlayerManager.player2.defence, topLeftSprite.sprite);
        AddPlayListeners(); // Add listeners to the buttons and execute the appropriate functions
        UpdatePlayerTurnText(); // Update the player turn text
        UpdateCodexButtonState(); // Update the codex button state
    }

    public void UpdateCodexButtonState()
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

        Vector3 temp = bottomRightSprite.rectTransform.localScale;
        temp.x = temp.x * -1; // Flip the sprite horizontally
        bottomRightSprite.rectTransform.localScale = temp;

        temp = topLeftSprite.rectTransform.localScale;
        temp.x = temp.x * -1; // Flip the sprite horizontally
        topLeftSprite.rectTransform.localScale = temp;
    }

    private int baseElementSelections = 0; // Tracks selections

    public void AddPlayListeners() // Adds listeners to the buttons and executes the appropriate functions
    {

        // Canvas References
        Canvas playerTurnCanvas = GameObject.Find("Player Turn Canvas").GetComponent<Canvas>();
        Canvas baseElementCanvas = GameObject.Find("Base Element Canvas").GetComponent<Canvas>();
        Canvas playCanvas = GameObject.Find("PlayCanvas").GetComponent<Canvas>();
        Canvas powerSourceSelectionCanvas = GameObject.Find("Power Source Selection Canvas").GetComponent<Canvas>();

        GameObject playGrid = GameObject.Find("PlayGrid"); // Find the Play Grid object

        if (powerSourceSelectionCanvas != null) Debug.Log("Power Source Selection Canvas found");

        // References to managers
        settingsManagerObj = GameObject.Find("SettingsManager");
        codexManagerObj = GameObject.Find("CodexManager");
        soundManagerObj = GameObject.Find("SoundManager");

        settingsManager = settingsManagerObj.GetComponent<SettingsManager>();
        codexManager = codexManagerObj.GetComponent<CodexManager>();
        soundManager = soundManagerObj.GetComponent<SoundManager>();

        if (soundManagerObj != null && settingsManagerObj != null)
        {
            // Settings Button
            settingsButton.onClick.AddListener(() =>
            {
                soundManager.PlaySFX(SoundManager.instance.buttonClickSFX); // Play button click SFX
                settingsManager.ToggleSettings(); // Show settings canvas
            }
            );
            // Codex Button
            codexButton.onClick.AddListener(() =>
            {
                soundManager.PlaySFX(SoundManager.instance.buttonClickSFX); // Play button click SFX
                codexManager.ToggleCodex(); // Show codex canvas
            }
            );

            SceneTransition sceneTransition = FindAnyObjectByType<SceneTransition>(); // Find the SceneTransition component

            // Commit Button
            commitButton.onClick.AddListener(() =>
            {
                soundManager.PlaySFX(SoundManager.instance.buttonClickSFX);

                if (commitClickedBefore) // Player 2 clicked the commit button
                {
                    commitClickedBefore = false; // Reset the clicked state
                    if (!(PlayerManager.player1.activePowerSource == null && PlayerManager.player2.activePowerSource == null))
                    {
                        StartCoroutine(DisableAndUpdatePlayViewWithDelay(2f, playCanvas, playerTurnCanvas, baseElementCanvas, powerSourceSelectionCanvas, playGrid)); // Disable the Play View after 2 seconds
                        sceneTransition.ChangeScene("CombatScene", true, false); // Change the scene to Combat Phase with additive load

                        Scene combatScene = SceneManager.GetSceneByName("CombatScene");
                        if (combatScene.IsValid())
                        {
                            SceneManager.SetActiveScene(combatScene); // Set the active scene to Combat Scene
                        }
                        GameManager.instance.ChangeGameState(GameManager.GameState.CombatPhase); // Change the game state to Combat Phase
                    }
                    else
                    {
                        UpdatePlayerView(playCanvas, playerTurnCanvas); // Update the playing field view
                    }
                }
                else
                {
                    // Player 1 clicked the commit button
                    commitClickedBefore = true; // Set the clicked state
                    UpdatePlayerView(playCanvas, playerTurnCanvas); // Update the playing field view
                }
            }
            );

            // Base Element Selection
            void SelectBaseElement(Player.element baseElement) // Function to select base element
            {
                baseElementSelections++; // Increment selections counter
                soundManager.PlaySFX(SoundManager.instance.buttonClickSFX); // Play button click sound

                if (TurnManager.isPlayer1Turn)
                {
                    Debug.Log("Player 1 selected " + baseElement);
                    PlayerManager.player1.baseElement = baseElement; // Assign base element to Player 1

                    // Assign Player 1 sprite based on their chosen base element
                    PlayerManager.player1.spriteName = "Player1_" + baseElement.ToString();
                    bottomRightSprite.sprite = PlayerSpriteManager.GetPlayerSprite(PlayerManager.player1.spriteName + "_1"); // set portrait sprite
                }
                else
                {
                    Debug.Log("Player 2 selected " + baseElement);
                    PlayerManager.player2.baseElement = baseElement; // Assign base element to Player 2

                    // Assign Player 2 sprite based on their chosen base element
                    PlayerManager.player2.spriteName = "Player2_" + baseElement.ToString(); // Assign sprite name
                    topLeftSprite.sprite = PlayerSpriteManager.GetPlayerSprite(PlayerManager.player2.spriteName + "_1"); // set portrait sprite
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
                soundManager.PlaySFX(SoundManager.instance.buttonClickSFX); // Play button click sound
                playerTurnCanvas.enabled = false;
                if (baseElementSelections < 2) // If both players have not selected base elements
                    baseElementCanvas.enabled = true;
                else
                {
                    GetPowerSourceOptions(); // Get PowerSource options for the current player
                    powerSourceSelectionCanvas.enabled = true; // Show Power Source Selection Canvas
                    SceneTransition sceneTransition = FindAnyObjectByType<SceneTransition>(); // Find the SceneTransition component
                    StartCoroutine(sceneTransition.FadeInScreen()); // Fade in the screen
                    UpdatePlayerTurnText(); // Update the player turn text
                }
            });

            // Power Source Selection
            powerSource1Button.onClick.AddListener(() =>
            {
                soundManager.PlaySFX(SoundManager.instance.buttonClickSFX); // Play button click sound

                SatchelManager.Instance.SwitchPlayerSatchel(); // Switch the player satchel

                // Spawn PowerSource 1
                SatchelManager.Instance.SpawnPowerSourceInSatchel(PowerSourceManager.GetPowerSourceData(powerSourceChoices[0]));
                powerSourceSelectionCanvas.enabled = false; // Hide Power Source Selection Canvas

                playCanvas.enabled = true; // Show Play Canvas
            });
            powerSource2Button.onClick.AddListener(() =>
            {
                soundManager.PlaySFX(SoundManager.instance.buttonClickSFX); // Play button click sound

                SatchelManager.Instance.SwitchPlayerSatchel(); // Switch the player satchel

                // Spawn PowerSource 2
                SatchelManager.Instance.SpawnPowerSourceInSatchel(PowerSourceManager.GetPowerSourceData(powerSourceChoices[1]));
                powerSourceSelectionCanvas.enabled = false; // Hide Power Source Selection Canvas

                playCanvas.enabled = true; // Show Play Canvas
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

    public void GetPowerSourceOptions() // Get PowerSource options for the current player
    {
        powerSourceChoices = PowerSourceGenerator.GeneratePowerSources(); // Generate PowerSources for Player 1
        powerSource1ButtonImage.sprite = PowerSourceManager.GetPowerSourceSprite(powerSourceChoices[0]); // Set PowerSource 1 sprite
        powerSource2ButtonImage.sprite = PowerSourceManager.GetPowerSourceSprite(powerSourceChoices[1]); // Set PowerSource 2 sprite
    }

    public void UpdatePlayerView(Canvas playCanvas, Canvas playerTurnCanvas)
    {
        PlayingFieldManager.Instance.FlipPlayingField(); // Flip the playing field
        TurnManager.SwitchTurn(); // Switch turn
        Debug.Log("It is now " + (TurnManager.isPlayer1Turn ? "Player 1's" : "Player 2's") + " turn.");

        UpdatePlayerTurnText(); // Update the player turn text
        SwitchPlayHUD(); // Switch the HUD between Player1 and Player2

        PowerSource temp = FindAnyObjectByType<PowerSource>(); // Find a PowerSource object
        temp.nameText.text = ""; // Clear the PowerSource name text
        playCanvas.enabled = false; // Hide Play Canvas
        playerTurnCanvas.enabled = true;   // Show Player Turn Canvas
    }

    public IEnumerator DisableAndUpdatePlayViewWithDelay(float seconds, Canvas playCanvas,
    Canvas playerTurnCanvas, Canvas baseElementCanvas, Canvas powerSourceSelectionCanvas, GameObject playGrid) // Wait for a specified number of seconds
    {
        yield return new WaitForSeconds(seconds);
        UpdatePlayerView(playCanvas, playerTurnCanvas); // Update the player view
        playCanvas.enabled = false; // Hide Play Canvas
        playerTurnCanvas.enabled = false; // Hide Player Turn Canvas
        baseElementCanvas.enabled = false; // Hide Base Element Picker
        powerSourceSelectionCanvas.enabled = false; // Hide Power Source Selection Canvas
        playGrid.SetActive(false); // Hide the grid 
    }

    public void EnablePlayView(Canvas playCanvas, Canvas playerTurnCanvas, Canvas baseElementCanvas, Canvas powerSourceSelectionCanvas, GameObject playGrid) // Enable the Play View
    {
        UpdateBottomRightHUD("Player 1", PlayerManager.player1.health, PlayerManager.player1.attack, PlayerManager.player1.defence, bottomRightSprite.sprite);
        UpdateTopLeftHUD("Player 2", PlayerManager.player2.health, PlayerManager.player2.attack, PlayerManager.player2.defence, topLeftSprite.sprite);

        playCanvas.enabled = true; // Show Play Canvas
        playerTurnCanvas.enabled = true; // Hide Player Turn Canvas
        baseElementCanvas.enabled = false; // Hide Base Element Picker
        powerSourceSelectionCanvas.enabled = false; // Hide Power Source Selection Canvas
        playGrid.SetActive(true); // Show the grid
    }
}