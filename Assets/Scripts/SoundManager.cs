using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    public AudioSource audioSource;
    public AudioClip menuMusic;
    public AudioClip playPhaseMusic;
    public AudioClip combatPhaseMusic;
    public AudioClip victoryMusic;

    private void Awake()
    {
        if (instance == null) // Singleton pattern to ensure only one instance
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Persist between scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayMusic(AudioClip musicClip)
    {
        if (audioSource.clip == musicClip) return; // Prevent restarting same music
        audioSource.clip = musicClip;
        audioSource.Play();
    }

    public void StopMusic()
    {
        audioSource.Stop();
    }
}