using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class CombatHUDManager : MonoBehaviour
{
    public static CombatHUDManager Instance; // Singleton instance of the CombatHUDManager


    [Header("Player 1 HUD")] // Combat HUD for Player 1

    public TextMeshProUGUI player1AttackText;
    public TextMeshProUGUI player1DefenceText;
    public TextMeshProUGUI player1HealthText;
    public Image player1Sprite;

    [Header("Player 2 HUD")] // Combat HUD for Player 2

    public TextMeshProUGUI player2AttackText;
    public TextMeshProUGUI player2DefenceText;
    public TextMeshProUGUI player2HealthText;
    public Image player2Sprite;

    float combatMultiplier = 1.5f; // Combat multiplier for buffs
    bool playergotBuffed = true; // Flag to check if buffs are applied

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // Ensure only one instance exists
        }
    }

    private void Start()
    {
        CalculateCombatBuffs(); // Calculate combat buffs based on power sources

        UpdatePlayer1HUD(); // Initialize Player 1 HUD
        UpdatePlayer2HUD(); // Initialize Player 2 HUD

        // Player 1 Attack
        PlayerManager.Player1Attack(); // Player 1 attacks
        StartCoroutine(UpdatePlayer2HUDAfterAnimation()); // Update Player 2 HUD after animation

        // Player 2 Attack
        PlayerManager.Player2Attack(); // Player 2 attacks
        StartCoroutine(UpdatePlayer1HUDAfterAnimation()); // update Player 1 HUD after animation

        UndoCombatBuffs(); // Undo combat buffs after the attack
    }

    public IEnumerator UpdatePlayer1HUDAfterAnimation() {
        yield return new WaitForSeconds(7f); // Wait for 7 seconds before updating HUD

        if (CheckWinner()) // Check for winner after attacks and then transition to victory screen
        {
            PlayerManager.player1.health = 0; // Set Player 1 health to 0
            PlayerManager.player1.attack = 0; // Set Player 1 attack to 0
            PlayerManager.player1.defence = 0; // Set Player 1 defence to 0

            StartCoroutine(DeathAnimationDelay()); // Check for winner after attacks and then transition to victory screen
        }
        UpdatePlayer1HUD(); // Update Player 1 HUD after the attack
    }

     public IEnumerator UpdatePlayer2HUDAfterAnimation() {

        yield return new WaitForSeconds(7f); // Wait for 7 seconds before updating HUD

        if (CheckWinner()) // Check for winner after attacks and then transition to victory screen
        {
            PlayerManager.player2.health = 0; // Set Player 2 health to 0
            PlayerManager.player2.attack = 0; // Set Player 2 attack to 0
            PlayerManager.player2.defence = 0; // Set Player 2 defence to 0

            StartCoroutine(DeathAnimationDelay()); // Check for winner after attacks and then transition to victory screen
        }

        UpdatePlayer2HUD(); // Update Player 2 HUD after the attack
    }

    public IEnumerator DeathAnimationDelay() {

        yield return new WaitForSeconds(3f); // Wait for 3 seconds before updating HUD
        if (PlayerManager.player2.health <= 0) // Check if Player 2 is dead
        {
              VictoryManager.Instance.DisplayWinner(1); // Show Player 1 as the winner
        }
        else if (PlayerManager.player1.health <= 0) // Check if Player 1 is dead
        {
            VictoryManager.Instance.DisplayWinner(2); // Show Player 2 as the winner
        }
    }

    public void CalculateCombatBuffs(){
        if (PlayerManager.player1.activePowerSource == null || PlayerManager.player2.activePowerSource == null) return; // Check if active power sources are null

        if (PlayerManager.player1.activePowerSource.powerSourceData.powerSourceName == "Fire"
         && PlayerManager.player2.activePowerSource.powerSourceData.powerSourceName == "Water") // water beats fire
        {
            PlayerManager.player2.attack = (int)(PlayerManager.player2.attack * combatMultiplier);
            PlayerManager.player2.defence = (int)(PlayerManager.player2.defence * combatMultiplier);
            PlayerManager.player2.overhealth = (int)(PlayerManager.player2.health * combatMultiplier);
            PlayerManager.player2.health += PlayerManager.player2.overhealth; // Add overhealth to Player 2's health
            PlayerManager.player2.gotBuffed = true; // Set buffed status to true for Player 2
        }
        else if (PlayerManager.player1.activePowerSource.powerSourceData.powerSourceName == "Water"
         && PlayerManager.player2.activePowerSource.powerSourceData.powerSourceName == "Fire") // water beats fire
        {
            PlayerManager.player1.attack = (int)(PlayerManager.player1.attack * combatMultiplier);
            PlayerManager.player1.defence = (int)(PlayerManager.player1.defence * combatMultiplier);
            PlayerManager.player1.overhealth = (int)(PlayerManager.player1.health * combatMultiplier);
            PlayerManager.player1.health += PlayerManager.player1.overhealth; // Add overhealth to Player 1's health
            PlayerManager.player1.gotBuffed = true; // Set buffed status to true for Player 1
        }
        else if (PlayerManager.player1.activePowerSource.powerSourceData.powerSourceName == "Earth"
         && PlayerManager.player2.activePowerSource.powerSourceData.powerSourceName == "Wind")  // wind beats earth
        {
            PlayerManager.player2.attack = (int)(PlayerManager.player2.attack * combatMultiplier);
            PlayerManager.player2.defence = (int)(PlayerManager.player2.defence * combatMultiplier);
            PlayerManager.player2.overhealth = (int)(PlayerManager.player2.health * combatMultiplier);
            PlayerManager.player2.health += PlayerManager.player2.overhealth; // Add overhealth to Player 2's health
            PlayerManager.player2.gotBuffed = true; // Set buffed status to true for Player 2
        }
        else if (PlayerManager.player1.activePowerSource.powerSourceData.powerSourceName == "Wind"
         && PlayerManager.player2.activePowerSource.powerSourceData.powerSourceName == "Earth") // wind beats earth
        {
            PlayerManager.player1.attack = (int)(PlayerManager.player1.attack * combatMultiplier);
            PlayerManager.player1.defence = (int)(PlayerManager.player1.defence * combatMultiplier);
            PlayerManager.player1.overhealth = (int)(PlayerManager.player2.health * combatMultiplier);
            PlayerManager.player1.health += PlayerManager.player1.overhealth; // Add overhealth to Player 1's health
            PlayerManager.player1.gotBuffed = true; // Set buffed status to true for Player 1
        }
        else if (PlayerManager.player1.activePowerSource.powerSourceData.powerSourceName == "Wind"
         && PlayerManager.player2.activePowerSource.powerSourceData.powerSourceName == "Fire") // fire beats wind
        {
            PlayerManager.player2.attack = (int)(PlayerManager.player2.attack * combatMultiplier);
            PlayerManager.player2.defence = (int)(PlayerManager.player2.defence * combatMultiplier);
            PlayerManager.player2.overhealth = (int)(PlayerManager.player2.health * combatMultiplier);
            PlayerManager.player2.health += PlayerManager.player2.overhealth; // Add overhealth to Player 2's health
            PlayerManager.player2.gotBuffed = true; // Set buffed status to true for Player 2
        }
        else if (PlayerManager.player1.activePowerSource.powerSourceData.powerSourceName == "Fire"
         && PlayerManager.player2.activePowerSource.powerSourceData.powerSourceName == "Wind") // fire beats wind
        {
            PlayerManager.player1.attack = (int)(PlayerManager.player1.attack * combatMultiplier);
            PlayerManager.player1.defence = (int)(PlayerManager.player1.defence * combatMultiplier);
            PlayerManager.player1.overhealth = (int)(PlayerManager.player1.health * combatMultiplier);
            PlayerManager.player1.health += PlayerManager.player1.overhealth; // Add overhealth to Player 1's health
            PlayerManager.player1.gotBuffed = true; // Set buffed status to true for Player 1
        }
         else if (PlayerManager.player1.activePowerSource.powerSourceData.powerSourceName == "Water"
         && PlayerManager.player2.activePowerSource.powerSourceData.powerSourceName == "Earth") // earth beats water
        {
            PlayerManager.player2.attack = (int)(PlayerManager.player2.attack * combatMultiplier);
            PlayerManager.player2.defence = (int)(PlayerManager.player2.defence * combatMultiplier);
            PlayerManager.player2.overhealth = (int)(PlayerManager.player2.health * combatMultiplier);
            PlayerManager.player2.health += PlayerManager.player2.overhealth; // Add overhealth to Player 2's health
            PlayerManager.player2.gotBuffed = true; // Set buffed status to true for Player 2
        }
        else if (PlayerManager.player1.activePowerSource.powerSourceData.powerSourceName == "Earth"
         && PlayerManager.player2.activePowerSource.powerSourceData.powerSourceName == "Water") // earth beats water
        {
            PlayerManager.player1.attack = (int)(PlayerManager.player1.attack * combatMultiplier);
            PlayerManager.player1.defence = (int)(PlayerManager.player1.defence * combatMultiplier);
            PlayerManager.player1.overhealth = (int)(PlayerManager.player1.health * combatMultiplier);
            PlayerManager.player1.health += PlayerManager.player1.overhealth; // Add overhealth to Player 1's health
            PlayerManager.player1.gotBuffed = true; // Set buffed status to true for Player 1
        }
        else {
        }
    }

    public void UndoCombatBuffs(){
        if (PlayerManager.player1.gotBuffed == true && PlayerManager.player2.gotBuffed == false) // Player 1 got buffed
        {
            PlayerManager.player1.attack = (int)(PlayerManager.player1.attack / combatMultiplier);
            PlayerManager.player1.defence = (int)(PlayerManager.player1.defence / combatMultiplier);
            PlayerManager.player1.health = PlayerManager.player1.health - PlayerManager.player1.overhealth; // Remove overhealth from Player 1's health
            PlayerManager.player1.overhealth = 0; // Reset overhealth to 0
        }
        else if (PlayerManager.player2.gotBuffed == true && PlayerManager.player1.gotBuffed == false) // Player 2 got buffed
        {
            PlayerManager.player2.attack = (int)(PlayerManager.player2.attack / combatMultiplier);
            PlayerManager.player2.defence = (int)(PlayerManager.player2.defence / combatMultiplier);
            PlayerManager.player2.health = PlayerManager.player2.health - PlayerManager.player2.overhealth; // Remove overhealth from Player 2's health
            PlayerManager.player2.overhealth = 0; // Reset overhealth to 0
        }
    }

    public void UpdatePlayer1HUD()
    {
        player1AttackText.text = PlayerManager.player1.attack.ToString(); // Update Player 1 attack text
        player1DefenceText.text = PlayerManager.player1.defence.ToString(); // Update Player 1 defence text
        player1HealthText.text = PlayerManager.player1.health.ToString(); // Update Player 1 health text
        if (PlayerManager.player1.spriteName != null) {player1Sprite.sprite = PlayerSpriteManager.GetPlayerSprite(PlayerManager.player1.spriteName);} // Update Player 1 sprite
    }
    public void UpdatePlayer2HUD()
    {
        player2AttackText.text = PlayerManager.player2.attack.ToString(); // Update Player 2 attack text
        player2DefenceText.text = PlayerManager.player2.defence.ToString(); // Update Player 2 defence text
        player2HealthText.text = PlayerManager.player2.health.ToString(); // Update Player 2 health text
        if (PlayerManager.player2.spriteName != null) {player2Sprite.sprite = PlayerSpriteManager.GetPlayerSprite(PlayerManager.player2.spriteName); }// Update Player 2 sprite
    }

    public bool CheckWinner()
    {
        if (PlayerManager.player1.health <= 0)
        {
            // Player 1 is defeated
            Debug.Log("Player 1 is defeated!");
            return true; // Player 2 wins
        }
        else if (PlayerManager.player2.health <= 0)
        {
            // Player 2 is defeated
            Debug.Log("Player 2 is defeated!");
            return true; // Player 1 wins
        }
        return false; // No winner yet
    }
}
