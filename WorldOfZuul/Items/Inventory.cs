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
            if (items.Count >= Capacity)
            {
                Console.WriteLine("Inventory is full. Cannot add more items.");
                return;
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
    }
}
