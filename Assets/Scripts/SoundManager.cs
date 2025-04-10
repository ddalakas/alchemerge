using System.Collections;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    public AudioSource audioSource;
    public AudioClip menuMusic;
    public AudioClip playPhaseMusic;
    public AudioClip combatPhaseMusic;
    public AudioClip victoryMusic;
    public AudioSource sfxSource;
    public AudioClip buttonClickSFX;

    private void Awake()
    {
        if (instance == null) // Singleton pattern to ensure only one instance
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Persist SoundManager between scenes
            DontDestroyOnLoad(audioSource); // Persist music AudioSource between scenes
            DontDestroyOnLoad(sfxSource); // Persist SFX AudioSource between scenes
        }
        else
        {
            Destroy(gameObject);
            Destroy(audioSource); // Destroy duplicate AudioSource
            Destroy(sfxSource); // Destroy duplicate AudioSource
        }
    }

    public void PlayMusic(AudioClip musicClip)
    {
        if (audioSource.clip == musicClip) return; // Prevent restarting same music
        audioSource.clip = musicClip;
        audioSource.Play();
    }

    public void PlaySFX(AudioClip sfxClip)
    {
        sfxSource.PlayOneShot(sfxClip, 0.5f); // Plays without interrupting other sounds
    }

    public IEnumerator FadeOutInMusic(float fadeDuration)
    {
        float initialVolume = audioSource.volume; // Store the initial volume
        for (float t = 0.0f; t < fadeDuration; t += Time.deltaTime)
        {
            audioSource.volume = (1 - t / fadeDuration) * initialVolume; // Smoothly decrease volume to 0
            yield return null; // Wait until the next frame
        }
        audioSource.volume = 0.0f; // Ensure volume is set to 0 exactly
        audioSource.Stop(); // Stop the music
        Debug.Log("Fade Out Complete");

        // Fade in the new music
        Debug.Log("Fading in the new music");
        audioSource.clip = playPhaseMusic;
        audioSource.Play(); // Play the new clip
        StartCoroutine(FadeInMusic(fadeDuration, 0.2f));
    }

    public IEnumerator FadeInMusic(float fadeDuration, float targetVolume)
    {
        Debug.Log("Starting FadeInMusic");
        for (float t = 0.0f; t < fadeDuration; t += Time.deltaTime)
        {
            audioSource.volume = t / fadeDuration * targetVolume; // Smoothly increase volume to target volume
            yield return null; // Wait until the next frame
        }
        audioSource.volume = targetVolume;
        Debug.Log("Finished FadeInMusic");
    }

    public IEnumerator FadeOutInMusicHelper(float fadeDuration, float targetVolume, AudioClip newClip)
    {
        // Fade out the current music and then fade in the new music
        yield return StartCoroutine(FadeOutInMusic(fadeDuration));
    }
}