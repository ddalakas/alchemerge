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

    public void SpawnPowerSourceInSatchel(PowerSourceData data)
    {
        Debug.Log("Occupation: " + satchelSlots[0].isOccupied + " " + satchelSlots[1].isOccupied + " " + satchelSlots[2].isOccupied + " " + satchelSlots[3].isOccupied);

        // Spawn the PowerSource in the first available SatchelSlot (for the current player)
        for (int i = 0; i < satchelSlots.Length; i++)
        {
            if (!satchelSlots[i].isOccupied)
            {
                SpawnPowerSource(satchelSlots, i, data); // Spawn the PowerSource in the SatchelSlot
                break; // Exit the loop after spawning
            }
        }
    }

    public void SpawnPowerSource(SatchelSlot[] satchelSlots, int i, PowerSourceData data)
    {

        GameObject newPowerSource = Instantiate(powerSourcePrefab, satchelSlots[i].slotTransform); // Instantiate the PowerSource prefab in the SatchelSlot

        PowerSource ps = newPowerSource.GetComponent<PowerSource>(); // Get the PowerSource component from the prefab
        ps.satchelSlot = satchelSlots[i]; // Assign the SatchelSlot to the PowerSource
        ps.playingFieldSlot = null; // Set the PlayingFieldSlot to null
        ps.isDraggable = true; // Set the PowerSource to be draggable

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

        // Set the element type based on PowerSource name
        if (data.powerSourceName == "Fire")
        {
            ps.elementType = Player.element.Fire;
            ps.powerSourceImage.rectTransform.sizeDelta = new Vector2(95, 95); // Adjust to 95% of the original size
        }
        else if (data.powerSourceName == "Water")
        {
            ps.elementType = Player.element.Water;
        }
        else if (data.powerSourceName == "Earth")
        {
            ps.elementType = Player.element.Earth;
            ps.powerSourceImage.rectTransform.sizeDelta = new Vector2(95, 95); // Adjust to 95% of the original size
        }
        else if (data.powerSourceName == "Wind")
        {
            ps.elementType = Player.element.Wind;
            // Shrink the Wind icon to fit the slot
            ps.powerSourceImage.rectTransform.sizeDelta = new Vector2(95, 95); // Adjust to 95% of the original size
        }

        // Get the RectTransform
        RectTransform rectTransform = newPowerSource.GetComponent<RectTransform>();
        // Set position explicitly
        rectTransform.anchoredPosition = new Vector2(0, 0); // Center of parent

        // Initialize with required references to canvas and text object
        ps.Initialize(mainCanvas, nameText);
        satchelSlots[i].isOccupied = true; // Mark the slot as occupied
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
                    satchelSlots[i].isOccupied = true; // Mark the slot as occupied
                    player1SatchelPowerSources[i].gameObject.SetActive(true);
                }
                else
                {
                    satchelSlots[i].isOccupied = false; // Mark the slot as unoccupied
                }

                if (player2SatchelPowerSources[i] != null)
                {
                    player2SatchelPowerSources[i].gameObject.SetActive(false);
                }
            }
        }
        else
        {
            for (int i = 0; i < player2SatchelPowerSources.Length; i++)
            {
                if (player2SatchelPowerSources[i] != null)
                {
                    satchelSlots[i].isOccupied = true; // Mark the slot as occupied
                    player2SatchelPowerSources[i].gameObject.SetActive(true);
                }
                else
                {
                    satchelSlots[i].isOccupied = false; // Mark the slot as unoccupied
                }
                if (player1SatchelPowerSources[i] != null)
                {
                    player1SatchelPowerSources[i].gameObject.SetActive(false);
                }
            }
        }
    }
}
