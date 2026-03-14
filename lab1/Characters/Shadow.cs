namespace RPGGame;

public class Shadow : Character
{
    private bool _trapSet = false;
    private int _trapDamage = 0;
    private readonly double _critChance;
    private static readonly Random _rng = new Random();

    private int _frenzyCharges = 0;
    private const int MaxFrenzyCharges = 3;
    private const int FrenzyDamageBonus = 5;

    public Shadow(string name, IInventory<Item> inventory, double critChance = 0.30)
        : base(name, maxHealth: 100, attackPower: 26, defense: 5, inventory)
    {
        _critChance = critChance;
    }

    public override void PerformAttack(Character target)
    {
        if (_trapSet)
        {
            _trapSet = false;
            Console.WriteLine($"{Name}'s trap triggers on {target.Name}! ({_trapDamage} raw damage)");
            target.TakeRawDamage(_trapDamage);
        }

        bool isCrit = _rng.NextDouble() < _critChance;
        int damage = isCrit ? (int)(AttackPower * 1.6) : AttackPower;
        damage += _frenzyCharges * FrenzyDamageBonus;

        if (isCrit)
        {
            if (_frenzyCharges < MaxFrenzyCharges)
                _frenzyCharges++;
            Console.WriteLine($"{Name} CRITICAL STRIKE on {target.Name}! Frenzy Charge gained! ({_frenzyCharges}/{MaxFrenzyCharges})");
        }
        else
            Console.WriteLine($"{Name} stabs {target.Name} with a dagger!");

        target.TakeDamage(damage);
    }

    public void SetTrap()
    {
        _trapSet = true;
        _trapDamage = AttackPower + 15;
        Console.WriteLine($"{Name} places a trap! ({_trapDamage} damage next turn)");
    }

    public override void TakeTurn(Character target, int turn)
    {
        if (turn == 2 && Inventory.Count > 0)
            UseItem(0, target);

        if (turn % 3 == 0)
            SetTrap();
        else
            PerformAttack(target);

        if (Health < MaxHealth * 0.35)
        {
            int lifeFlaskIdx = Inventory.FindIndex(item => item is LifeFlask);
            if (lifeFlaskIdx >= 0) UseItem(lifeFlaskIdx);
        }
    }

    public override void PrintStatus()
    {
        base.PrintStatus();
        Console.WriteLine($"    Crit chance: {_critChance * 100}% | Frenzy Charges: {_frenzyCharges}/{MaxFrenzyCharges} | Trap ready: {(_trapSet ? "YES" : "NO")}");
    }
}
