using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CodexPrimal : MonoBehaviour{


    // UI Elements for Power Source
    public TMP_Text powerSourceName;   // power source name
    public Image powerSourceIcon; 

    public Image attackIcon; // attack icon 
    public TMP_Text attackText; // attack value

    public Image defenceIcon; // defence icon
    public TMP_Text defenceText; // defence value

    public Image healthIcon; // health icon
    public TMP_Text healthText; // health value

    // method to setup a codex entry
    public void Setup(string powerSourceName, Sprite powerSourceIcon, PowerSourceData data)
    {
        this.powerSourceName.text = powerSourceName; // set the name of the Power Source (same as merge result name)
        this.powerSourceIcon.sprite = powerSourceIcon; // set the icon of the Power Source A
        attackText.text = data.attack.ToString(); // set the attack value
        defenceText.text = data.defence.ToString(); // set the defense value
        healthText.text = data.health.ToString(); // set the health value
    }
}
 



