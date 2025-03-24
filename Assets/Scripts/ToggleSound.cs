using UnityEngine;
using UnityEngine.UI;

public class ToggleSound : MonoBehaviour
{
    public Image speakerImage; // Reference to the speaker image
    public Sprite musicOnSprite; // Sprite for sound on
    public Sprite musicOffSprite; // Sprite for sound off
    public static bool isMusicMuted = false; // Track the mute state

    public TMPro.TextMeshProUGUI sfxText; // Reference to the SFX text
    public static bool isSFXMuted = false; // Track the mute state

    public void ToggleMusic()
    {
        isMusicMuted = !isMusicMuted; // Toggle state
        speakerImage.sprite = isMusicMuted ? musicOffSprite : musicOnSprite; // Change sprite based on state

        // Pause or unpause the music
        if (isMusicMuted)
        {
            SoundManager.instance.audioSource.Pause();
        }
        else
        {
            SoundManager.instance.audioSource.UnPause();
        }

        //SoundManager.instance.audioSource.volume = isMusicMuted ? 0 : 0.1f; // Set volume to 0 if muted, 0.1 if not
    }
    public void ToggleSFX()
    {
        isSFXMuted = !isSFXMuted; // Toggle state
        sfxText.text = isSFXMuted ? "SFX: OFF" : "SFX: ON";
        SoundManager.instance.sfxSource.volume = isSFXMuted ? 0 : 0.5f; // Set volume to 0 if muted, 0.5 if not
    }
}
