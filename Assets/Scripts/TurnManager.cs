using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;



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
        if (Instance == null)
        {
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
        if (timerCoroutine != null)
        {

            StopCoroutine(timerCoroutine);
        }

        currentTime = turnDuration; // Reset the timer to the full duration
        timerCoroutine = StartCoroutine(Countdown());
    }

    // Coroutine to handle the countdown logic
    private IEnumerator Countdown()
    {
        while (currentTime > 0)
        {
            if (!isPaused)
            {
                currentTime -= Time.deltaTime;

                int minutes = Mathf.FloorToInt(currentTime / 60); // Get minutes digit
                int seconds = Mathf.CeilToInt(currentTime % 60); // Get seconds digits

                // Fix for the "0:60" issue
                if (seconds == 60)
                {
                    minutes += 1;
                    seconds = 0;
                }

                // Ensure two zeros for seconds
                string formattedTime = minutes + ":" + seconds.ToString("00");

                // Change color when under 10 seconds
                if (currentTime < 10)
                {
                    UIManager.Instance.timerText.color = new Color(0.83f, 0.0f, 0.0f, 1.0f); // Custom red colour
                }
                else
                {
                    UIManager.Instance.timerText.color = Color.white;
                }

                UIManager.Instance.timerText.text = formattedTime;
            }
            yield return null;
        }

        UIManager.Instance.timerText.text = "0:00"; // Ensure it displays 0:00 when finished
        OnTimerEnd();
    }

    // Called when the timer reaches 0 and the turn ends
    private void OnTimerEnd()
    {
        Debug.Log("Turn time ended. Executing end-of-turn logic.");
        SceneTransition sceneTransition = FindAnyObjectByType<SceneTransition>(); // Find the SceneTransition component

        // Canvas References
        Canvas playerTurnCanvas = GameObject.Find("Player Turn Canvas").GetComponent<Canvas>();
        Canvas baseElementCanvas = GameObject.Find("Base Element Canvas").GetComponent<Canvas>();
        Canvas playCanvas = GameObject.Find("PlayCanvas").GetComponent<Canvas>();
        Canvas powerSourceSelectionCanvas = GameObject.Find("Power Source Selection Canvas").GetComponent<Canvas>();

        GameObject playGrid = GameObject.Find("PlayGrid"); // Find the Play Grid object


        if (!(PlayerManager.player1.activePowerSource == null && PlayerManager.player2.activePowerSource == null))
        {
            StartCoroutine(UIManager.Instance.DisableAndUpdatePlayViewWithDelay(2f, playCanvas, playerTurnCanvas, baseElementCanvas, powerSourceSelectionCanvas, playGrid)); // Disable the Play View after 2 seconds
            sceneTransition.ChangeScene("CombatScene", true, false); // Change the scene to Combat Phase with additive load

            Scene combatScene = SceneManager.GetSceneByName("CombatScene");
            if (combatScene.IsValid())
            {
                SceneManager.SetActiveScene(combatScene); // Set the active scene to Combat Scene
            }
            GameManager.instance.ChangeGameState(GameManager.GameState.CombatPhase); // Change the game state to Combat Phase
        }
        else
        {
            UIManager.Instance.UpdatePlayerView(playCanvas, playerTurnCanvas); // Update the playing field view
        }
    }

    // Pause the timer when the settings menu is opened, resume when closed
    public void ToggleTimer()
    {
        isPaused = !isPaused; // Toggle the pause state
    }
}
