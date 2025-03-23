using UnityEngine;
using UnityEngine.UI;

public class FlyingSprite : MonoBehaviour
{
    public float speed = 400f; // constant speed value
    public Vector2 startXRange = new Vector2(-1088f, 1088f); // off-screen left & right
    public Vector2 startYRange = new Vector2(-540f, 540f); // within the screen height
    public Sprite[] sprites;

    private RectTransform rectTransform;
    private Image imageComponent;
    private Vector2 moveDirection;
    bool enteredScreen = false;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>(); // Get the RectTransform component of the sprite we want to move
        imageComponent = GetComponent<Image>(); // Get the Image component of the sprite we want to move
        ResetPosition(); // Set the initial position and direction
    }

    void Update()
    {

        rectTransform.anchoredPosition += speed * Time.deltaTime * moveDirection; // Move the sprite
        rectTransform.Rotate(0, 0, 30f * Time.deltaTime); // Rotate the sprite 30 degrees

        if (!enteredScreen)
        {
            if (rectTransform.anchoredPosition.x > -960 && rectTransform.anchoredPosition.x < 960 &&
                rectTransform.anchoredPosition.y > -540 && rectTransform.anchoredPosition.y < 540)
            {
                enteredScreen = true;
            }
        }

        // If it moves off any screen edge, reset
        if (rectTransform.anchoredPosition.x < -1088 || rectTransform.anchoredPosition.x > 1088 ||
            rectTransform.anchoredPosition.y < -540 || rectTransform.anchoredPosition.y > 540)
        {
            enteredScreen = false;
            ResetPosition();
        }
    }

    void ResetPosition()
    {
        bool spawnsLeft = Random.value < 0.5f; // 50% chance of spawning on the left side or right side
        float startX = spawnsLeft ? -1088f : 1088f; // Off-screen left or right
        float startY = Random.Range(-540f, 540f); // Within screen height
        rectTransform.anchoredPosition = new Vector2(startX, startY); // Set the position

        if (sprites.Length > 0)
        {
            imageComponent.sprite = sprites[Random.Range(0, sprites.Length)]; // Random sprite each time
        }

        float xDir = spawnsLeft ? 1 : -1; // Move towards the opposite side of the screen
        float yDir = Random.Range(-0.3f, 0.3f); // Random vertical direction (up or down) with a bias towards the center
        if (yDir > -0.05f && yDir < 0.05f) yDir = 0.1f; // Ensure a minimum vertical speed

        moveDirection = new Vector2(xDir, yDir).normalized; // normalize the vector to ensure constant speed
    }
}
