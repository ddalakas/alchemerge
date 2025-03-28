using UnityEngine;

public class SunGlow : MonoBehaviour
{
    public float glowSpeed = 1f; // Controls how fast the glow pulses
    public float minGlow = 1f;   // Minimum brightness
    public float maxGlow = 2f;   // Maximum brightness

    private SpriteRenderer spriteRenderer;
    private Material material;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        // Ensure we are using a material with an emission property
        if (spriteRenderer.material != null)
        {
            material = spriteRenderer.material;
        }
    }

    void Update()
    {
        if (material != null)
        {
            // Create a pulsing effect using sine wave
            float glowIntensity = Mathf.Lerp(minGlow, maxGlow, (Mathf.Sin(Time.time * glowSpeed) + 1) / 2);
            material.SetFloat("_Emission", glowIntensity);
        }
    }
}
