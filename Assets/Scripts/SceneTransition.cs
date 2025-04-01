using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SceneTransition : MonoBehaviour
{
    public Image fadeImage;

    public float fadeDuration = 2.0f; // two second fade duration

    private void Start()
    {
        Debug.Log("Fading in scene");
        StartCoroutine(FadeInScreen());
    }

    public void ChangeScene(string sceneName, bool additiveLoad, bool unload)
    {
        if (unload)
        {
            Debug.Log("Unloading scene");
            SceneManager.UnloadSceneAsync(sceneName); // Unload the current scene

            Scene playScene = SceneManager.GetSceneByName("PlayScene"); // Get the Play Scene
            if (playScene.IsValid())
            {
                SceneManager.SetActiveScene(playScene); // Set the active scene to Play Scene
            }

            // Canvas References   
            Canvas playerTurnCanvas = GameObject.Find("Player Turn Canvas").GetComponent<Canvas>();
            Canvas baseElementCanvas = GameObject.Find("Base Element Canvas").GetComponent<Canvas>();
            Canvas playCanvas = GameObject.Find("PlayCanvas").GetComponent<Canvas>();
            Canvas powerSourceSelectionCanvas = GameObject.Find("Power Source Selection Canvas").GetComponent<Canvas>();
            GameObject playGrid = UIManager.Instance.playGrid; // Get the play grid object
            UIManager.Instance.EnablePlayView(playCanvas, playerTurnCanvas, baseElementCanvas, powerSourceSelectionCanvas, playGrid); // Enable the Play View
        }
        else
        {
            if (sceneName == "MainMenuScene")
            {
                CleanUpOnLoadObjects(); // Clean up any objects that should not persist
                PlayerManager.ResetPlayerData(); // Reset player data
                Debug.Log("On Load Objects Cleaned");
            }
            Debug.Log("Changing scene to " + sceneName);
            StartCoroutine(FadeOutLoadNewScreen(sceneName, additiveLoad)); // Start fading out and loading the new scene
        }
    }

    public IEnumerator FadeInScreen()
    {
        for (float t = fadeDuration; t > 0.0f; t -= Time.deltaTime)
        {
            fadeImage.color = new Color(0, 0, 0, t / fadeDuration); // gradually decrease the alpha value of the fade image to 0
            yield return null; // Wait until the next frame
        }
    }

    IEnumerator FadeOutLoadNewScreen(string sceneName, bool additiveLoad)
    {
        for (float t = 0.0f; t < fadeDuration; t += Time.deltaTime)
        {
            fadeImage.color = new Color(0, 0, 0, t / fadeDuration); // gradually increase the alpha value of the fade image to 0
            yield return null; // Wait until the next frame
        }
        fadeImage.color = new Color(0, 0, 0, 1); // Ensure fully opaque
        if (additiveLoad)
        {
            yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive); // Wait for the scene to load with Additive mode
        }
        else
        {
            yield return SceneManager.LoadSceneAsync(sceneName); // Wait for the scene to load normally
        }
        StartCoroutine(FadeInScreen()); // Start fading in new scene
    }

    public void CleanUpOnLoadObjects()
    {
        // Find and destroy all objects with that are'DontDestroyOnLoad'
        string[] persistentObjectNames =
        { "MainMenuManager", "FusionMergeController","SettingsManager","SoundManager", "Audio Source", "SFX Source",
           "GameManager", "CodexManager", "PowerSourceManager",
           "PlayingFieldManager"};

        GameObject persistentObject;
        foreach (string persistentObjectName in persistentObjectNames)
        {
            persistentObject = GameObject.Find(persistentObjectName);
            if (persistentObject != null)
            {
                Debug.Log("Destroying " + persistentObjectName);
                Destroy(persistentObject); // Destroy the object
            }
        }
        Debug.Log("Destryoing Settings and Codex Canvas");
        Destroy(SettingsManager.Instance.settingsCanvas);
        Destroy(CodexManager.Instance.codexCanvas);
    }
}