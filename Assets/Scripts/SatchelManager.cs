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

    public static SatchelManager Instance; // Singleton instance

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this; // Set the singleton instance
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public GameObject powerSourcePrefab; // Reference to the PowerSource prefab
    public Canvas mainCanvas;
    public TextMeshProUGUI nameText;

    public SatchelSlot[] satchelSlots; // Array of SatchelSlots

    public PowerSource[] player1SatchelPowerSources = new PowerSource[4]; // Array to store Player 1 PowerSources in the Satchel
    public PowerSource[] player2SatchelPowerSources = new PowerSource[4]; // Array to store Player 2 PowerSources in the Satchel

    public void SpawnPowerSource(PowerSourceData data)

    {
        // Spawn the PowerSource in the first available SatchelSlot (for the current player)
        for (int i = 0; i < satchelSlots.Length; i++)
        {
            if (!satchelSlots[i].isOccupied)
            {
                Debug.Log("PowerSource spawned.");

                GameObject newPowerSource = Instantiate(powerSourcePrefab, satchelSlots[i].slotTransform);
                newPowerSource.transform.localPosition = Vector3.zero; // Reset position within the slot
                newPowerSource.transform.localScale = Vector3.one; // Ensure correct scaling

                PowerSource ps = newPowerSource.GetComponent<PowerSource>();
                ps.satchelSlot = satchelSlots[i];

                if (TurnManager.isPlayer1Turn)
                {
                    player1SatchelPowerSources[i] = ps; // Store the PowerSource in the Player 1 Satchel array
                }
                else
                {
                    player2SatchelPowerSources[i] = ps; // Store the PowerSource in the Player 2 Satchel array
                }

                // Set properties
                ps.powerSourceData.powerSourceName = data.powerSourceName;
                ps.powerSourceData.attack = data.attack;
                ps.powerSourceData.defence = data.defence;
                ps.powerSourceData.health = data.health;

                ps.powerSourceImage = newPowerSource.GetComponent<Image>(); // Get and set the Image component of the PowerSource
                ps.powerSourceImage.sprite = PowerSourceManager.GetPowerSourceSprite(data.powerSourceName); // Set sprite based on PowerSource name

                // Get the RectTransform
                RectTransform rectTransform = newPowerSource.GetComponent<RectTransform>();
                // Set position explicitly
                rectTransform.anchoredPosition = new Vector2(0, 0); // Center of parent

                // Initialize with required references to canvas and text object
                ps.Initialize(mainCanvas, nameText);
                satchelSlots[i].isOccupied = true;
                break;
            }
        }
    }

    public void SwitchPlayerSatchel()
    {
        // Switch the PowerSources in the Satchel based on the current player
        if (TurnManager.isPlayer1Turn)
        {
            for (int i = 0; i < player1SatchelPowerSources.Length; i++)
            {
                if (player1SatchelPowerSources[i] != null)
                {
                    player1SatchelPowerSources[i].gameObject.SetActive(true);
                }
                if (player2SatchelPowerSources[i] != null)
                {
                    player2SatchelPowerSources[i].gameObject.SetActive(false);

                    if (player1SatchelPowerSources[i] == null)
                    {
                        satchelSlots[i].isOccupied = false; // Ensure the slot is marked as unoccupied
                    }

                }
            }
        }
        else
        {
            for (int i = 0; i < player2SatchelPowerSources.Length; i++)
            {
                if (player2SatchelPowerSources[i] != null)
                {
                    player2SatchelPowerSources[i].gameObject.SetActive(true);
                }
                if (player1SatchelPowerSources[i] != null)
                {
                    player1SatchelPowerSources[i].gameObject.SetActive(false);

                    if (player2SatchelPowerSources[i] == null)
                    {
                        satchelSlots[i].isOccupied = false; // Ensure the slot is marked as unoccupied
                    }
                }
            }
        }
    }
}
