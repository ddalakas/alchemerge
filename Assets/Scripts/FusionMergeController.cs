using System.Collections.Generic;
using UnityEngine;

public class MergeRecipe
{
    public string powerSourceAName; // first input of recipe
    public string powerSourceBName; // second input of recipe
    public string mergeResultName; // result of merge
    public MergeRecipe(string a, string b, string res)
    {
        powerSourceAName = a;
        powerSourceBName = b;
        mergeResultName = res;
    }
}

public class FusionMergeController : MonoBehaviour
{
    public static FusionMergeController Instance; // Singleton pattern
    public TextAsset csvFile; // external CSV containing recipes
    public List<MergeRecipe> recipes = new List<MergeRecipe>();
    

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        LoadRecipes();
        Debug.Log("Recipes loaded: " + recipes.Count);
    }

    void LoadRecipes()
    {
        string[] lines = csvFile.text.Split(new char[] { '\n' }); // get individual recipes from CSV

        for (int i = 1; i < lines.Length; i++)  // skip the CSV header
        {
            string[] parts = lines[i].Split(',');
            if (parts.Length == 3)
            {
                string powerSourceA = parts[0].Trim();
                string powerSourceB = parts[1].Trim();
                string mergeResult = parts[2].Trim();
                recipes.Add(new MergeRecipe(powerSourceA, powerSourceB, mergeResult));
            }
        }
    }

    public string GetMergeResult(string powerSourceA, string powerSourceB)
    {
        foreach (var recipe in recipes)
        {
            // check in either order
            if ((recipe.powerSourceAName == powerSourceA && recipe.powerSourceBName == powerSourceB) ||
                (recipe.powerSourceAName == powerSourceA && recipe.powerSourceBName == powerSourceB))
            {
                return recipe.mergeResultName;
            }
        }
        return null; // not found
    }
}
