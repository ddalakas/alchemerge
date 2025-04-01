using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum GameState { MainMenu, PlayPhase, CombatPhase, Victory }
    public static GameState currentState;
    public static GameManager instance; // Singleton pattern ensuring only one instance with public access

    private void Awake()
    {
        if (instance == null)
        {
            instance = this; // Set the instance to this object
            DontDestroyOnLoad(gameObject); // Don't destroy this object when loading new scenes
        }
        else
            Destroy(gameObject); // Destroy the object the script is attached to
    }

    private void Start()
    {
        ChangeGameState(GameState.MainMenu); // Set the initial game state to Main Menu
    }

    public void ChangeGameState(GameState newState)
    {
        currentState = newState; // Update the current game state
        switch (newState)
        {
            case GameState.MainMenu:
                StartCoroutine(SoundManager.instance.FadeInMusic(3.5f, 0.1f)); // Fade in music over 3.5 seconds to 10% volume
                SoundManager.instance.PlayMusic(SoundManager.instance.menuMusic);
                break;
            case GameState.PlayPhase:
                if (!ToggleSound.isMusicMuted) // Only fade out and in music when not muted
                {
                    // Fade out music over 5 seconds and then fade in new music at 20% volume
                    StartCoroutine(SoundManager.instance.FadeOutInMusicHelper(5.0f, 0.2f, SoundManager.instance.playPhaseMusic));
                }
                break;
            case GameState.CombatPhase:
                SoundManager.instance.PlayMusic(SoundManager.instance.combatPhaseMusic);
                break;
            case GameState.Victory:
                SoundManager.instance.PlaySFX(SoundManager.instance.victoryMusic);
                break;
        }
    }
}