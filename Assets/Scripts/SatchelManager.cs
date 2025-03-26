using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SatchelManager : MonoBehaviour
{
    [System.Serializable]
    public class SatchelSlot
    {
        public bool isOccupied = false;
        public Transform slotTransform;
    }

    public GameObject powerSourcePrefab; // Reference to the PowerSource prefab
    public Canvas mainCanvas;
    public TextMeshProUGUI nameText;

    public SatchelSlot[] satchelSlots;

    public void SpawnPowerSource(PowerSourceData data)
    {
        // Create the PowerSource

        foreach (SatchelSlot satchelSlot in satchelSlots)
        {
            if (!satchelSlot.isOccupied)
            {
                Debug.Log("PowerSource spawned.");

                GameObject newPowerSource = Instantiate(powerSourcePrefab, satchelSlot.slotTransform);
                PowerSource ps = newPowerSource.GetComponent<PowerSource>();
                ps.satchelSlot = satchelSlot;

                // Set properties
                ps.powerSourceData.powerSourceName = data.powerSourceName;
                ps.powerSourceData.attack = data.attack;
                ps.powerSourceData.defence = data.defence;
                ps.powerSourceData.health = data.health;

                ps.powerSourceImage = newPowerSource.GetComponent<Image>(); // Get and set the Image component of the PowerSource
                ps.powerSourceImage.sprite = PowerSourceManager.spriteDict[data.powerSourceName]; // Set sprite based on PowerSource name

                // Get the RectTransform
                RectTransform rectTransform = newPowerSource.GetComponent<RectTransform>();
                // Set position explicitly
                rectTransform.anchoredPosition = new Vector2(0, 0); // Center of parent

                // Initialize with required references to canvas and text object
                ps.Initialize(mainCanvas, nameText);
                satchelSlot.isOccupied = true;
                break;
            }
        }
    }

    public void SpawnDefaultPowerSource()
    {
        SpawnPowerSource(PowerSourceManager.powerSourceList.PowerSources[0]); // Spawn the first PowerSource in the list
    }
}
