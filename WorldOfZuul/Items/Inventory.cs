namespace WorldOfZuul.Items
{
    /*
     * Inventory class representing a collection of items held by a player/entity.
     */
    public class Inventory
    {
        public List<Item> items = new List<Item>();



        /*
        * Returns the list of items in the inventory. (This is being used so be carefull)
        */
        public List<Item> GetItems()
        {
            return items;
        }



        /*
        * Adds an item to the inventory.
        */
        public void AddItem(Item item)
        {
            items.Add(item);
        }
        


        /*
        * Removes an item from the inventory.
        */
        public void RemoveItem(Item item)
        {
            items.Remove(item);
        }
    }
}