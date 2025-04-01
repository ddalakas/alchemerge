public class Player
{
    public int attack; // Player's attack stat
    public int defence; // Player's defence stat
    public int health; // Player's health stat

    public int baseHealth; // Player's base health stat
    public int overhealth = 0; // Player's overhealth stat

    public int combatHealth; // Player's health is gained from a buff during combat

    public enum element { Earth, Fire, Water, Wind };
    public element baseElement;
    public string spriteName;
    public PowerSource activePowerSource; // Active PowerSource
    public bool gotBuffed = false; // Buffed stat

    public Player(int attack, int defence, int baseHealth)
    {
        this.attack = attack;
        this.defence = defence;
        this.baseHealth = baseHealth;
        health = baseHealth; // Initialize health to base health
    }
}