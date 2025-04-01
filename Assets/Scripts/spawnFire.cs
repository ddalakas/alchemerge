using UnityEngine;

public class SpawnFire : MonoBehaviour
{
    public GameObject firePrefab;  
    public Transform spawnPoint;   

    public void SpawnFireProjectile()
    {
        Debug.Log("Spawning fire!");

        Instantiate(firePrefab, spawnPoint.position, Quaternion.identity);
    }
}
