namespace RPGGame;

public class LifeFlask : Item
{
    private readonly int _healAmount;

    public LifeFlask(int healAmount = 55)
        : base("Life Flask", $"Instantly restores {healAmount} HP")
    {
        _healAmount = healAmount;
    }

    public override void Apply(Character target)
    {
        target.Heal(_healAmount);
    }
}
