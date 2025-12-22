using WorldOfZuul.Items;

namespace WorldOfZuul.Items {
    public class Inventory
    {
        private List<Item> items = new List<Item>();
        private int Capacity { get; }

        public Inventory(int capacity = 10)
        {
            this.Capacity = capacity;
        }



        public void AddItem(Item item)
        {
            if (items.Length == 0)
            {
                Console.WriteLine("Your inventory is empty.");
                return;
            }
            else
            {
                Console.WriteLine("Inventory:"); // Prints the header "Inventory:"
                foreach (Item item in items) // Loops through each item in the array(items)
                {
                    if (item == items[0])
                    {
                        Console.WriteLine($" - {item.Name} ({item.Description})"); // Prints the first item with name and description
                    }
                    else
                        Console.WriteLine($" - {item.Name} ({item.Description})"); // Prints each item with name and description
                }
            }
            items.Add(item);
        }



        public void RemoveItem(Item item)
        {
            items.Remove(item);
        }



        public List<Item> GetAllItems()
        {
            return items;
        }


        
        public Item? GetItem(string id)
        {
            return items.FirstOrDefault(i => i.Id == id);
        }
        public Item? GetItem(string itemName)
        {
            foreach (Item item in items)
            {
                if (item.Name.Equals(itemName, StringComparison.OrdinalIgnoreCase))
                    return item;
            }
            return null;
        }
        public bool HasItemWithId(string id)
        {
            foreach (var it in items)
            {
                if (it != null && it.Id == id) return true;
            }
            return false;
            }
    }
}