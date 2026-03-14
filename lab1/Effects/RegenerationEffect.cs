namespace RPGGame;

public class RegenerationEffect : IEffect
{
    public string Name => "Regeneration";
    public bool IsExpired => _remainingTurns <= 0;

    private readonly int _healPerTurn;
    private int _remainingTurns;

    public RegenerationEffect(int healPerTurn, int duration)
    {
        _healPerTurn = healPerTurn;
        _remainingTurns = duration;
    }

    public void Tick(Character target)
    {
        _remainingTurns--;
        Console.WriteLine($"  [REGENERATION] {target.Name} restores {_healPerTurn} HP. ({_remainingTurns} turns left)");
        target.Heal(_healPerTurn);
    }
}
