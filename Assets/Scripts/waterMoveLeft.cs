using UnityEngine;

public class WaterMover : MonoBehaviour
{
    public float speed = 5f; // Adjust speed in Inspector
    public float upwardDistance = 100f; // Distance to move up before moving right
    private bool movingRight = false;
    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        if (!movingRight)
        {
            transform.Translate(Vector3.up * speed);
            if (transform.position.y >= startPosition.y + upwardDistance)
            {
                movingRight = true;
            }
        }
        else
        {
            transform.Translate(Vector2.right * speed);
        }

        // Destroy when it moves off-screen (adjust values if needed)
        if (transform.position.x > 1600) 
        {
            Destroy(gameObject);
        }
    }
}

