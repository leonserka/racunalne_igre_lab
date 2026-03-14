namespace RPGGame;

public class Inventory<T> : IInventory<T>
{
    private List<T> _items = new List<T>();
    public int MaxSlots { get; private set; }
    public int Count => _items.Count;

    public Inventory(int maxSlots = 8)
    {
        MaxSlots = maxSlots;
    }

    public bool Add(T item)
    {
        if (_items.Count >= MaxSlots)
        {
            Console.WriteLine("  Inventory is full!");
            return false;
        }
        _items.Add(item);
        return true;
    }

    public bool Remove(T item) => _items.Remove(item);

    public bool RemoveAt(int index)
    {
        if (index < 0 || index >= _items.Count) return false;
        _items.RemoveAt(index);
        return true;
    }

    public T? GetItem(int index)
    {
        if (index < 0 || index >= _items.Count) return default;
        return _items[index];
    }

    public int FindIndex(Func<T, bool> predicate)
    {
        for (int i = 0; i < _items.Count; i++)
            if (predicate(_items[i])) return i;
        return -1;
    }

    public void PrintInventory()
    {
        Console.WriteLine($"  Inventory ({_items.Count}/{MaxSlots}):");
        if (_items.Count == 0) { Console.WriteLine("    (empty)"); return; }
        for (int i = 0; i < _items.Count; i++)
            Console.WriteLine($"    [{i}] {_items[i]}");
    }
}
