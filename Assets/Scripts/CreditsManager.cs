using UnityEngine;
using TMPro;

public class CreditsManager : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Text creditsText;  
    [TextArea(5, 20)]
    public string creditsContent; // Credits content
    public GameObject creditsCanvas; // Reference to the credits canvas




    void Start()
    {
        if (creditsText != null)
        {
            creditsText.text = creditsContent;  // Set the text
        }
    }

    public void ToggleCreditsCanvas()
    {
        creditsCanvas.SetActive(!creditsCanvas.activeSelf); // Toggle the credits canvas
    }
}
