using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;
using System;
using Unity.VisualScripting;

public class CombatHUDManager : MonoBehaviour
{
    [Header("Player 1 HUD")] // Combat HUD for Player 1

    public TextMeshProUGUI player1AttackText;
    public TextMeshProUGUI player1DefenceText;
    public TextMeshProUGUI player1HealthText;
    public Image player1HUDSprite;
    public Image player1ActivePowerSourceSprite;

    [Header("Player 2 HUD")] // Combat HUD for Player 2

    public TextMeshProUGUI player2AttackText;
    public TextMeshProUGUI player2DefenceText;
    public TextMeshProUGUI player2HealthText;
    public Image player2HUDSprite;
    public Image player2ActivePowerSourceSprite;

    readonly float combatMultiplier = 1.1f; // Combat multiplier for buffs

    int playerThatWon = 0; // Stores number of the player that won (zero if no player won)

    private void Start()
    {
        player1HUDSprite.sprite = PlayerSpriteManager.GetPlayerSprite(PlayerManager.player1.spriteName + "_1"); // Set Player 1 HUD sprite
        player2HUDSprite.sprite = PlayerSpriteManager.GetPlayerSprite(PlayerManager.player2.spriteName + "_1"); // Set Player 2 HUD sprite

        if (PlayerManager.player1.activePowerSource != null)
        {
            player1ActivePowerSourceSprite.sprite = PowerSourceManager.GetPowerSourceSprite(PlayerManager.player1.activePowerSource.powerSourceData.powerSourceName); // Set Player 1 active power source sprite
        }
        if (PlayerManager.player2.activePowerSource != null)
        {
            player2ActivePowerSourceSprite.sprite = PowerSourceManager.GetPowerSourceSprite(PlayerManager.player2.activePowerSource.powerSourceData.powerSourceName); // Set Player 2 active power source sprite
        }

        SetPowerSourcesNotNewlyPlaced(); // Set all power sources to not newly placed

        CalculateCombatBuffs(); // Calculate combat buffs based on power sources
        Color greenColor = new Color(60 / 255f, 201 / 255f, 32 / 255f, 0.85f); // Define green color
        if (PlayerManager.player1.gotBuffed) { player1AttackText.color = greenColor; player1DefenceText.color = greenColor; player1HealthText.color = greenColor; } // Set Player 1 HUD text color to green if buffed
        else if (PlayerManager.player2.gotBuffed) { player2AttackText.color = greenColor; player2DefenceText.color = greenColor; player2HealthText.color = greenColor; } // Set Player 2 HUD text color to green if buffed

        UpdatePlayer1Stats(); // Initialize Player 1 HUD
        UpdatePlayer2Stats(); // Initialize Player 2 HUD

        // Player 1 Attack
        PlayerManager.Player1Attack(); // Player 1 attacks
        StartCoroutine(UpdatePlayer2HUDAfterAnimation()); // Update Player 2 HUD after animation

        // Player 2 Attack
        PlayerManager.Player2Attack(); // Player 2 attacks
        StartCoroutine(UpdatePlayer1HUDAfterAnimation()); // update Player 1 HUD after animation

        StartCoroutine(PlayAndVictoryTransitionWait()); // Wait before transitioning to play phase screen or victory screen
    }

    public void SetPowerSourcesNotNewlyPlaced()
    {
        foreach (PowerSource powerSource in PlayingFieldManager.Instance.powerSources)
        {
            if (powerSource != null)
            {
                powerSource.newlyPlaced = false; // Set newly placed status to false for all power sources
            }
        }
    }

    public IEnumerator UpdatePlayer1HUDAfterAnimation()
    {
        yield return new WaitForSeconds(7f); // Wait for 7 seconds before updating HUD
        UpdatePlayer1Stats(); // Update Player 1 HUD after the attack
    }

    public IEnumerator UpdatePlayer2HUDAfterAnimation()
    {

        yield return new WaitForSeconds(7f); // Wait for 7 seconds before updating HUD
        UpdatePlayer2Stats(); // Update Player 2 HUD after the attack
    }

