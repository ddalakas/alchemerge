using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance; // Proper Singleton pattern
    public static bool isPlayer1Turn; // Keeps track of whose turn it is

    private void Awake()
    {
        if (Instance == null)
            Instance = this; // Set the instance to this object
        else
            Destroy(gameObject); // Destroy duplicate instances
    }

    void Start()
    {
        isPlayer1Turn = true; // Player1 starts first
        DisableOpponentSlots(); // Ensure correct slots are disabled at start
    }
    // Switch the turn between Player 1 and Player 2
    public static void SwitchTurn()
    {
        isPlayer1Turn = !isPlayer1Turn;
        DisableOpponentSlots();
    }
    // Disable the slots that belong to the opponent during the current player's turn
    public static void DisableOpponentSlots()
    {
        for (int i = 0; i < PlayingFieldManager.Instance.powerSources.Length; i++)
        {
            if (PlayingFieldManager.Instance.powerSources[i] != null)
            {
                CanvasGroup canvasGroup = PlayingFieldManager.Instance.powerSources[i].GetComponent<CanvasGroup>();
                if (canvasGroup != null)
                {
                    if (isPlayer1Turn)
                    {
                        Debug.Log("Interactable for slot" + i + ":" + (i < 5));
                        PlayingFieldManager.Instance.powerSources[i].isDraggable = i < 5; // Enable Player 1's PowerSources
                    }
                    else
                    {
                        Debug.Log("Interactable for slot" + i + ":" + (i >= 5));
                        PlayingFieldManager.Instance.powerSources[i].isDraggable = i >= 5; // Enable Player 2's PowerSources
                    }
                }
            }
        }
    }
}
