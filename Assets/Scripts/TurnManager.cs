using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public TurnManager instance; // Singleton pattern
    public static bool isPlayer1Turn; // Keeps track of whose turn it is

    private void Awake()
    {
        if (instance == null)
            instance = this; // Set the instance to this object
        else
            Destroy(gameObject); // Destroy the object the script is attached to
    }

    void Start()
    {
        isPlayer1Turn = true; // Player1 starts first
    }

    public static void SwitchTurn() // Switches the turn between Player1 and Player2
    {
        // Toggle turn
        isPlayer1Turn = !isPlayer1Turn;
    }
}