using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CodexManager : MonoBehaviour
{   

     public static CodexManager Instance;

    [Header("Prefabs & UI References")]
    public GameObject codexEntryPrefab; // reference to prefab
    public Transform codexContentParent;


    public GameObject codexCanvas;

    
    void Awake()
    {
        if (Instance == null)   // Singleton pattern
        {
            Instance = this;
            DontDestroyOnLoad(codexCanvas); // Keep Codex canvas across scenes
        }
        else
        {
            Destroy(codexCanvas); // Prevent duplicates
        }
    }
    void Start()
    {
        PopulateCodex();
    }

    void PopulateCodex()
    {

        // Loop through every recipe from the list in FusionMergeController
        foreach (var recipe in FusionMergeController.Instance.recipes)
        {
            // Instantiate a new Codex entry prefab
            GameObject entry = Instantiate(codexEntryPrefab, codexContentParent);

            // Get the CodexEntry component from the prefab
            CodexEntry codexEntry = entry.GetComponent<CodexEntry>();
            if (codexEntry != null)
            {
                // Get icons for each Power Source
                //Sprite spriteA = // dictionary gets sprite A
                //Sprite spriteB = // dictionary gets sprite B
                //Sprite spriteResult = // dictionary gets result sprite

                // Get stats of each Power Source from existing array in PowerSourceManager
                // PowerSourceManager.Instance.GetPowerSource(recipe.powerSourceAName);

                // Setup the entry with text and icon.
                //codexEntry.Setup(recipe.powerSourceAName, recipe.powerSourceBName, recipe.mergeResultName, spriteA, spriteB, spriteResult);
            }
        }
    }

    public void ToggleCodex()
    {
        codexCanvas.SetActive(!codexCanvas.activeSelf);
    }

    

}
