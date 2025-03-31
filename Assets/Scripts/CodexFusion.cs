using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CodexFusion : MonoBehaviour
{
    // UI elements for recipe section
    public Image powerSourceAIcon;
    public TMP_Text powerSourceAText;
    public Image powerSourceBIcon;
    public TMP_Text powerSourceBText;
    public Image mergeResultIcon;
    public TMP_Text mergeResultText;

    // UI Elements for Power Source
    public TMP_Text powerSourceName;   // power source name

    public Image attackIcon; // attack icon 
    public TMP_Text attackText; // attack value

    public Image defenceIcon; // defence icon
    public TMP_Text defenceText; // defence value

    public Image healthIcon; // health icon
    public TMP_Text healthText; // health value

    public TMP_Text tierText; // tier value

    // method to setup a codex entry
    public void Setup(string powerSourceAName, string powerSourceBName, string mergeResultName, Sprite powerSourceA, Sprite powerSourceB, Sprite mergeResult,
                      PowerSourceData data)
    {
        powerSourceAText.text = powerSourceAName; //  set the text of the Power Source A
        powerSourceBText.text = powerSourceBName; //  set the text of the Power Source B
        mergeResultText.text = mergeResultName; //  set the text of the merge result

        powerSourceName.text = mergeResultName; // set the name of the Power Source (same as merge result name)
        powerSourceAIcon.sprite = powerSourceA; //  set the icon of the Power Source A
        powerSourceBIcon.sprite = powerSourceB; // set the icon of the Power Source B
        mergeResultIcon.sprite = mergeResult; // set the icon of the merge result
        attackText.text = data.attack.ToString(); // set the attack value
        defenceText.text = data.defence.ToString(); // set the defense value
        healthText.text = data.health.ToString(); // set the health value

        if (data.tier == 1) {
            tierText.color = new Color(28f / 255f, 201f / 255f, 143f / 255f, 0.82f); // set the tier 1 color to green
        }
        else if (data.tier == 2) {
            tierText.color = new Color(47f/ 255f, 89f / 255f, 214f / 255f, 0.82f); // set the tier 2 color to blue
        }
        else {
            tierText.color = new Color(201f/ 255f, 80f / 255f, 64f / 255f, 0.82f); // set the tier 3 color to red
        }
        tierText.text = "(T"+data.tier.ToString()+")"; // set the tier value
    }
}



