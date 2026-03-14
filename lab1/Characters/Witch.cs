namespace RPGGame;

public class Witch : Character
{
    public int Mana { get; private set; }
    public int MaxMana { get; private set; }
    public int EnergyShield { get; private set; }
    public int MaxEnergyShield { get; private set; }
    private const int SpellCost = 20;

    public Witch(string name, IInventory<Item> inventory)
        : base(name, maxHealth: 80, attackPower: 36, defense: 2, inventory)
    {
        MaxMana = 100;
        Mana = 100;
        MaxEnergyShield = 35;
        EnergyShield = 35;
    }

    public override void PerformAttack(Character target)
    {
        if (Mana >= SpellCost)
        {
            Mana -= SpellCost;
            Console.WriteLine($"{Name} casts Fireball at {target.Name}! (Mana: {Mana}/{MaxMana})");
            target.TakeRawDamage(AttackPower);
        }
        else
        {
            Console.WriteLine($"{Name} is out of mana! Hits with a staff.");
            target.TakeDamage(AttackPower / 4);
        }
    }

    public override void TakeDamage(int rawDamage)
    {
        if (EnergyShield > 0)
        {
            int absorbed = Math.Min(EnergyShield, rawDamage);
            EnergyShield -= absorbed;
            rawDamage -= absorbed;
            Console.WriteLine($"  [ENERGY SHIELD] Absorbed {absorbed} damage. ES: {EnergyShield}/{MaxEnergyShield}");
        }
        base.TakeDamage(rawDamage);
    }

    public void RestoreMana(int amount)
    {
        Mana = Math.Min(MaxMana, Mana + amount);
        Console.WriteLine($"{Name} meditates. Mana: {Mana}/{MaxMana}");
    }

    public override void TakeTurn(Character target, int turn)
    {
        if (turn == 1 && Inventory.Count > 0)
            UseItem(0);
        else if (turn % 4 == 0)
            RestoreMana(40);
        else
        {
            PerformAttack(target);
            if (Health < MaxHealth * 0.3 && Inventory.Count > 0)
                UseItem(0);
        }
    }

    public override void PrintStatus()
    {
        base.PrintStatus();
        Console.WriteLine($"    Mana: {Mana}/{MaxMana} | Energy Shield: {EnergyShield}/{MaxEnergyShield}");
    }
}
