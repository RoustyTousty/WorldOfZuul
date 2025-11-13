namespace WorldOfZuul.Items
{
public class StationaryItem : Item
    {
        public override bool CanPickUp => false;
        
        public StationaryItem(string id, string name, string description)
            : base(id, name, description)
        {
        }

        public void Interact()
        {
            Console.WriteLine($"You interact with the {Name}.");
        }

        public override void Use() => Interact();


        //Needs some work, I don't think stationary items can use the 'Use' command. But so far you can't pick them up.
    }
}
