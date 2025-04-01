using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class SettingsManager : MonoBehaviour
{

    public static SettingsManager Instance; // Singleton pattern

    [Header("UI References")]
    public Button codexEnableButton; // Button to enable/disable the codex

    public Button backToMainMenuButton; // Button to go back to the main menu

    public Button backButton; // Button to go back to the previous menu

    public TMP_Text codexStatusText; // Text displayed on top of the button(ON/OFF)

    public GameObject settingsCanvas; // Reference to the settings canvas

    private bool isCodexEnabled = true; // Default to enabled


    void Awake()
    {
        if (Instance == null) // Singleton pattern
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist through scene changes
            DontDestroyOnLoad(settingsCanvas); // Persist through scene changes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate settings manager
            Destroy(settingsCanvas); // Destroy duplicate settings canvas
        }
    }

    void Start()
    {
        // Add a on-click listener to the button
        if (codexEnableButton != null)
        {
            codexEnableButton.onClick.AddListener(OnCodexEnableButtonClicked);
        }

    }

    private void OnCodexEnableButtonClicked()
    {
        // Toggle the codex boolean and update the button text
        ToggleCodexEnabled();
    }


    // Toggle the in-game codex on or off
    private void ToggleCodexEnabled()
    {
        isCodexEnabled = !isCodexEnabled; // Toggle boolean
        codexStatusText.text = isCodexEnabled ? "ON" : "OFF"; // Set text to "On" if enabled, "Off" if disabled
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateCodexButtonState(); // Update the button state in the UIManager
        }
    }

    // UIManager will call this to check if the Codex is enabled
    public bool IsCodexEnabled()
    {
        return isCodexEnabled;
    }


    public void ToggleSettings()
    {
        // Toggle the settings menu
        settingsCanvas.SetActive(!settingsCanvas.activeSelf);


        // Toggle Back to Main Menu button
        if (backToMainMenuButton != null)
        {
            if (SceneManager.GetActiveScene().name == "MainMenuScene")
            {
                backToMainMenuButton.gameObject.SetActive(false); // Hide the button
            }
            else
            {
                backToMainMenuButton.gameObject.SetActive(true); // Show the button
            }
        }
    }

    public void LoadMainMenu()
    {
        Debug.Log("Loading Main Menu");
        ToggleSettings(); // Close the settings menu if it's open
        UIManager.Instance.soundManager.audioSource.Stop(); // Stop the current music
        UIManager.Instance.soundManager.audioSource.volume = 0.0f; // Set the volume to 0%

        SceneTransition sceneTransition = FindAnyObjectByType<SceneTransition>(); // Find the SceneTransition component
        sceneTransition.ChangeScene("MainMenuScene", false, false); // Change the scene to Main Menu Scene with no additive load
        GameManager.instance.ChangeGameState(GameManager.GameState.MainMenu); // Change the game state to Main Menu
    }
}


