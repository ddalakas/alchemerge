using UnityEngine;

public class SpawnRock : MonoBehaviour
{
    public GameObject rockPrefab;  
    public Transform spawnPoint;   

    public void SpawnProjectile()
    {
        Debug.Log("Spawning rock!");

        Instantiate(rockPrefab, spawnPoint.position, Quaternion.identity);
    }
}