    public void CalculateCombatBuffs()
    {
        if (PlayerManager.player1.activePowerSource == null && PlayerManager.player2.activePowerSource != null)
        {
            PlayerManager.player2.attack = (int)Math.Round(PlayerManager.player2.attack * combatMultiplier);
            PlayerManager.player2.defence = (int)Math.Round(PlayerManager.player2.defence * combatMultiplier);
            PlayerManager.player2.combatHealth = (int)Math.Round(PlayerManager.player2.health * (combatMultiplier - 1));
            PlayerManager.player2.health += PlayerManager.player2.combatHealth; // Add combatHealth to Player 2's health
            PlayerManager.player2.gotBuffed = true; // Set buffed status to true for Player 2
        }
        else if (PlayerManager.player2.activePowerSource == null && PlayerManager.player1.activePowerSource != null)
        {
            PlayerManager.player1.attack = (int)Math.Round(PlayerManager.player1.attack * combatMultiplier);
            PlayerManager.player1.defence = (int)Math.Round(PlayerManager.player1.defence * combatMultiplier);
            PlayerManager.player1.combatHealth = (int)Math.Round(PlayerManager.player1.health * (combatMultiplier - 1));
            PlayerManager.player1.health += PlayerManager.player1.combatHealth; // Add combatHealth to Player 2's health
            PlayerManager.player1.gotBuffed = true; // Set buffed status to true for Player 2
        }

        else if (PlayerManager.player1.activePowerSource.elementType == Player.element.Fire
         && PlayerManager.player2.activePowerSource.elementType == Player.element.Water) // water beats fire
        {

            PlayerManager.player2.attack = (int)Math.Round(PlayerManager.player2.attack * combatMultiplier);
            PlayerManager.player2.defence = (int)Math.Round(PlayerManager.player2.defence * combatMultiplier);
            PlayerManager.player2.combatHealth = (int)Math.Round(PlayerManager.player2.health * (combatMultiplier - 1));
            PlayerManager.player2.health += PlayerManager.player2.combatHealth; // Add combatHealth to Player 2's health
            PlayerManager.player2.gotBuffed = true; // Set buffed status to true for Player 2

            Debug.Log("Player 2 got buffed for " + PlayerManager.player2.activePowerSource.elementType);
        }
        else if (PlayerManager.player1.activePowerSource.elementType == Player.element.Water
         && PlayerManager.player2.activePowerSource.elementType == Player.element.Fire) // water beats fire
        {
            PlayerManager.player1.attack = (int)Math.Round(PlayerManager.player1.attack * combatMultiplier);
            PlayerManager.player1.defence = (int)Math.Round(PlayerManager.player1.defence * combatMultiplier);
            PlayerManager.player1.combatHealth = (int)Math.Round(PlayerManager.player1.health * (combatMultiplier - 1));
            PlayerManager.player1.health += PlayerManager.player1.combatHealth; // Add combatHealth to Player 1's health
            PlayerManager.player1.gotBuffed = true; // Set buffed status to true for Player 1

            Debug.Log("Player 1 got buffed for " + PlayerManager.player1.activePowerSource.elementType);
        }
        else if (PlayerManager.player1.activePowerSource.elementType == Player.element.Earth
         && PlayerManager.player2.activePowerSource.elementType == Player.element.Wind)  // wind beats earth
        {
            PlayerManager.player2.attack = (int)Math.Round(PlayerManager.player2.attack * combatMultiplier);
            PlayerManager.player2.defence = (int)Math.Round(PlayerManager.player2.defence * combatMultiplier);
            PlayerManager.player2.combatHealth = (int)Math.Round(PlayerManager.player2.health * (combatMultiplier - 1));
            PlayerManager.player2.health += PlayerManager.player2.combatHealth; // Add combatHealth to Player 2's health
            PlayerManager.player2.gotBuffed = true; // Set buffed status to true for Player 2

            Debug.Log("Player 2 got buffed for " + PlayerManager.player2.activePowerSource.elementType);
        }
        else if (PlayerManager.player1.activePowerSource.elementType == Player.element.Wind
         && PlayerManager.player2.activePowerSource.elementType == Player.element.Earth) // wind beats earth
        {
            PlayerManager.player1.attack = (int)Math.Round(PlayerManager.player1.attack * combatMultiplier);
            PlayerManager.player1.defence = (int)Math.Round(PlayerManager.player1.defence * combatMultiplier);
            PlayerManager.player1.combatHealth = (int)Math.Round(PlayerManager.player1.health * (combatMultiplier - 1));
            PlayerManager.player1.health += PlayerManager.player1.combatHealth; // Add combatHealth to Player 1's health
            PlayerManager.player1.gotBuffed = true; // Set buffed status to true for Player 1

            Debug.Log("Player 1 got buffed for " + PlayerManager.player1.activePowerSource.elementType);
        }
        else if (PlayerManager.player1.activePowerSource.elementType == Player.element.Wind
         && PlayerManager.player2.activePowerSource.elementType == Player.element.Fire) // fire beats wind
        {
            PlayerManager.player2.attack = (int)Math.Round(PlayerManager.player2.attack * combatMultiplier);
            PlayerManager.player2.defence = (int)Math.Round(PlayerManager.player2.defence * combatMultiplier);
            PlayerManager.player2.combatHealth = (int)Math.Round(PlayerManager.player2.health * (combatMultiplier - 1));
            PlayerManager.player2.health += PlayerManager.player2.combatHealth; // Add combatHealth to Player 2's health
            PlayerManager.player2.gotBuffed = true; // Set buffed status to true for Player 2

            Debug.Log("Player 2 got buffed for " + PlayerManager.player2.activePowerSource.elementType);
        }
        else if (PlayerManager.player1.activePowerSource.elementType == Player.element.Fire
         && PlayerManager.player2.activePowerSource.elementType == Player.element.Wind) // fire beats wind
        {
            PlayerManager.player1.attack = (int)Math.Round(PlayerManager.player1.attack * combatMultiplier);
            PlayerManager.player1.defence = (int)Math.Round(PlayerManager.player1.defence * combatMultiplier);
            PlayerManager.player1.combatHealth = (int)Math.Round(PlayerManager.player1.health * (combatMultiplier - 1));
            PlayerManager.player1.health += PlayerManager.player1.combatHealth; // Add combatHealth to Player 1's health
            PlayerManager.player1.gotBuffed = true; // Set buffed status to true for Player 1

            Debug.Log("Player 1 got buffed for " + PlayerManager.player1.activePowerSource.elementType);
        }
        else if (PlayerManager.player1.activePowerSource.elementType == Player.element.Water
         && PlayerManager.player2.activePowerSource.elementType == Player.element.Earth) // earth beats water
        {
            PlayerManager.player2.attack = (int)Math.Round(PlayerManager.player2.attack * combatMultiplier);
            PlayerManager.player2.defence = (int)Math.Round(PlayerManager.player2.defence * combatMultiplier);
            PlayerManager.player2.combatHealth = (int)Math.Round(PlayerManager.player2.health * (combatMultiplier - 1));
            PlayerManager.player2.health += PlayerManager.player2.combatHealth; // Add combatHealth to Player 2's health
            PlayerManager.player2.gotBuffed = true; // Set buffed status to true for Player 2

            Debug.Log("Player 2 got buffed for " + PlayerManager.player2.activePowerSource.elementType);
        }
        else if (PlayerManager.player1.activePowerSource.elementType == Player.element.Earth
         && PlayerManager.player2.activePowerSource.elementType == Player.element.Water) // earth beats water
        {
            PlayerManager.player1.attack = (int)Math.Round(PlayerManager.player1.attack * combatMultiplier);
            PlayerManager.player1.defence = (int)Math.Round(PlayerManager.player1.defence * combatMultiplier);
            PlayerManager.player1.combatHealth = (int)Math.Round(PlayerManager.player1.health * (combatMultiplier - 1));
            PlayerManager.player1.health += PlayerManager.player1.combatHealth; // Add combatHealth to Player 1's health
            PlayerManager.player1.gotBuffed = true; // Set buffed status to true for Player 1

            Debug.Log("Player 1 got buffed for " + PlayerManager.player1.activePowerSource.elementType);
        }
    }
    public void UndoCombatBuffs()
    {
        if (PlayerManager.player1.gotBuffed == true && PlayerManager.player2.gotBuffed == false) // Player 1 got buffed
        {
            PlayerManager.player1.attack = (int)Math.Round(PlayerManager.player1.attack / combatMultiplier);
            PlayerManager.player1.defence = (int)Math.Round(PlayerManager.player1.defence / combatMultiplier);
            PlayerManager.player1.health = PlayerManager.player1.baseHealth + PlayerManager.player1.overhealth; // Remove combatHealth from Player 1's health
        }
        else if (PlayerManager.player2.gotBuffed == true && PlayerManager.player1.gotBuffed == false) // Player 2 got buffed
        {
            PlayerManager.player2.attack = (int)Math.Round(PlayerManager.player2.attack / combatMultiplier);
            PlayerManager.player2.defence = (int)Math.Round(PlayerManager.player2.defence / combatMultiplier);
            PlayerManager.player2.health = PlayerManager.player2.baseHealth + PlayerManager.player2.overhealth; // Remove combatHealth from Player 1's health
        }
        PlayerManager.player1.gotBuffed = false; // Reset buffed status for Player 1
        PlayerManager.player2.gotBuffed = false; // Reset buffed status for Player 2
    }

