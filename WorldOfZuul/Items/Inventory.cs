using WorldOfZuul.Items;

namespace WorldOfZuul.Items {
    /*
     * Creates an inventory managing the items .
     */
    public class Inventory
    /*creates a public class (acessible from other classes) called Inventory */
    {
        public Item[] items;
        /*creates a private array that stores each item called items */
        public Inventory(Item[] items) // Constructor that takes an array of items as parameter
        {
            this.items = items; //This.items refers to the private array(above line 10) and it is assigned the value of the parameter items(line 12)
        }
        public Inventory() : this(new Item[0]) //creates a public constructor that creates an empty inventory by calling the first constructor with an empty array
        {
        }
        public void AddItem(Item item) //Creates a public method that doesn't return anything (void) called AddItem that takes an Item as parameter
        {
            if (!Contains(items, item)) //this checks if the item is not already in the inventory 
            {
                Item[] newItems = new Item[items.Length + 1]; // Creates a new array one size bigger
                for (int i = 0; i < items.Length; i++)
                {
                    newItems[i] = items[i];
                } // Copies all existing items in the new array
                newItems[items.Length] = item; // Puts the new item at the end
                items = newItems; // Updates the inventory with new array
            }
        }
        public void RemoveItem(Item item) //Creates a public method that doesn't return anything (void) called RemoveItem that takes an Item as parameter
        {
            if (Contains(items, item)) //this checks if the item is already in the inventory
            {
                Item[] newItems = new Item[items.Length - 1]; // Creates a new array one size smaller
                int i = 0; // Start a counter at 0
                for (; items[i] != item; i++) //counts until it finds the item to remove
                {
                    newItems[i] = items[i]; //Copies the items until the one to remove is found
                }
                for (i++; i < items.Length; i++) //continues to count after the found item
                {
                    newItems[i - 1] = items[i];// Copies the remaining items, shifting them down one spot
                }
                items = newItems; // Uses the new array 
            }
        }
        public bool UseItem(Item item)
        {
            if (item is UsableItem usableItem)// Check if the item is usable
            {
                if (usableItem.Use())// Attempt to use the item
                {
                    // If the item was successfully used, remove it from inventory
                    RemoveItem(item);//removes the item from the inventory
                    return true; // Return true indicating the item was used
                }
            }
            return false; // Return false if the item was not used
        }

        public void PrintInventory() // creates a public empty method that displays the items in the inventory
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
                    Console.WriteLine(item); // Prints the first item without extra line
                }
                else
                Console.WriteLine(" - " + item);// Prints each intem with a dash in front
            }
            }
        }
        private bool Contains(Item[] items, Item item)
        {
            foreach (Item candidateItem in items)
            {
                if (candidateItem == item) return true;
            }
            return false;
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