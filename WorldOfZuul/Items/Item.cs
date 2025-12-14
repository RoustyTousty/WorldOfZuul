using WorldOfZuul.Events;

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
        public string? EventId { get; set; }
        public virtual bool CanPickUp => true;

        public Item(string id, string name, string description, string? useText = null, string? eventId = null)
        {
            Id = id;
            Name = name;
            Description = description;
            UseText = useText;
            EventId = eventId;
        }



        public virtual string Inspect()
        {
            return Description;
        }

        public virtual void Use()
        {
            if (EventId != null)
            {
                EventManager.Instance.Trigger(EventId);
                return;
            }

            Console.WriteLine("Nothing happens...");
        }

        public virtual void Drop()
        {
            Console.WriteLine($"You drop the {Name}.");
        }
    }
}
