using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void StartGame()
    {
        GameManager.instance.ChangeGameState(GameManager.GameState.PlayPhase); // Change the game state to Play Phase
        SceneTransition sceneTransition = FindAnyObjectByType<SceneTransition>(); // Find the SceneTransition component
        sceneTransition.ChangeScene("PlayScene"); // Change the scene to Play Scene
    }
}
