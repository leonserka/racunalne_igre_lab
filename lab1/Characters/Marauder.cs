namespace RPGGame;

public class Marauder : Character
{
    private int _charges = 0;
    private const int MaxCharges = 3;
    private const int ReductionPerCharge = 6;

    public Marauder(string name, IInventory<Item> inventory)
        : base(name, maxHealth: 160, attackPower: 22, defense: 10, inventory)
    {
    }

    public override void PerformAttack(Character target)
    {
        Console.WriteLine($"{Name} smashes {target.Name} with a war hammer!");
        target.TakeDamage(AttackPower);
    }

    public override void TakeDamage(int rawDamage)
    {
        if (_charges > 0)
        {
            int reduction = _charges * ReductionPerCharge;
            rawDamage = Math.Max(0, rawDamage - reduction);
            Console.WriteLine($"  [ENDURANCE x{_charges}] Damage reduced by {reduction}.");
        }
        base.TakeDamage(rawDamage);
    }

    public void GainCharge()
    {
        if (_charges < MaxCharges)
        {
            _charges++;
            Console.WriteLine($"{Name} gains an Endurance Charge! ({_charges}/{MaxCharges})");
        }
    }

    public void FuryLeap(Character target)
    {
        int bonusDamage = _charges * 10;
        _charges = 0;
        Console.WriteLine($"{Name} performs FURY LEAP on {target.Name}! (bonus: +{bonusDamage} damage)");
        target.TakeDamage(AttackPower + bonusDamage);
    }

    public override void TakeTurn(Character target, int turn)
    {
        GainCharge();

        if (turn == 2 && Inventory.Count > 1)
            UseItem(1);

        if (turn % 4 == 0)
            FuryLeap(target);
        else
            PerformAttack(target);

        if (Health < MaxHealth * 0.4 && Inventory.Count > 0)
            UseItem(0);
    }

    public override void PrintStatus()
    {
        base.PrintStatus();
        Console.WriteLine($"    Endurance Charges: {_charges}/{MaxCharges}");
    }
}
