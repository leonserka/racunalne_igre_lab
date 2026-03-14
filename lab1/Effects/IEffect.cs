namespace RPGGame;

public interface IEffect
{
    string Name { get; }
    bool IsExpired { get; }
    void Tick(Character target);
}
