using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public static bool isPlayer1Turn = true; // Keeps track of whose turn it is

    void Start()
    {

    }

    void Update()
    {
        // Check if mouse is clicked
        if (Input.GetMouseButtonDown(0)) // Left mouse button click
        {
            SwitchTurn();
        }
    }

    void SwitchTurn() // Switches the turn between Player1 and Player2
    {
        // Toggle turn
        isPlayer1Turn = !isPlayer1Turn;
    }
}