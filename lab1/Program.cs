using RPGGame;

Console.WriteLine("||     PATH OF EXILE - ARENA BATTLE    ||");

var marauder = new Marauder("Kaom",     new Inventory<Item>());
var witch     = new Witch("Shavronne", new Inventory<Item>());
var shadow    = new Shadow("Kira",     new Inventory<Item>());

marauder.Inventory.Add(new LifeFlask(60));
marauder.Inventory.Add(new GraniteFlask(14));

witch.Inventory.Add(new RegenerationFlask(20, 3));
witch.Inventory.Add(new LifeFlask(40));

shadow.Inventory.Add(new PoisonFlask(9, 3));
shadow.Inventory.Add(new LifeFlask(35));

Console.WriteLine("=== INITIAL STATE ===");
marauder.PrintStatus();
marauder.Inventory.PrintInventory();
Console.WriteLine();
witch.PrintStatus();
witch.Inventory.PrintInventory();
Console.WriteLine();
shadow.PrintStatus();
shadow.Inventory.PrintInventory();

Console.WriteLine("||     BATTLE 1: Marauder vs Witch      ||");
SimulateBattle(marauder, witch);

var marauder2 = new Marauder("Kaom II", new Inventory<Item>());
var shadow2   = new Shadow("Kira II",   new Inventory<Item>());

marauder2.Inventory.Add(new LifeFlask(60));
shadow2.Inventory.Add(new PoisonFlask(9, 3));
shadow2.Inventory.Add(new LifeFlask(35));

Console.WriteLine("||     BATTLE 2: Shadow vs Marauder     ||");
SimulateBattle(shadow2, marauder2);

static void SimulateBattle(Character a, Character b, int maxTurns = 8)
{
    Console.WriteLine($"\n{a.Name} [{a.GetType().Name}] vs {b.Name} [{b.GetType().Name}]");
    Console.WriteLine(new string('─', 45));

    for (int turn = 1; turn <= maxTurns; turn++)
    {
        if (!a.IsAlive || !b.IsAlive) break;

        Console.WriteLine($"\n┌── Turn {turn} ────────────────────────────");

        a.ProcessEffects();
        b.ProcessEffects();

        if (!a.IsAlive || !b.IsAlive) break;

        Console.WriteLine($"\n>> {a.Name} acts:");
        a.TakeTurn(b, turn);

        if (!b.IsAlive) break;

        Console.WriteLine($"\n>> {b.Name} acts:");
        b.TakeTurn(a, turn);

        Console.WriteLine($"\n└── State after turn {turn}:");
        a.PrintStatus();
        b.PrintStatus();
    }

    Console.WriteLine("\n=========================================");
    if (!a.IsAlive)
        Console.WriteLine($"  WINNER: {b.Name}!");
    else if (!b.IsAlive)
        Console.WriteLine($"  WINNER: {a.Name}!");
    else
        Console.WriteLine("  Battle ends in a draw.");
    Console.WriteLine("=========================================");
}
