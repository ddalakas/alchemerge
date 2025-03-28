using UnityEngine;

public class CloudIdleMovement : MonoBehaviour
{
    public float minSpeed = 0.4f;  // Minimum floating speed
    public float maxSpeed = 0.5f;  // Maximum floating speed
    public float minRange = 5f;  // Minimum movement range
    public float maxRange = 10f;  // Maximum movement range

    private Vector3 startPosition;
    private float speed;
    private float range;
    private float timeOffset;
    private int direction; // 1 or -1 for random left/right movement

    void Start()
    {
        startPosition = transform.position;
        
        // Randomize movement parameters
        speed = Random.Range(minSpeed, maxSpeed);
        range = Random.Range(minRange, maxRange);
        timeOffset = Random.Range(0f, Mathf.PI * 2f); // Ensures clouds don’t sync
        
        // Randomize movement direction (left or right)
        direction = Random.value > 0.5f ? 1 : -1;
    }

    void Update()
    {
        // Independent horizontal movement
        float offsetX = Mathf.Sin(Time.time * speed + timeOffset) * range * direction;
        
        // Optional vertical floating
        float offsetY = Mathf.Cos(Time.time * speed + timeOffset) * (range / 2);
        
        transform.position = startPosition + new Vector3(offsetX, offsetY, 0);
    }
}
