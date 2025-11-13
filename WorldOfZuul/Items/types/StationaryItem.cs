namespace WorldOfZuul.Items
{
public class StationaryItem : Item
    {
        public override bool CanPickUp => false;
        
        public StationaryItem(string id, string name, string description, string? useText = null)
            : base(id, name, description, useText)
        {
        }

        public void Interact()
        {
            Console.WriteLine($"You interact with the {Name}.");
        }

        //this item cannot be picked up, so it can't use the Use method.
    }
}
