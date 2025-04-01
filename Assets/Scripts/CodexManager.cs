using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CodexManager : MonoBehaviour
{

    public static CodexManager Instance;

    [Header("Prefabs & UI References")]
    public GameObject codexFusionPrefab; // reference to prefab
    public GameObject codexPrimalPrefab; // reference to prefab
    public Transform codexContentParent;
    public GameObject codexCanvas;

    private string[] primalElements = { "Fire", "Water", "Earth", "Wind" }; // List of primal elements


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

        Debug.Log($"CodexCanvas Instance: {Instance} | Current Object: {gameObject}");

    }
    void Start()
    {
        PopulateCodex();
    }

    void PopulateCodex()
    {
        // Add Primal Elements to the Codex , primal elements don't have recipes
        Debug.Log("Adding Primal Elements to Codex");
        foreach (var primal in primalElements)
        {

            Debug.Log(primal + " added to Codex");
            GameObject primalEntry = Instantiate(codexPrimalPrefab, codexContentParent);

            // Get the CodexEntry component from the prefab
            CodexPrimal codexPrimal = primalEntry.GetComponent<CodexPrimal>();

            if (codexPrimal != null)
            {
                // Get icon for the Primal Element
                Sprite sprite = PowerSourceManager.GetPowerSourceSprite(primal); // get sprite from dictionary

                // Get the stats for the Primal Element
                PowerSourceData primalData = PowerSourceManager.GetPowerSourceData(primal);
                if (primalData == null) Debug.LogError("Result data is null");

                // Setup the Codex entry with text and icon.
                codexPrimal.Setup(primal, sprite, primalData);
            }

        }


        // Loop through every recipe from the list in FusionMergeController
        foreach (var recipe in FusionMergeController.Instance.recipes)
        {
            Debug.Log("Adding " + recipe.powerSourceAName + " + " + recipe.powerSourceBName + " = " + recipe.mergeResultName + " to Codex");
            // Instantiate a new Codex entry prefab
            GameObject fusion = Instantiate(codexFusionPrefab, codexContentParent);

            // Get the CodexEntry component from the prefab
            CodexFusion codexFusion = fusion.GetComponent<CodexFusion>();
            if (codexFusion != null)
            {
                // Get icons for each Power Source
                Sprite spriteA = PowerSourceManager.GetPowerSourceSprite(recipe.powerSourceAName); // get sprite A from dictionary
                Sprite spriteB = PowerSourceManager.GetPowerSourceSprite(recipe.powerSourceBName); // get sprite B from dictionary
                Sprite spriteResult = PowerSourceManager.GetPowerSourceSprite(recipe.mergeResultName); // get sprite for merge result from dictionary

                // Get the stats for the resultant Power Source
                PowerSourceData resultData = PowerSourceManager.GetPowerSourceData(recipe.mergeResultName);
                if (resultData == null) Debug.LogError("Result data is null");

                // Setup the entry with text and icon.
                codexFusion.Setup(recipe.powerSourceAName, recipe.powerSourceBName, recipe.mergeResultName, spriteA, spriteB, spriteResult, resultData);
            }
        }
    }

    public void ToggleCodex()
    {
        codexCanvas.SetActive(!codexCanvas.activeSelf);
    }



}
