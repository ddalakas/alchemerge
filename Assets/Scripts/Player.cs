// Player Class for storing player stats and image
public class Player
{
    public int attack;
    public int defence;
    public int health;

    public enum element { Earth, Fire, Water, Wind };
    public element baseElement;

    public string spriteName;

    public Player(int attack, int defence, int health)
    {
        this.attack = attack;
        this.defence = defence;
        this.health = health;
    }
}