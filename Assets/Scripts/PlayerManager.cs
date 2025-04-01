using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static Player player1 = new(0, 0, 50); // Create Player 1 with 0 attack, 0 defence, and 50 health
    public static Player player2 = new(0, 0, 50); // Create Player 2 with 0 attack, 0 defence, and 50 health

    public static void Player1Attack()
    {
        // If damage exceeds combat health, apply damage to overhealth (from Power Sources), and then to base health

        int damage = player1.attack - player2.defence;

        if (damage > 0)
        {
            int damageToOverhealth = damage - player2.combatHealth; // Calculate damage to overhealth

            if (damageToOverhealth > player2.overhealth)
            {
                // If damage exceeds overhealth, apply damage to base health
                player2.baseHealth -= damageToOverhealth - player2.overhealth;
                player2.health = player2.baseHealth;
                player2.overhealth = 0; // Reset overhealth after damage is applied
            }
            else
            {
                player2.overhealth -= damageToOverhealth; // Reduce overhealth by the damage dealt to it
                player2.health = player2.baseHealth + player2.overhealth;
            }
        }
    }

    public static void Player2Attack()
    {
        // If damage exceeds combat health, apply damage to overhealth (from Power Sources), and then to base health

        int damage = player2.attack - player1.defence;

        if (damage > 0)
        {
            int damageToOverhealth = damage - player1.combatHealth; // Calculate damage to overhealth

            if (damageToOverhealth > player1.overhealth)
            {
                // If damage exceeds overhealth, apply damage to base health
                player1.baseHealth -= damageToOverhealth - player1.overhealth;
                player1.health = player1.baseHealth;
                player1.overhealth = 0; // Reset overhealth after damage is applied
            }
            else
            {
                player1.overhealth -= damageToOverhealth; // Reduce overhealth by the damage dealt to it
                player1.health = player1.baseHealth + player1.overhealth;
            }
        }
    }

    public static void CalculatePlayerStats()
    {
        PlayingFieldManager.Instance.SumPowerSourceStats();
    }

    public static void ResetPlayerData()
    {
        player1 = new(0, 0, 50); // Reset Player 1 to default values
        player2 = new(0, 0, 50); // Reset Player 2 to default values
    }

}