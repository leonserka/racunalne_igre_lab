namespace RPGGame;

public interface IInventory<T>
{
    int Count { get; }
    int MaxSlots { get; }
    bool Add(T item);
    bool Remove(T item);
    bool RemoveAt(int index);
    T? GetItem(int index);
    int FindIndex(Func<T, bool> predicate);
    void PrintInventory();
}
