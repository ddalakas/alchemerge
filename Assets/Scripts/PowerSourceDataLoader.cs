using UnityEngine;

public class PowerSourceLoader : MonoBehaviour
{
    public TextAsset jsonFile; // Reference to the JSON file containg the PowerSource data.

    void Start()
    {
        if (jsonFile != null)
        {
            PowerSourceList powerSourceData = JsonUtility.FromJson<PowerSourceList>(jsonFile.text);

            if (powerSourceData != null && powerSourceData.PowerSources != null)
            {
                foreach (PowerSourceData ps in powerSourceData.PowerSources)
                {
                    Debug.Log($"Loaded: {ps.powerSourceName}, Attack: {ps.attack}, Defence: {ps.defence}, Health: {ps.health}");
                }
                PowerSourceManager.powerSourceList = powerSourceData; // Assign the PowerSource data to the PowerSourceManager.
            }
            else
            {
                Debug.LogError("Failed to parse PowerSource data.");
            }
        }
        else
        {
            Debug.LogError("JSON file not assigned.");
        }
    }
}