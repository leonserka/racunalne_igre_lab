namespace RPGGame;

public class RegenerationFlask : Item
{
    private readonly int _healPerTurn;
    private readonly int _duration;

    public RegenerationFlask(int healPerTurn = 18, int duration = 3)
        : base("Regeneration Flask", $"Restores {healPerTurn} HP/turn for {duration} turns")
    {
        _healPerTurn = healPerTurn;
        _duration = duration;
    }

    public override void Apply(Character target)
    {
        target.AddEffect(new RegenerationEffect(_healPerTurn, _duration));
    }
}
