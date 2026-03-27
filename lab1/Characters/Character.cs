namespace RPGGame;

public abstract class Character
{
    public string Name { get; protected set; }
    public int Health { get; protected set; }
    public int MaxHealth { get; protected set; }
    public int AttackPower { get; protected set; }
    public int Defense { get; protected set; }
    public bool IsAlive => Health > 0;

    public IInventory<Item> Inventory { get; private set; }
    private List<IEffect> _activeEffects = new List<IEffect>();

    protected Character(string name, int maxHealth, int attackPower, int defense, IInventory<Item> inventory)
    {
        Name = name;
        MaxHealth = maxHealth;
        Health = maxHealth;
        AttackPower = attackPower;
        Defense = defense;
        Inventory = inventory;
    }

    public abstract void PerformAttack(Character target);
    public abstract void TakeTurn(Character target, int turn);

    public virtual void TakeDamage(int rawDamage)
    {
        int blocked = Math.Min(rawDamage, Defense);
        int actual = rawDamage - blocked;
        Health = Math.Max(0, Health - actual);
        Console.WriteLine($"  {Name} takes {actual} damage (blocked {blocked} by armor). HP: {Health}/{MaxHealth}");
    }

    public void TakeRawDamage(int damage)
    {
        Health = Math.Max(0, Health - damage);
        Console.WriteLine($"  {Name} takes {damage} raw damage. HP: {Health}/{MaxHealth}");
    }

    public void Heal(int amount)
    {
        int actual = Math.Min(amount, MaxHealth - Health);
        Health += actual;
        Console.WriteLine($"  {Name} restores {actual} HP. HP: {Health}/{MaxHealth}");
    }

    public void ModifyAttack(int amount) => AttackPower += amount;
    public void ModifyDefense(int amount) => Defense += amount;

    public void AddEffect(IEffect effect)
    {
        _activeEffects.Add(effect);
        Console.WriteLine($"  Effect '{effect.Name}' applied to {Name}.");
    }

    public void ProcessEffects()
    {
        if (_activeEffects.Count == 0) return;
        for (int i = _activeEffects.Count - 1; i >= 0; i--)
        {
            _activeEffects[i].Tick(this);
            if (_activeEffects[i].IsExpired)
            {
                Console.WriteLine($"  Effect '{_activeEffects[i].Name}' has expired on {Name}.");
                _activeEffects.RemoveAt(i);
            }
        }
    }

    public void UseItem(int index, Character? target = null)
    {
        var item = Inventory.GetItem(index);
        if (item == null) { Console.WriteLine("  No item at that index."); return; }
        Console.WriteLine($"\n{Name} uses: {item.Name}");
        item.Apply(target ?? this);
        Inventory.RemoveAt(index);
    }

    public virtual void PrintStatus()
    {
        Console.WriteLine($"  [{GetType().Name}] {Name} | HP: {Health}/{MaxHealth} | ATK: {AttackPower} | DEF: {Defense}");
    }
}
