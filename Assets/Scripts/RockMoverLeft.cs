using UnityEngine;

public class RockMoverLeft : MonoBehaviour
{
    public float speed = 5f; // Adjust speed in Inspector

    void Update()
    {
        transform.Translate(Vector2.left * speed);

        // Destroy when it moves off-screen (adjust value if needed)
        if (transform.position.x < 300) 
        {
            Destroy(gameObject);
        }
    }
}
