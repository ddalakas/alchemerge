using UnityEngine;
using TMPro;

public class IconPositioner : MonoBehaviour
{
    public TMP_Text text;           // Reference to the dynamically sized text
    public RectTransform icon;      // Reference to the icon's RectTransform
    public float offset;      // Fixed spacing after the text

    void Start()
    {
        UpdateIconPosition();
    }

    void UpdateIconPosition()
    {
        // Calculate the rendered width of the text
        float textWidth = text.preferredWidth;

        RectTransform textRect = text.GetComponent<RectTransform>(); // Get the RectTransform component of the text

        // Anchor to the left
        textRect.anchorMin = new Vector2(0, 0);
        textRect.anchorMax = new Vector2(0, 0);
        textRect.pivot = new Vector2(0, 0.5f); // Set pivot to left

        float mulFactor = 1.5f;
        if (textWidth > 180f)
        {
            mulFactor = 1.4f;
        }
        else if (textWidth < 50f)
        {
            mulFactor = 2f;
        }

        // Set the icon's position relative to the text's position plus an offset
        Vector2 newPosition = icon.position;

        // Custom offsets for specific sprites
        if (text.text == "Fire")
        {
            newPosition.x = text.rectTransform.position.x + textWidth * mulFactor;
            newPosition.y = text.rectTransform.position.y + 10f;
        }
        else if (text.text == "Earth" || text.text == "Plant")
        {
            newPosition.x = text.rectTransform.position.x + textWidth * mulFactor + offset + 10f;
        }
        else if (text.text == "Lava")
        {
            newPosition.x = text.rectTransform.position.x + textWidth * mulFactor + offset + 10f;
            newPosition.y = text.rectTransform.position.y + 5f;
        }
        else if (text.text == "Sand")
        {
            newPosition.x = text.rectTransform.position.x + textWidth * mulFactor + offset + 10f;
            newPosition.y = text.rectTransform.position.y + 10f;
        }
        else if (text.text == "Cloud" || text.text == "Glacier")
        {
            newPosition.x = text.rectTransform.position.x + textWidth * mulFactor + offset + 10f;
        }
        else if (text.text == "Oasis")
        {
            newPosition.x = text.rectTransform.position.x + textWidth * mulFactor + offset + 10f;
            newPosition.y = text.rectTransform.position.y - 3f;
        }
        else if (text.text == "Glass")
        {
            newPosition.x = text.rectTransform.position.x + textWidth * mulFactor + offset;
            newPosition.y = text.rectTransform.position.y - 5f;
        }
        else if (text.text == "Sinkhole")
        {
            newPosition.x = text.rectTransform.position.x + textWidth * mulFactor + offset + 10f;
            newPosition.y = text.rectTransform.position.y - 5f;
        }
        else if (text.text == "Petrified Forest")
        {
            newPosition.x = text.rectTransform.position.x + textWidth * mulFactor + offset + 10f;
            newPosition.y = text.rectTransform.position.y + 5f;
        }
        else if (text.text == "Ash")
        {
            newPosition.x = text.rectTransform.position.x + textWidth * mulFactor + 7f;
            newPosition.y = text.rectTransform.position.y - 3f;
        }
        else if (text.text == "Obsidian")
        {
            newPosition.x = text.rectTransform.position.x + textWidth * mulFactor + 10f;
            newPosition.y = text.rectTransform.position.y - 5f;
        }
        else if (text.text == "Arctic Exile")
        {
            newPosition.x = text.rectTransform.position.x + textWidth * mulFactor + offset + 5f;
        }
        else
        {
            newPosition.x = text.rectTransform.position.x + textWidth * mulFactor + offset;
        }
        icon.position = newPosition; // Set the new position
    }
}
