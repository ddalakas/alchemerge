using UnityEngine;

public class SpawnWater : MonoBehaviour
{
    public GameObject waterPrefab;  
    public Transform spawnPoint;   

    public void SpawnProjectileWater()
    {
        Debug.Log("Spawning water!");

        Instantiate(waterPrefab, spawnPoint.position, Quaternion.identity);
    }
}
