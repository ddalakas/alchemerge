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
            DontDestroyOnLoad(gameObject); // Keep CodexManager across scenes

        }
        else
        {
            Destroy(gameObject); // Prevent duplicates
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
            Debug.Log(recipe.powerSourceAName + " + " + recipe.powerSourceBName + " = " + recipe.mergeResultName);
            GameObject entry = Instantiate(codexEntryPrefab, codexContentParent);

            // Get the CodexEntry component from the prefab
            CodexEntry codexEntry = entry.GetComponent<CodexEntry>();
            if (codexEntry != null)
            {
                // Get icons for each Power Source
                Sprite spriteA = PowerSourceManager.GetPowerSourceSprite(recipe.powerSourceAName); // get sprite A from dictionary
                Sprite spriteB = PowerSourceManager.GetPowerSourceSprite(recipe.powerSourceBName); // get sprite B from dictionary
                Sprite spriteResult = PowerSourceManager.GetPowerSourceSprite(recipe.mergeResultName); // get sprite for merge result from dictionary

                // Get the stats for the resultant Power Source
                PowerSourceData resultData = PowerSourceManager.GetPowerSourceData(recipe.mergeResultName);
                if (resultData == null) Debug.LogError("Result data is null");

                // Setup the entry with text and icon.
                codexEntry.Setup(recipe.powerSourceAName, recipe.powerSourceBName, recipe.mergeResultName, spriteA, spriteB, spriteResult, resultData);
            }
        }
    }

    public void ToggleCodex()
    {
        codexCanvas.SetActive(!codexCanvas.activeSelf);
    }



}
