using UnityEngine;

public class WaterMoverRight : MonoBehaviour
{
    public float speed = 5f; // Adjust speed in Inspector
    public float upwardDistance = 100f; // Distance to move up before moving right
    private bool movingleft = false;
    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        if (!movingleft)
        {
            transform.Translate(Vector3.up * speed);
            if (transform.position.y >= startPosition.y + upwardDistance)
            {
                movingleft = true;
            }
        }
        else
        {
            transform.Translate(Vector2.left * speed);
        }

        // Destroy when it moves off-screen (adjust values if needed)
        if (transform.position.x < 300) 
        {
            Destroy(gameObject);
        }
    }
}

