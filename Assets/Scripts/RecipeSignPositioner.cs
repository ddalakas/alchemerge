using UnityEngine;

public class RecipeSignPositioner : MonoBehaviour
{
    public RectTransform leftIcon; // The icon on the left
    public RectTransform middleText; // The text field that needs centering
    public RectTransform rightText; // The text field on the right

    public float spacing = 10f; // Extra spacing between elements

    void Start()
    {
        PositionMiddleText();
    }
    void PositionMiddleText()
    {
        if (leftIcon == null || rightText == null || middleText == null)
        {
            Debug.LogError("Assign all RectTransforms in the inspector.");
            return;
        }

        // Get the world position of the right edge of the left icon
        Vector3 leftIconWorldPosition = leftIcon.TransformPoint(new Vector3(leftIcon.rect.width / 2, 0, 0));

        // Get the world position of the left edge of the right text
        Vector3 rightTextWorldPosition = rightText.TransformPoint(new Vector3(-rightText.rect.width / 2, 0, 0));

        // Calculate the position of the equals sign (between the left icon and right text)
        Vector3 targetPosition = (leftIconWorldPosition + rightTextWorldPosition) / 2;

        // Adjust by an offset for spacing
        targetPosition.x += spacing;

        // Convert the world position to local position for the equals sign's parent container
        Vector3 localPosition = middleText.parent.InverseTransformPoint(targetPosition);

        // Set the local position of the equals sign
        middleText.localPosition = new Vector3(localPosition.x, middleText.localPosition.y, middleText.localPosition.z);
    }
}
