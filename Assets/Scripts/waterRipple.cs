using UnityEngine;

public class WaterRipple : MonoBehaviour
{
    public float waveSpeed = 1f;
    public float minWaveHeight = 0.02f; // Min height variation
    public float maxWaveHeight = 0.07f; // Max height variation

    private Vector3 startPosition;
    private float randomWaveHeight;
    private float phaseOffset;

    void Start()
    {
        startPosition = transform.position;
        
        // Each water tile gets a slightly different wave height
        randomWaveHeight = Random.Range(minWaveHeight, maxWaveHeight);
        
        // Assign a random phase offset so tiles don’t move in sync
        phaseOffset = Random.Range(0f, Mathf.PI * 2);
    }

    void Update()
    {
        // Generate wave movement (ensuring it only moves upward)
        float yOffset = Mathf.Abs(Mathf.Sin(Time.time * waveSpeed + phaseOffset)) * randomWaveHeight;
        
        // Apply movement
        transform.position = startPosition + new Vector3(0, yOffset, 0);
    }
}
