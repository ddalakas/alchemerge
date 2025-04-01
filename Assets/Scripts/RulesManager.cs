using UnityEngine;
using TMPro;

public class RulesManager : MonoBehaviour
{
    public TMP_Text rulesText;
    [TextArea(5, 20)]
    public string rulesContent; // Rules content

    void Start()
    {
        if (rulesText != null)
        {
            rulesText.text = rulesContent;  // Set the text
        }
    }

    public void ToggleRulesCanvas()
    {
        gameObject.SetActive(!gameObject.activeSelf); // Toggle the rules canvas
    }
}
