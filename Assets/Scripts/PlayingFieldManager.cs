using UnityEngine;
using UnityEngine.UI;
public class PlayingFieldManager : MonoBehaviour
{
    [System.Serializable]
    public class PlayingFieldSlot
    {
        public bool isOccupied = false;
        public Transform slotTransform;
        public Color slotColor;
    }

    public PlayingFieldSlot[] playingFieldSlots = new PlayingFieldSlot[10]; // Array of all Playing Field slots

    public PowerSource[] powerSources = new PowerSource[10]; // keep track of all PowerSources on the Playing Field

    public static PlayingFieldManager Instance; // Singleton instance

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject); // Destroy duplicate instances
        }

        DontDestroyOnLoad(gameObject); // If you want to keep this across scene changes
    }
    public PlayingFieldSlot GetValidSlot(Vector2 position)
    {
        for (int i = 0; i < playingFieldSlots.Length; i++)
        {
            if (TurnManager.isPlayer1Turn && i < 5)
            {
                if (!playingFieldSlots[i].isOccupied) // Check if the slot is not occupied
                {
                    // Check if the slot is close enough to drop for Player 1 (first 5 slots)
                    if (Vector2.Distance(position, playingFieldSlots[i].slotTransform.position) < 100f) // Adjust threshold as needed
                    {
                        return playingFieldSlots[i];
                    }
                }
            }
            else if (!TurnManager.isPlayer1Turn && i >= 5)
            {
                if (!playingFieldSlots[i].isOccupied) // Check if the slot is not occupied
                {
                    // Check if the slot is close enough to drop for Player 2 (last 5 slots)
                    if (Vector2.Distance(position, playingFieldSlots[i].slotTransform.position) < 100f) // Adjust threshold as needed
                    {
                        return playingFieldSlots[i];
                    }
                }
            }
        }
        return null; // No valid slot found
    }

    // Move a PowerSource to a specific slot
    public void AddPowerSourceToSlot(int slotIndex, PowerSource powerSource)
    {
        if (slotIndex >= 0 && slotIndex < powerSources.Length)
        {
            powerSources[slotIndex] = powerSource;
            playingFieldSlots[slotIndex].isOccupied = true;
        }
        Debug.Log("PowerSource added to slot " + slotIndex);
    }

    // Remove a PowerSource from a specific slot
    public void RemovePowerSourceFromSlot(int slotIndex)
    {
        if (slotIndex >= 0 && slotIndex < powerSources.Length)
        {
            powerSources[slotIndex] = null;
            playingFieldSlots[slotIndex].isOccupied = false;
        }
        Debug.Log("PowerSource removed from slot " + slotIndex);
    }

    // Flips the playing field slots across the diagonal
    public void FlipPlayingField()
    {
        for (int i = 0; i < 5; i++)
        {
            int mirroredIndex = 9 - i; // Reflect across the diagonal
            Vector2 tempPosition = playingFieldSlots[i].slotTransform.position;
            playingFieldSlots[i].slotTransform.position = playingFieldSlots[mirroredIndex].slotTransform.position;
            playingFieldSlots[mirroredIndex].slotTransform.position = tempPosition;
        }
    }
    // Sums the stats of all PowerSources on the Playing Field and updates the player's stats (if it's their turn)
    public void SumPowerSourceStats()
    {
        float multiplier; // multiplier
        if (TurnManager.isPlayer1Turn)
        {
            int player1Attack = 0;
            int player1Defence = 0;
            int player1Overhealth = 0;

            for (int i = 0; i < 5; i++)
            {
                if (powerSources[i] != null)
                {
                    // Check if the PowerSource is of the same element as the player's base element
                    if (powerSources[i].powerSourceData.powerSourceName == PlayerManager.player1.baseElement.ToString())
                    {
                        multiplier = 1.25f; // Increase stats by 25% if the PowerSource matches the player's base element
                        player1Attack += (int)(powerSources[i].powerSourceData.attack * multiplier);
                        player1Defence += (int)(powerSources[i].powerSourceData.defence * multiplier);
                        player1Overhealth += (int)(powerSources[i].powerSourceData.health * multiplier);
                    }
                    else
                    {
                        player1Attack += powerSources[i].powerSourceData.attack;
                        player1Defence += powerSources[i].powerSourceData.defence;
                        player1Overhealth += powerSources[i].powerSourceData.health;
                    }
                }
            }
            PlayerManager.player1.attack = player1Attack;
            PlayerManager.player1.defence = player1Defence;
            PlayerManager.player1.health = PlayerManager.player1.baseHealth + player1Overhealth; // Update health based on base health and overhealth
        }
        else
        {
            int player2Attack = 0;
            int player2Defence = 0;
            int player2Overhealth = 0;

            for (int i = 5; i < 10; i++)
            {
                if (powerSources[i] != null)
                {
                    // Check if the PowerSource is of the same element as the player's base element
                    if (powerSources[i].powerSourceData.powerSourceName == PlayerManager.player2.baseElement.ToString())
                    {
                        multiplier = 1.25f; // Increase stats by 25% if the PowerSource matches the player's base element
                        player2Attack += (int)(powerSources[i].powerSourceData.attack * multiplier);
                        player2Defence += (int)(powerSources[i].powerSourceData.defence * multiplier);
                        player2Overhealth += (int)(powerSources[i].powerSourceData.health * multiplier);
                    }
                    else
                    {
                        player2Attack += powerSources[i].powerSourceData.attack;
                        player2Defence += powerSources[i].powerSourceData.defence;
                        player2Overhealth += powerSources[i].powerSourceData.health;
                    }
                }
            }
            PlayerManager.player2.attack = player2Attack;
            PlayerManager.player2.defence = player2Defence;
            PlayerManager.player2.health = PlayerManager.player2.baseHealth + player2Overhealth; // Update health based on base health and overhealth
        }
    }

    public bool GetValidMerge(Vector2 position, PowerSource powerSourceA)
    {
        for (int i = 0; i < playingFieldSlots.Length; i++)
        {
            if (TurnManager.isPlayer1Turn && i < 5)
            {
                if (playingFieldSlots[i].isOccupied) // Check if the slot is occupied
                {
                    Debug.Log("Slot" + i + " is occupied");
                    // Check if the slot is close enough to drop for Player 1 (first 5 slots)
                    if (Vector2.Distance(position, playingFieldSlots[i].slotTransform.position) < 100f)
                    {
                        PowerSource powerSourceB = powerSources[i]; // Get the second PowerSource
                        string result = FusionMergeController.Instance.GetMergeResult(powerSourceA.powerSourceData.powerSourceName, powerSourceB.powerSourceData.powerSourceName);
                        if (result == null) Debug.Log("No valid merge result found.");
                        if (!string.IsNullOrEmpty(result)) // If a valid merge result is found
                        {
                            PlayerManager.player1.activePowerSource = ProcessMerge(powerSourceA, powerSourceB, result); // Merge the PowerSources and set the result as the active PowerSource
                            return true;
                        }
                    }
                }
            }
            else if (!TurnManager.isPlayer1Turn && i >= 5)
            {
                if (playingFieldSlots[i].isOccupied) // Check if the slot is occupied
                {
                    Debug.Log("Slot" + i + " is occupied");
                    // Check if the slot is close enough to drop for Player 2 (first 5 slots)
                    if (Vector2.Distance(position, playingFieldSlots[i].slotTransform.position) < 100f)
                    {
                        PowerSource powerSourceB = powerSources[i]; // Get the second PowerSource
                        string result = FusionMergeController.Instance.GetMergeResult(powerSourceA.powerSourceData.powerSourceName, powerSourceB.powerSourceData.powerSourceName);
                        if (!string.IsNullOrEmpty(result)) // If a valid merge result is found
                        {
                            PlayerManager.player2.activePowerSource = ProcessMerge(powerSourceA, powerSourceB, result); // Merge the PowerSources and set the result as the active PowerSource
                            return true;
                        }
                    }
                }
            }
        }
        return false; // No valid merge found
    }

    public PowerSource ProcessMerge(PowerSource powerSourceA, PowerSource powerSourceB, string result)
    {
        Debug.Log("Starting Merge: " + powerSourceA.powerSourceData.powerSourceName + powerSourceB.powerSourceData.powerSourceName + "=" + result);

        RectTransform transformB = powerSourceB.GetComponent<RectTransform>(); // Get the RectTransform of the PowerSource being dragged onto
        GameObject newFusionObj = Instantiate(SatchelManager.Instance.powerSourcePrefab, transform);

        Debug.Log("Fusion Object Instantiated: " + newFusionObj.name);

        PowerSource fusion = newFusionObj.GetComponent<PowerSource>();
        PowerSourceData data = PowerSourceManager.GetPowerSourceData(result);
        Sprite fusionSprite = PowerSourceManager.GetPowerSourceSprite(result);

        fusion.powerSourceData = new PowerSourceData(); // Create new PowerSourceData object

        // Set PowerSourceData properties
        fusion.powerSourceData.powerSourceName = data.powerSourceName;
        fusion.powerSourceData.attack = data.attack;
        fusion.powerSourceData.defence = data.defence;
        fusion.powerSourceData.health = data.health;

        fusion.powerSourceImage = newFusionObj.GetComponent<Image>();

        fusion.powerSourceImage.sprite = fusionSprite; // Set Fusion sprite

        RectTransform rectTransform = fusion.GetComponent<RectTransform>();
        rectTransform.SetParent(transformB.parent, false); // Set parent to the same as PowerSource B 
        rectTransform.anchoredPosition = transformB.anchoredPosition; // Set position

        // Initialize with required references to canvas and text object
        fusion.Initialize(SatchelManager.Instance.mainCanvas, SatchelManager.Instance.nameText);

        // Remove PowerSource A and B from the playing field and satchel

        int slotIndexA = System.Array.IndexOf(powerSources, powerSourceA);
        int slotIndexB = System.Array.IndexOf(powerSources, powerSourceB);

        if (slotIndexA != -1) // If PowerSource A is found in the playing field
        {
            RemovePowerSourceFromSlot(slotIndexA); // Remove PowerSource A from the slot
        }
        if (slotIndexB != -1) // If PowerSource B is found in the playing field
        {
            RemovePowerSourceFromSlot(slotIndexB); // Remove PowerSource B from the slot
        }

        if (powerSourceA.satchelSlot != null) // If PowerSource A was in a satchel slot
        {
            int satchelSlotIndex = System.Array.IndexOf(SatchelManager.Instance.satchelSlots, powerSourceA.satchelSlot);
            if (satchelSlotIndex != -1) // Ensure that this Power Source originated from the Satchel
            {
                if (TurnManager.isPlayer1Turn)
                {
                    SatchelManager.Instance.player1SatchelPowerSources[satchelSlotIndex] = null; // Remove from Player 1's satchel
                }
                else
                {
                    SatchelManager.Instance.player2SatchelPowerSources[satchelSlotIndex] = null; // Remove from Player 2's satchel
                }
            }
        }
        if (powerSourceB.satchelSlot != null) // If PowerSource B was in a satchel slot
        {
            int satchelSlotIndex = System.Array.IndexOf(SatchelManager.Instance.satchelSlots, powerSourceB.satchelSlot);
            if (satchelSlotIndex != -1) // Ensure that this Power Source originated from the Satchel
            {
                if (TurnManager.isPlayer1Turn)
                {
                    SatchelManager.Instance.player1SatchelPowerSources[satchelSlotIndex] = null; // Remove from Player 1's satchel
                }
                else
                {
                    SatchelManager.Instance.player2SatchelPowerSources[satchelSlotIndex] = null; // Remove from Player 2's satchel
                }
            }
        }

        // Add the Fusion PowerSource to the playing field
        AddPowerSourceToSlot(slotIndexB, fusion); // Add the Fusion to the slot where PowerSource B was
        fusion.satchelSlot = null; // Set the Fusion PowerSource's satchel slot to null
        fusion.playingFieldSlot = playingFieldSlots[slotIndexB]; // Set the Fusion PowerSource's playing field slot to the same as PowerSource B

        Destroy(powerSourceA.gameObject); // Destroy PowerSource A
        Destroy(powerSourceB.gameObject); // Destroy PowerSource B

        return fusion; // Return the new Fusion PowerSource
    }
}

