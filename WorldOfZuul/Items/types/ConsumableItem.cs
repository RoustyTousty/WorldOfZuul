namespace WorldOfZuul.Items
{
public class ConsumableItem : Item
    {
        public int Uses { get; private set; }

        public ConsumableItem(string id, string name, string description, int uses)
            : base(id, name, description)
        {
            Uses = uses;
        }


        // Consumes one use of the item.
        public override void Use()
        {
            if (Uses > 0)
            {
            Uses--;
                Console.WriteLine($"{Name} has {Uses} uses left.");

                if (Uses == 0)
                {
                    Console.WriteLine($"The {Name} has been fully consumed.");
                    // TODO: remove from inventory here
                }
            }
            else
            {
                Console.WriteLine($"The {Name} is already used up.");
            }
            
        }
    }
}