using UnityEngine;

public class SpawnWind : MonoBehaviour
{
    public GameObject windPrefab;  
    public Transform spawnPoint;   

    public void SpawnProjectileWind()
    {
        Debug.Log("Spawning wind!");

        Instantiate(windPrefab, spawnPoint.position, Quaternion.identity);
    }
}
