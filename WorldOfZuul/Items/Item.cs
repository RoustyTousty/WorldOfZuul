namespace WorldOfZuul.Items
{
    /*
     * Base class for all items in the game.
     */
    public class Item
    {
        public string Id { get; }
        public string Name { get; set; }
        public string Description { get; set; }

        // TODO: Add types.
        public Item(string id, string name, string description)
        {
            Id = id;
            Name = name;
            Description = description;
        }


        public string Inspect()
        {
            return Description;
        }


        // TODO: Add item pickup/use/drop methods.




    }
}