    public void UpdatePlayer1Stats()
    {
        player1AttackText.text = PlayerManager.player1.attack.ToString(); // Update Player 1 attack text
        player1DefenceText.text = PlayerManager.player1.defence.ToString(); // Update Player 1 defence text
        player1HealthText.text = PlayerManager.player1.health.ToString(); // Update Player 1 health text
    }
    public void UpdatePlayer2Stats()
    {
        player2AttackText.text = PlayerManager.player2.attack.ToString(); // Update Player 2 attack text
        player2DefenceText.text = PlayerManager.player2.defence.ToString(); // Update Player 2 defence text
        player2HealthText.text = PlayerManager.player2.health.ToString(); // Update Player 2 health text
    }

    public bool CheckWinner()
    {
        if (PlayerManager.player1.health <= 0 && PlayerManager.player2.health > PlayerManager.player1.health)
        {
            // Player 1 is defeated
            Debug.Log("Player 2 wins!");
            playerThatWon = 2; // Set player that won to Player 2
            return true;
        }
        else if (PlayerManager.player1.health <= 0 && PlayerManager.player1.health > PlayerManager.player2.health)
        {
            // Player 2 is defeated
            Debug.Log("Player 1 Wins!");
            playerThatWon = 1; // Set player that won to Player 1
            return true;
        }
        else if (PlayerManager.player2.health <= 0 && PlayerManager.player1.health > PlayerManager.player2.health)
        {
            // Player 2 is defeated
            Debug.Log("Player 1 Wins!");
            playerThatWon = 1; // Set player that won to Player 1
            return true;
        }
        else if (PlayerManager.player2.health <= 0 && PlayerManager.player2.health > PlayerManager.player1.health)
        {
            // Player 1 is defeated
            Debug.Log("Player 2 wins!");
            playerThatWon = 2; // Set player that won to Player 2
            return true;
        }
        else if (PlayerManager.player1.health <= 0 && (PlayerManager.player1.health == PlayerManager.player2.health))
        {
            // Player 1 wins they have more (negative) health
            Debug.Log("Player 1 wins!");
            playerThatWon = 1; // Set player that won to Player 1
            return true;
        }
        return false; // No winner yet
    }

