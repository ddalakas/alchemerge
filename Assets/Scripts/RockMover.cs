using UnityEngine;

public class RockMover : MonoBehaviour
{
    public float speed = 15f; // Adjust speed in Inspector

    void Update()
    {
        transform.Translate(Vector2.right * speed);

        // Destroy when it moves off-screen (adjust value if needed)
        if (transform.position.x > 2000) 
        {
            Destroy(gameObject);
        }
    }
}
