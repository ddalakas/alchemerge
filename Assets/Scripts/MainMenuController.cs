using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void StartGame()
    {
        GameManager.instance.ChangeGameState(GameManager.GameState.PlayPhase); // Change the game state to Play Phase
        SceneManager.LoadScene("PlayScene"); // Loads the Play Phase scene
    }
}
