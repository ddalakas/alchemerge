using UnityEngine;
using TMPro;

public class PrimalIconPositioner : MonoBehaviour
{
    public TMP_Text text;           // Reference to the dynamically sized text
    public RectTransform icon;      // Reference to the icon's RectTransform
    public float offset;      // Fixed spacing after the text

    void Start()
    {
        UpdatePrimalIconPosition();
    }

    void UpdatePrimalIconPosition()
    {
        // Calculate the rendered width of the text
        float textWidth = text.preferredWidth;

        RectTransform textRect = text.GetComponent<RectTransform>(); // Get the RectTransform component of the text

        // Anchor to the left
        textRect.anchorMin = new Vector2(0, 0);
        textRect.anchorMax = new Vector2(0, 0);
        textRect.pivot = new Vector2(0, 0.5f); // Set pivot to left

        // Set the icon's position relative to the text's position plus an offset
        Vector2 newPosition = icon.position;

        if (text.text == "Fire")
        {
            newPosition.x = text.rectTransform.position.x + textWidth * 1.5f + offset;
        }

        else if (text.text == "Wind")
        {
            newPosition.x = text.rectTransform.position.x + textWidth * 1.5f + offset + 10f;
        }
       
        icon.position = newPosition; // Set the new position
    }
}
