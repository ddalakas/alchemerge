using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CodexEntry : MonoBehaviour
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
    }
}



