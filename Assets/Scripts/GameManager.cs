using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum GameState { MainMenu, PlayPhase, CombatPhase, Victory }
    public static GameState currentState;
    public static GameManager instance; // Singleton pattern ensuring only one instance with public access

    private void Awake()
    {
        if (instance == null)
            instance = this; // Set the instance to this object
        else
            Destroy(gameObject); // Destroy the object the script is attached to
    }

    private void Start()
    {
        ChangeGameState(GameState.MainMenu); // Set the initial game state to Main Menu
    }

    public void ChangeGameState(GameState newState)
    {
        currentState = newState;
        SoundManager.instance.audioSource = FindAnyObjectByType<AudioSource>(); // Get the audio source component from the game object
        switch (newState)
        {
            case GameState.MainMenu:
                SoundManager.instance.PlayMusic(SoundManager.instance.menuMusic);
                break;
            case GameState.PlayPhase:
                SoundManager.instance.PlayMusic(SoundManager.instance.playPhaseMusic);
                break;
            case GameState.CombatPhase:
                SoundManager.instance.PlayMusic(SoundManager.instance.combatPhaseMusic);
                break;
            case GameState.Victory:
                SoundManager.instance.PlayMusic(SoundManager.instance.victoryMusic);
                break;
        }
    }
}