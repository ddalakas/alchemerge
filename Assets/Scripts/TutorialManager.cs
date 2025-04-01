using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public Image tutorialImage;  
    public Sprite[] tutorialSprites;  // Array of tutorial images shown in order
    private int currentIndex = 0;

    public GameObject tutorialCanvas; 

    void Start()
    {
        if (tutorialSprites.Length > 0)
        {
            tutorialImage.sprite = tutorialSprites[currentIndex];
        }


    }

    public void ShowNextImage()
    {
        if (currentIndex < tutorialSprites.Length - 1)
        {
            currentIndex++;
            tutorialImage.sprite = tutorialSprites[currentIndex];
        }

        else {
            tutorialCanvas.SetActive(false); // Hide the canvas when the last image is reached
        }
    }

    public void ToggleTutorialCanvas(){
         tutorialCanvas.SetActive(!tutorialCanvas.activeSelf);

         // reset index to show the first image again
            tutorialImage.sprite = tutorialSprites[0];
            currentIndex = 0;
    }
}
