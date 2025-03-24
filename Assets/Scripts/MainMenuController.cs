using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    bool clicked = false; // Boolean to check if the player has clicked the Play button
    public void StartGame() // Function to start the game by changing to the Play Phase
    {
        if (!clicked) // If the player has not clicked the Play button
        {
            {
                GameManager.instance.ChangeGameState(GameManager.GameState.PlayPhase); // Change the game state to Play Phase
                SceneTransition sceneTransition = FindAnyObjectByType<SceneTransition>(); // Find the SceneTransition component
                sceneTransition.ChangeScene("PlayScene"); // Change the scene to Play Scene
            }
        }
        clicked = true; // Set clicked to true
    }
}
