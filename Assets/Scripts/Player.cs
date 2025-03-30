public class Player
{
    public int attack; // Player's attack stat
    public int defence; // Player's defence stat
    public int health; // Player's health stat

    public int baseHealth; // Player's base health stat
    public int overhealth = 0; // Player's overhealth stat

    public enum element { Earth, Fire, Water, Wind };
    public element baseElement;
    public string spriteName;

    public PowerSource activePowerSource; // Active PowerSource
    public bool gotBuffed = false; // Buffed stat

    public string[] unlockedPowerSources = new string[37]; // Array of unlocked PowerSource names

    public Player(int attack, int defence, int baseHealth)
    {
        this.attack = attack;
        this.defence = defence;
        this.baseHealth = baseHealth;
        health = baseHealth; // Initialize health to base health

        // Players start with all four Primal elements unlocked
        unlockedPowerSources[0] = "Earth";
        unlockedPowerSources[1] = "Fire";
        unlockedPowerSources[2] = "Water";
        unlockedPowerSources[3] = "Wind";
    }
}