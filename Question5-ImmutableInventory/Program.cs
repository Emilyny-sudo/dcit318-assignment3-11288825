using System;
using System.Collections.Generic;

// Define immutable record
public record InventoryItem(string ItemName, int Quantity);

class Program
{
    static void Main(string[] args)
    {
        List<InventoryItem> inventory = new();

        while (true)
        {
            Console.WriteLine("\nInventory Menu:");
            Console.WriteLine("1. Add Item");
            Console.WriteLine("2. Update Item Quantity");
            Console.WriteLine("3. Display Inventory");
            Console.WriteLine("4. Exit");
            Console.Write("Choose option (1-4): ");
            string? input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    Console.Write("Enter item name: ");
                    string? itemName = Console.ReadLine();
                    Console.Write("Enter quantity: ");
                    int quantity = int.Parse(Console.ReadLine() ?? "0");

                    inventory.Add(new InventoryItem(itemName!, quantity));
                    Console.WriteLine("Item added.");
                    break;

                case "2":
                    Console.Write("Enter item name to update: ");
                    string? nameToUpdate = Console.ReadLine();

                    var item = inventory.Find(i => i.ItemName == nameToUpdate);
                    if (item != null)
                    {
                        Console.Write("Enter new quantity: ");
                        int newQty = int.Parse(Console.ReadLine() ?? "0");

                        InventoryItem updated = item with { Quantity = newQty };
                        inventory.Remove(item);
                        inventory.Add(updated);

                        Console.WriteLine("Quantity updated (immutably).");
                    }
                    else
                    {
                        Console.WriteLine("Item not found.");
                    }
                    break;

                case "3":
                    Console.WriteLine("\n--- Inventory List ---");
                    foreach (var i in inventory)
                        Console.WriteLine($"Item: {i.ItemName}, Quantity: {i.Quantity}");
                    break;

                case "4":
                    return;

                default:
                    Console.WriteLine("Invalid choice.");
                    break;
            }
        }
    }
}
