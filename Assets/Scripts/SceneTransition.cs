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

    public void ChangeScene(string sceneName)
    {
        Debug.Log("Changing scene to " + sceneName);
        StartCoroutine(FadeOutLoadNewScreen(sceneName));
    }

    public IEnumerator FadeInScreen()
    {
        for (float t = fadeDuration; t > 0.0f; t -= Time.deltaTime)
        {
            fadeImage.color = new Color(0, 0, 0, t / fadeDuration); // gradually decrease the alpha value of the fade image to 0
            yield return null; // Wait until the next frame
        }
    }

    IEnumerator FadeOutLoadNewScreen(string sceneName)
    {
        for (float t = 0.0f; t < fadeDuration; t += Time.deltaTime)
        {
            fadeImage.color = new Color(0, 0, 0, t / fadeDuration); // gradually increase the alpha value of the fade image to 0
            yield return null; // Wait until the next frame
        }
        fadeImage.color = new Color(0, 0, 0, 1); // Ensure fully opaque
        yield return SceneManager.LoadSceneAsync(sceneName); // Wait for the scene to load
        StartCoroutine(FadeInScreen()); // Start fading in new scene
    }
}