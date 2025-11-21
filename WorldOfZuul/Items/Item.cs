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

        public string? UseText { get; set; }
        public virtual bool CanPickUp => true;

        // TODO: Add types.
        public Item(string id, string name, string description, string? useText = null)
        {
            Id = id;
            Name = name;
            Description = description;
            UseText = useText;
        }



        public virtual string Inspect()
        {
            return Description;
        }

        public virtual void Use()
        {
            Console.WriteLine("nothing happens...");
        }

        public virtual void Drop()
        {
            Console.WriteLine($"You drop the {Name}.");
        }


        // TODO: Add item pickup/use/drop methods.




    }
}
