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


        public virtual bool CanPickUp => true;

        // TODO: Add types.
        public Item(string id, string name, string description)
        {
            Id = id;
            Name = name;
            Description = description;
        }



        public virtual string Inspect()
        {
            return Description;
        }

        public virtual void Use()
        {
            Console.WriteLine($"You use the {Name}, but nothing happens.");
        }

        public virtual void Drop()
        {
            Console.WriteLine($"You drop the {Name}.");
        }


        // TODO: Add item pickup/use/drop methods.




    }
}
