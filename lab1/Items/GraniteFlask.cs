namespace RPGGame;

public class GraniteFlask : Item
{
    private readonly int _defenseBonus;

    public GraniteFlask(int defenseBonus = 14)
        : base("Granite Flask", $"Permanently increases armor by {defenseBonus}")
    {
        _defenseBonus = defenseBonus;
    }

    public override void Apply(Character target)
    {
        Console.WriteLine($"  Armor +{_defenseBonus}!");
        target.ModifyDefense(_defenseBonus);
    }
}
