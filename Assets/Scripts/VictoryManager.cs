using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor.SearchService;

public class VictoryManager : MonoBehaviour
{

    public static VictoryManager Instance; // Singleton instance of the VictoryManager
    public TMP_Text winnerText; // Assign in inspector
    public Image winnerImage; 
    public GameObject victoryCanvas; // Reference to the victory canvas

    private void Awake()
    {
        // Check if an instance of VictoryManager already exists
        if (Instance == null)
        {
            Instance = this; // Set the instance to this object
            DontDestroyOnLoad(gameObject); // Don't destroy this object when loading new scenes
        }
        else
        {
            Destroy(gameObject); // Destroy this object if another instance already exists
        }
    }
    private void Start()
    {
        // Hide the victory canvas at the start
        if (victoryCanvas != null)
        {
            victoryCanvas.SetActive(false);
        }
        else
        {
            Debug.LogError("Victory Canvas is not assigned in the inspector!");
        }
    }


    public void DisplayWinner(int playerNumber) {

        if (playerNumber == 1)
        {
            winnerText.text = "Player 1 Wins!";
            if (PlayerManager.player1.spriteName != null) winnerImage.sprite = PlayerSpriteManager.GetPlayerSprite(PlayerManager.player1.spriteName);
        }
        else if (playerNumber == 2)
        {
            winnerText.text = "Player 2 Wins!";
            if (PlayerManager.player2.spriteName != null) winnerImage.sprite = PlayerSpriteManager.GetPlayerSprite(PlayerManager.player1.spriteName);
        }
    
        //GameManager.instance.ChangeGameState(GameManager.GameState.Victory); // Change the game state to Victory
        victoryCanvas.SetActive(true); // Show the victory screen
        SceneTransition sceneTransition = FindAnyObjectByType<SceneTransition>(); // Find the SceneTransition component
        StartCoroutine(sceneTransition.FadeInScreen()); // Fade in the screen
    }

    public void LoadMainMenu()
    {   
        SceneTransition sceneTransition = FindAnyObjectByType<SceneTransition>(); // Find the SceneTransition component
        sceneTransition.ChangeScene("MainMenuScene"); // Change the scene to Main Menu Scene
        GameManager.instance.ChangeGameState(GameManager.GameState.MainMenu); // Change the game state to Main Menu
    }
}