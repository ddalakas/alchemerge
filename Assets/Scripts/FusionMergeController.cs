using System.Collections.Generic;
using UnityEngine;

public class MergeRecipe
{
    public string inputA; // first input of recipe
    public string inputB; // second input of recipe
    public string result; // result of merge

    public MergeRecipe(string a, string b, string res)
    {
        inputA = a;
        inputB = b;
        result = res;
    }
}

public class FusionMergeController : MonoBehaviour
{
    public TextAsset csvFile; // external CSV containing recipes
    public List<MergeRecipe> recipes = new List<MergeRecipe>();

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
                string a = parts[0].Trim();
                string b = parts[1].Trim();
                string result = parts[2].Trim();
                recipes.Add(new MergeRecipe(a, b, result));
            }
        }
    }

    public string GetMergeResult(string elementA, string elementB)
    {
        foreach (var recipe in recipes)
        {
            // check in either order
            if ((recipe.inputA == elementA && recipe.inputB == elementB) ||
                (recipe.inputA == elementB && recipe.inputB == elementA))
            {
                return recipe.result;
            }
        }
        return null; // not found
    }
}
