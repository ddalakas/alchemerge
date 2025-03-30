using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static Player player1 = new(0, 0, 50); // Create Player 1 with 0 attack, 0 defence, and 50 health
    public static Player player2 = new(0, 0, 50); // Create Player 2 with 0 attack, 0 defence, and 50 health

    public static void Player1Attack()
    {
        int damage = player1.attack - player2.defence;
        if (damage > 0)
        {
            if (damage - player2.overhealth > 0)
            {
                player2.baseHealth -= damage - player2.overhealth;
                player2.health = player2.baseHealth;
                player2.overhealth = 0; // Reset overhealth after damage is applied
            }
            else
            {
                player2.health = player2.baseHealth + player2.overhealth - damage;
                player2.overhealth -= damage; // Reduce overhealth by the damage dealt
            }
        }
        if (player2.baseHealth < 0) player2.baseHealth = 0;
    }

    public static void Player2Attack()
    {
        int damage = player2.attack - player1.defence;
        if (damage > 0)
        {
            if (damage - player1.overhealth > 0)
            {
                player1.baseHealth -= damage - player1.overhealth;
                player1.health = player1.baseHealth;
                player1.overhealth = 0; // Reset overhealth after damage is applied
            }
            else
            {
                player1.health = player1.baseHealth + player1.overhealth - damage;
                player1.overhealth -= damage; // Reduce overhealth by the damage dealt
            }
        }
        if (player1.baseHealth < 0) player1.baseHealth = 0;
    }


    public static void CalculatePlayerStats()
    {
        PlayingFieldManager.Instance.SumPowerSourceStats();


    }
}