    IEnumerator PlayAndVictoryTransitionWait()
    {
        yield return new WaitForSeconds(15f); // Wait for 14 seconds

        if (playerThatWon != 0) // Check if a player has won
        {
            Debug.LogError("Player has won, transitioning to victory screen");
            VictoryManager.Instance.DisplayWinner(playerThatWon); // Show the winner

            // Unload the Play Scene
            Debug.LogWarning("Unloading Play Scene");

            Scene playScene = SceneManager.GetSceneByName("PlayScene"); // Get the Play Scene
            if (playScene.IsValid())
            {
                SceneManager.UnloadSceneAsync(playScene); // Unload the Play Scene
            }

            playerThatWon = 0; // Reset player that won
        }
        else
        {
            Debug.Log("No winner yet, returning to play phase");
            UndoCombatBuffs(); // Undo combat buffs after the attack
            BackToPlayPhase(); // Return to play phase
        }
    }

    public void BackToPlayPhase()
    {
        GameManager.instance.ChangeGameState(GameManager.GameState.PlayPhase); // Change the game state to Play Phase
        SceneTransition sceneTransition = FindAnyObjectByType<SceneTransition>(); // Find the SceneTransition component
        sceneTransition.ChangeScene("CombatScene", false, true); // Fade in the Play Scene screen with no additive load
    }
}
