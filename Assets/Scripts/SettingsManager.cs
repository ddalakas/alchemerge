using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsManager : MonoBehaviour
{   

    public static SettingsManager Instance;

    [Header("UI References")]
    public Button codexEnableButton;
    public TMP_Text codexStatusText; // Text displayed on top of the button(ON/OFF)

    public GameObject settingsCanvas; // Reference to the settings canvas

    private bool isCodexEnabled = true; // Default to enabled

    void Start()
    {   

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

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
    }

    // UIManager will call this to check if the Codex is enabled
    public bool IsCodexEnabled()
    {
        return isCodexEnabled;
    }

    
    public void ToggleSettings()
    {
        // Toggle the settings menu
        gameObject.SetActive(!gameObject.activeSelf);
    }

}
 
