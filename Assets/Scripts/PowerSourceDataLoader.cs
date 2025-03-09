using UnityEngine;

public class PowerSourceLoader : MonoBehaviour
{
    public TextAsset jsonFile; // Reference of JSON file

    void Start()
    {
        if (jsonFile != null)
        {
            PowerSourceList powerSourceData = JsonUtility.FromJson<PowerSourceList>(jsonFile.text);

            if (powerSourceData != null && powerSourceData.PowerSources != null)
            {
                foreach (PowerSource ps in powerSourceData.PowerSources)
                {
                    Debug.Log($"Loaded: {ps.name}, Attack: {ps.attack}, Defence: {ps.defence}, Health: {ps.health}");
                }
            }
            else
            {
                Debug.LogError("Failed to parse PowerSource data or PowerSources is empty.");
            }
        }
        else
        {
            Debug.LogError("JSON file not assigned.");
        }
    }
}