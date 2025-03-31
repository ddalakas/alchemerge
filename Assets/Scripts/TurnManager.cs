using UnityEngine;
using TMPro;
using System.Collections;
using Unity.VisualScripting;



public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance; // Singleton instance
    public static bool isPlayer1Turn;   // Tracks whose turn it is
    
    [Header("Timer Settings")]
    public float turnDuration = 58f;        // Duration of each turn in seconds
    private float currentTime;          // Remaining time for the current turn
    private bool isPaused = false;      // Whether the timer is paused
    private Coroutine timerCoroutine;   // Reference to the active timer coroutine

    


    private void Awake()
    {
        if (Instance == null){
            Instance = this;
        }
            
        else
            Destroy(gameObject);
    }

    private void Start()
    {   
        isPlayer1Turn = true;          // Player 1 starts first
        DisableOpponentSlots();        // Setup slots for the first turn
    }

    // Switch the turn between players and restart the timer
    public static void SwitchTurn()
    {
        isPlayer1Turn = !isPlayer1Turn;
        DisableOpponentSlots();
    }

    // Disable opponent's slots depending on whose turn it is
    public static void DisableOpponentSlots()
    {
        for (int i = 0; i < PlayingFieldManager.Instance.powerSources.Length; i++)
        {
            if (PlayingFieldManager.Instance.powerSources[i] != null)
            {
                // Enable slots for the active player (first 5 slots for Player 1; others for Player 2)
                if (isPlayer1Turn)
                {
                    Debug.Log("Interactable for slot " + i + ": " + (i < 5));
                    PlayingFieldManager.Instance.powerSources[i].isDraggable = i < 5;
                }
                else
                {
                    Debug.Log("Interactable for slot " + i + ": " + (i >= 5));
                    PlayingFieldManager.Instance.powerSources[i].isDraggable = i >= 5;
                }
            }
        }
    }

    // Start the timer for the turn
    public void StartTurnTimer()
    {
        if (timerCoroutine != null){

            StopCoroutine(timerCoroutine);
        }
           
        currentTime = turnDuration; // Reset the timer to the full duration
        timerCoroutine = StartCoroutine(Countdown());
    }

    // Coroutine to handle the countdown logic
    private IEnumerator Countdown()
    {   
        Color customRed;
        UnityEngine.ColorUtility.TryParseHtmlString("#CO2F00", out customRed); // Define custom red

        while (currentTime > 0)
        {
            if (!isPaused)
            {
                currentTime -= Time.deltaTime;

                // Make text red if less than 10 seconds
                if (currentTime < 10)
                {    
                     UIManager.Instance.timerText.color =  // customRed; // Change text color to red 
                     new Color(0.83f, 0.0f, 0.0f, 1.0f);
                     UIManager.Instance.timerText.text = "0:0" + Mathf.Ceil(currentTime).ToString();;
                }
                else
                {
                    UIManager.Instance.timerText.color = Color.white;
                }
                UIManager.Instance.timerText.text = "0:"+Mathf.Ceil(currentTime).ToString();
            }
            yield return null;
        }
         UIManager.Instance.timerText.text = "0:00"; // Ensure the timer displays 0 when finished
        OnTimerEnd();
    }

    // Called when the timer reaches 0 and the turn ends
    private void OnTimerEnd()
    {
        Debug.Log("Turn time ended. Executing end-of-turn logic.");
       
        //SwitchTurn();
    }

    // Pause the timer when the settings menu is opened, resume when closed
    public void ToggleTimer()
    {
        isPaused = !isPaused; // Toggle the pause state
    }

}
