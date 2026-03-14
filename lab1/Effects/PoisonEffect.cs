namespace RPGGame;

public class PoisonEffect : IEffect
{
    public string Name => "Poison";
    public bool IsExpired => _remainingTurns <= 0;

    private readonly int _damagePerTurn;
    private int _remainingTurns;

    public PoisonEffect(int damagePerTurn, int duration)
    {
        _damagePerTurn = damagePerTurn;
        _remainingTurns = duration;
    }

    public void Tick(Character target)
    {
        _remainingTurns--;
        Console.WriteLine($"  [POISON] {target.Name} takes {_damagePerTurn} poison damage. ({_remainingTurns} turns left)");
        target.TakeRawDamage(_damagePerTurn);
    }
}
