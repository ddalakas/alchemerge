using UnityEngine;

public class TreeSway : MonoBehaviour
{
    public float minSwaySpeed = 0.5f;  // Slowest sway speed
    public float maxSwaySpeed = 1.5f;  // Fastest sway speed
    public float minSwayAngle = 2f;    // Smallest sway angle
    public float maxSwayAngle = 5f;    // Largest sway angle

    private float swaySpeed;
    private float swayAngle;
    private float timeOffset;

    void Start()
    {
        // Randomize sway speed and angle for variety
        swaySpeed = Random.Range(minSwaySpeed, maxSwaySpeed);
        swayAngle = Random.Range(minSwayAngle, maxSwayAngle);
        timeOffset = Random.Range(0f, Mathf.PI * 2f); // Prevents synchronization
    }

    void Update()
    {
        // Calculate smooth swaying using a sine wave
        float angle = Mathf.Sin(Time.time * swaySpeed + timeOffset) * swayAngle;
        
        // Apply rotation to simulate swaying
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
