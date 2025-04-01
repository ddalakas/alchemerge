using UnityEngine;
[DefaultExecutionOrder(-100)] // Ensure this script runs before PowerSourceManager
public class PowerSourceDataLoader : MonoBehaviour
{
    public TextAsset jsonFile; // Reference to the JSON file containg the PowerSource data.

    void Awake()
    {
        if (jsonFile != null)
        {
            PowerSourceList powerSourceData = JsonUtility.FromJson<PowerSourceList>(jsonFile.text);

            if (powerSourceData != null && powerSourceData.PowerSources != null)
            {
                foreach (PowerSourceData ps in powerSourceData.PowerSources)
                {
                   Debug.Log($"Loaded: {ps.powerSourceName}, Attack: {ps.attack}, Defence: {ps.defence}, Health: {ps.health}, Tier:  {ps.tier}");
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