using System;
using System.Collections.Generic;

// Marker interface
public interface IInventoryItem
{
    int Id { get; }
    string Name { get; }
    int Quantity { get; set; }
}

// Product Classes
public class ElectronicItem : IInventoryItem
{
    public int Id { get; }
    public string Name { get; }
    public int Quantity { get; set; }
    public string Brand { get; set; }
    public int WarrantyMonths { get; set; }

    public ElectronicItem(int id, string name, int quantity, string brand, int warrantyMonths)
    {
        Id = id;
        Name = name;
        Quantity = quantity;
        Brand = brand;
        WarrantyMonths = warrantyMonths;
    }
}

public class GroceryItem : IInventoryItem
{
    public int Id { get; }
    public string Name { get; }
    public int Quantity { get; set; }
    public DateTime ExpiryDate { get; set; }

    public GroceryItem(int id, string name, int quantity, DateTime expiryDate)
    {
        Id = id;
        Name = name;
        Quantity = quantity;
        ExpiryDate = expiryDate;
    }
}

// Custom Exceptions
public class DuplicateItemException : Exception
{
    public DuplicateItemException(string message) : base(message) { }
}

public class ItemNotFoundException : Exception
{
    public ItemNotFoundException(string message) : base(message) { }
}

public class InvalidQuantityException : Exception
{
    public InvalidQuantityException(string message) : base(message) { }
}

// Generic Inventory Repository
public class InventoryRepository<T> where T : IInventoryItem
{
    private Dictionary<int, T> _items = new Dictionary<int, T>();

    public void AddItem(T item)
    {
        if (_items.ContainsKey(item.Id))
            throw new DuplicateItemException($"Item with ID {item.Id} already exists.");

        _items[item.Id] = item;
    }

    public T GetItemById(int id)
    {
        if (!_items.ContainsKey(id))
            throw new ItemNotFoundException($"Item with ID {id} not found.");

        return _items[id];
    }

    public void RemoveItem(int id)
    {
        if (!_items.ContainsKey(id))
            throw new ItemNotFoundException($"Item with ID {id} not found.");

        _items.Remove(id);
    }

    public List<T> GetAllItems()
    {
        return new List<T>(_items.Values);
    }

    public void UpdateQuantity(int id, int newQuantity)
    {
        if (newQuantity < 0)
            throw new InvalidQuantityException("Quantity cannot be negative.");

        if (!_items.ContainsKey(id))
            throw new ItemNotFoundException($"Item with ID {id} not found.");

        _items[id].Quantity = newQuantity;
    }
}

// Manager Class
public class WareHouseManager
{
    private InventoryRepository<ElectronicItem> _electronics = new InventoryRepository<ElectronicItem>();
    private InventoryRepository<GroceryItem> _groceries = new InventoryRepository<GroceryItem>();

    public void SeedData()
    {
        _electronics.AddItem(new ElectronicItem(1, "Laptop", 5, "Dell", 24));
        _electronics.AddItem(new ElectronicItem(2, "TV", 3, "Samsung", 36));

        _groceries.AddItem(new GroceryItem(1, "Rice", 10, DateTime.Now.AddMonths(6)));
        _groceries.AddItem(new GroceryItem(2, "Milk", 20, DateTime.Now.AddDays(30)));
    }

    public void PrintAllItems<T>(InventoryRepository<T> repo) where T : IInventoryItem
    {
        foreach (var item in repo.GetAllItems())
        {
            Console.WriteLine($"ID: {item.Id}, Name: {item.Name}, Quantity: {item.Quantity}");
        }
    }

    public void IncreaseStock<T>(InventoryRepository<T> repo, int id, int quantity) where T : IInventoryItem
    {
        try
        {
            var item = repo.GetItemById(id);
            repo.UpdateQuantity(id, item.Quantity + quantity);
            Console.WriteLine("Stock updated successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    public void RemoveItemById<T>(InventoryRepository<T> repo, int id) where T : IInventoryItem
    {
        try
        {
            repo.RemoveItem(id);
            Console.WriteLine("Item removed successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    public void Run()
    {
        try
        {
            SeedData();
            Console.WriteLine("\n--- Electronic Items ---");
            PrintAllItems(_electronics);

            Console.WriteLine("\n--- Grocery Items ---");
            PrintAllItems(_groceries);

            Console.WriteLine("\n--- Testing Errors ---");
            _groceries.AddItem(new GroceryItem(2, "Sugar", 15, DateTime.Now)); // Duplicate
        }
        catch (DuplicateItemException ex)
        {
            Console.WriteLine($"Duplicate Error: {ex.Message}");
        }

        RemoveItemById(_electronics, 99); // Not found
        IncreaseStock(_groceries, 1, -10); // Invalid quantity
    }
}

// Main
public class Program
{
    public static void Main()
    {
        WareHouseManager manager = new WareHouseManager();
        manager.Run();
    }
}
