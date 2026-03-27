namespace RPGGame;

public class PoisonFlask : Item
{
    private readonly int _damagePerTurn;
    private readonly int _duration;

    public PoisonFlask(int damagePerTurn = 9, int duration = 3)
        : base("Poison Flask", $"{damagePerTurn} poison damage/turn for {duration} turns")
    {
        _damagePerTurn = damagePerTurn;
        _duration = duration;
    }

    public override void Apply(Character target)
    {
        target.AddEffect(new PoisonEffect(_damagePerTurn, _duration));
    }
}
