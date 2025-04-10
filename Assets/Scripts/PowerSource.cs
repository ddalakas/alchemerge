using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

[System.Serializable]
public class PowerSourceData
{
    // PowerSource properties
    public string powerSourceName;
    public int attack;
    public int defence;
    public int health;
    public int tier; // Tier of the PowerSource

}

[System.Serializable]
public class PowerSourceList
{
    public PowerSourceData[] PowerSources;
}

// PowerSource class
public class PowerSource : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    private Canvas canvas; // Canvas which contains the PowerSource
    public TextMeshProUGUI nameText; // Text to display PowerSource name

    private RectTransform rectTransform; // Used to move the PowerSource
    private CanvasGroup canvasGroup; // Used to make the PowerSource semi-transparent during drag
    private Vector2 originalPosition; // Stores original position of the PowerSource
    private Transform originalParent; // Stores original parent of the PowerSource (used to return to original position)

    // PowerSource properties
    public PowerSourceData powerSourceData; // PowerSource data
    public Image powerSourceImage; // PowerSource image

    public SatchelManager.SatchelSlot satchelSlot; // Slot in the satchel where the PowerSource is placed (if any)
    public PlayingFieldManager.PlayingFieldSlot playingFieldSlot; // Slot in the playing field where the PowerSource is placed (if any)

    public bool isDraggable = true; // Determines if the PowerSource can be dragged

    public Player.element elementType; // Type of the PowerSource (Earth, Fire, Water, Wind)

    public bool newlyPlaced; // Indicates if the PowerSource is newly placed

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>(); // Get the RectTransform component of the PowerSource
        canvasGroup = GetComponent<CanvasGroup>();  // Get the CanvasGroup component of the PowerSource
        newlyPlaced = true; // Set newlyPlaced to true by default
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!isDraggable) return; // If not draggable, return

        originalPosition = rectTransform.anchoredPosition;
        originalParent = transform.parent;

        // Make the element semi-transparent and pass through raycasts during drag
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;

        // Move to canvas level for unrestricted dragging
        transform.SetParent(canvas.transform);

        // Make available Playing Field Slots glow
        GlowAvailableSlots();
    }

    private void GlowAvailableSlots()
    {
        PlayingFieldManager playingFieldManager = PlayingFieldManager.Instance;

        for (int i = 0; i < playingFieldManager.playingFieldSlots.Length; i++)
        {
            if (TurnManager.isPlayer1Turn && i < 5) // Check if it's Player 1's turn and the slot is within the first 5 slots
            {
                if (!playingFieldManager.playingFieldSlots[i].isOccupied) // Check if the slot is not occupied
                {
                    ApplySlotGlow(playingFieldManager.playingFieldSlots[i], true);
                }
            }
            else if (!TurnManager.isPlayer1Turn && i >= 5) // Check if it's Player 2's turn and the slot is within the last 5 slots
            {
                if (!playingFieldManager.playingFieldSlots[i].isOccupied)
                {
                    ApplySlotGlow(playingFieldManager.playingFieldSlots[i], true);
                }
            }
        }
    }

    private void ApplySlotGlow(PlayingFieldManager.PlayingFieldSlot slot, bool glow)
    {
        Image slotImage = slot.slotTransform.GetComponent<Image>(); // Get the Image component of the slot

        if (slotImage != null)
        {
            if (glow)
            {
                slotImage.color = new Color(1f, 1f, 0.5f, 0.5f); // Yellowish glow
            }
            else
            {
                slotImage.color = new Color(0.45f, 0.44f, 0.44f, 0.627f); // Reset to normal
            }
        }
    }
    private void ResetSlotGlow()
    {
        PlayingFieldManager playingFieldManager = PlayingFieldManager.Instance;
        for (int i = 0; i < playingFieldManager.playingFieldSlots.Length; i++)
        {
            ApplySlotGlow(playingFieldManager.playingFieldSlots[i], false); // Reset all slots back to normal colour
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDraggable) return; // If not draggable, return
        nameText.text = ""; // Clear description text while dragging
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor; // Move the PowerSource taking into account the canvas scale factor
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isDraggable) return; // If not draggable, return

        ResetSlotGlow(); // Reset slot glow

        // Restore normal appearance and raycast blocking
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        // Place PowerSource in the playing field
        PlayingFieldManager.PlayingFieldSlot newSlot = PlayingFieldManager.Instance.GetValidSlot(transform.position);

        if (newSlot != null)
        {
            // Find the index of the new slot
            int slotIndex = System.Array.IndexOf(PlayingFieldManager.Instance.playingFieldSlots, newSlot);

            transform.SetParent(newSlot.slotTransform);
            rectTransform.anchoredPosition = Vector2.zero; // Center within slot

            if (playingFieldSlot != null) // If previously in a slot
            {
                // Remove from previous slot
                int previousSlotIndex = System.Array.IndexOf(PlayingFieldManager.Instance.playingFieldSlots, playingFieldSlot);
                PlayingFieldManager.Instance.RemovePowerSourceFromSlot(previousSlotIndex);
            }

            // Add to new slot
            PlayingFieldManager.Instance.AddPowerSourceToSlot(slotIndex, this);
            playingFieldSlot = newSlot; // Update reference to new slot

            if (satchelSlot != null) // If PowerSource was in a satchel slot
            {
                int satchelSlotIndex = System.Array.IndexOf(SatchelManager.Instance.satchelSlots, satchelSlot);
                if (satchelSlotIndex != -1) // ensure that this Power Source originated from the Satchel
                {
                    if (TurnManager.isPlayer1Turn)
                    {
                        // Check if Player 2 has a PowerSource in the same slot
                        if (SatchelManager.Instance.player2SatchelPowerSources[satchelSlotIndex] == null) //
                        {
                            SatchelManager.Instance.satchelSlots[satchelSlotIndex].isOccupied = false; // Ensure the slot is marked as unoccupied
                        }
                        SatchelManager.Instance.player1SatchelPowerSources[satchelSlotIndex] = null; // Remove from satchel
                    }
                    else
                    {
                        // Check if Player 1 has a PowerSource in the same slot
                        if (SatchelManager.Instance.player1SatchelPowerSources[satchelSlotIndex] == null) //
                        {
                            SatchelManager.Instance.satchelSlots[satchelSlotIndex].isOccupied = false; // Ensure the slot is marked as unoccupied
                        }
                        SatchelManager.Instance.player2SatchelPowerSources[satchelSlotIndex] = null; // Remove from satchel
                    }
                    satchelSlot = null; // Clear the Power Source's satchel slot reference
                }
                if (TurnManager.isPlayer1Turn)
                {
                    Debug.Log("PowerSource " + powerSourceData.powerSourceName + " became Player 1's Active PowerSource");
                    PlayerManager.player1.activePowerSource = this; // Set the active PowerSource for Player 1
                }
                else
                {
                    Debug.Log("PowerSource " + powerSourceData.powerSourceName + " became Player 2's Active PowerSource");
                    PlayerManager.player2.activePowerSource = this; // Set the active PowerSource for Player 2
                }
            }
            Debug.Log("PowerSource was placed in the playing field at slot + " + slotIndex);
        }
        else
        {
            Debug.Log("No open slot found.");
            bool mergeFound = PlayingFieldManager.Instance.GetValidMerge(transform.position, gameObject.GetComponent<PowerSource>()); // Check for fusions
            if (!mergeFound)
            {
                // Return to original position if no valid slot
                transform.SetParent(originalParent);
                rectTransform.anchoredPosition = originalPosition;
            }
        }

        // Calculate Player Stats
        PlayerManager.CalculatePlayerStats();

        // Update the current player's stats on the screen
        if (TurnManager.isPlayer1Turn)
        {
            UIManager.Instance.UpdateStats(PlayerManager.player1.attack,
                PlayerManager.player1.defence, PlayerManager.player1.health);
        }
        else
        {
            UIManager.Instance.UpdateStats(PlayerManager.player2.attack,
                PlayerManager.player2.defence, PlayerManager.player2.health);
        }

    }
    public void OnPointerClick(PointerEventData eventData)
    {
        // When clicked, display PowerSource name above the PowerSource
        if (nameText != null)
        {
            nameText.transform.position = new Vector2(transform.position.x, transform.position.y + 120); // Position text above PowerSource
            nameText.text = powerSourceData.powerSourceName;
        }
    }

    // Initialize with canvas and text reference
    public void Initialize(Canvas canvas, TextMeshProUGUI nameText)
    {
        this.canvas = canvas;
        this.nameText = nameText;
    }
}