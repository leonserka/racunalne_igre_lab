namespace RPGGame;

public abstract class Item
{
    public string Name { get; protected set; }
    public string Description { get; protected set; }

    protected Item(string name, string description)
    {
        Name = name;
        Description = description;
    }

    public abstract void Apply(Character target);

    public override string ToString() => $"{Name} - {Description}";
}
