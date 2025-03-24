using UnityEngine;

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
    }

    // Remove a PowerSource from a specific slot
    public void RemovePowerSourceFromSlot(int slotIndex)
    {
        if (slotIndex >= 0 && slotIndex < powerSources.Length)
        {
            powerSources[slotIndex] = null;
            playingFieldSlots[slotIndex].isOccupied = false;
        }
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
        if (TurnManager.isPlayer1Turn)
        {
            int player1Attack = 0;
            int player1Defence = 0;
            int player1Health = 0;

            for (int i = 0; i < 5; i++)
            {
                if (powerSources[i] != null)
                {
                    player1Attack += powerSources[i].powerSourceData.attack;
                    player1Defence += powerSources[i].powerSourceData.defence;
                    player1Health += powerSources[i].powerSourceData.health;
                }
            }
            PlayerManager.player1.attack = player1Attack;
            PlayerManager.player1.defence = player1Defence;
            PlayerManager.player1.health += player1Health;

        }
        else
        {
            int player2Attack = 0;
            int player2Defence = 0;
            int player2Health = 0;

            for (int i = 5; i < 10; i++)
            {
                if (powerSources[i] != null)
                {

                    player2Attack += powerSources[i].powerSourceData.attack;
                    player2Defence += powerSources[i].powerSourceData.defence;
                    player2Health += powerSources[i].powerSourceData.health;
                }
            }
            PlayerManager.player2.attack = player2Attack;
            PlayerManager.player2.defence = player2Defence;
            PlayerManager.player2.health += player2Health;

        }
    }
}
