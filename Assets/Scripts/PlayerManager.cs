// Purpose: Manages the player's stats and uses the UIManager to update the player's stats on the screen
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static Player player1 = new(100, 20, 50); // Create Player 1 with 0 attack, 0 defence, and 50 health
    public static Player player2 = new(50, 20, 50); // Create Player 2 with 0 attack, 0 defence, and 50 health

    public static void Player1Attack()
    {
        if (player1.attack > player2.defence)
        {
            player2.health -= player1.attack - player2.defence;
        }
        if (player2.health < 0) player2.health = 0;
    }

    public static void Player2Attack()
    {
        if (player2.attack > player1.defence)
        {
            player1.health -= player2.attack - player1.defence;
        }
        if (player1.health < 0) player1.health = 0;
    }


    public static void CalculatePlayerStats()
    {
        PlayingFieldManager.Instance.SumPowerSourceStats();


    }
}