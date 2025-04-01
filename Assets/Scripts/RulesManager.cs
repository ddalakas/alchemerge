using UnityEngine;
using TMPro;

public class RulesManager : MonoBehaviour
{
    public TMP_Text rulesText;
    [TextArea(5, 20)]
    public string rulesContent; // Rules content
    public GameObject rulesCanvas; // Reference to the rules canvas

    void Start()
    {
        if (rulesText != null)
        {
            rulesText.text = rulesContent;  // Set the text
        }
    }

    public void ToggleRulesCanvas()
    {
        rulesCanvas.SetActive(!rulesCanvas.activeSelf); // Toggle the rules canvas
    }
}
