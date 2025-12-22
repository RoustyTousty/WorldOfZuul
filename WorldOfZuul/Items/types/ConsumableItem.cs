using System.Reflection.Metadata.Ecma335;

namespace WorldOfZuul.Items
{
    public class ConsumableItem : Item
    {
        public int Uses { get; private set; }

        public ConsumableItem(string id, string name, string description, int uses, string? useText = null)
            : base(id, name, description, useText)
        {
            Uses = uses;   // can assig multiple uses to an item before its consumed.
        }




        public override bool Use()
        {
        if (Uses > 0)
            {
                Uses--;

                if (!string.IsNullOrWhiteSpace(UseText))
                {
                    Console.WriteLine(UseText);
                    Console.WriteLine();
                    Console.WriteLine($"{Name} has {Uses} uses left.");
                    
                

                    if (Uses == 0)
                    {
                        Console.WriteLine($"The {Name} has been fully consumed.");
                    }
                }
            }
            else
            {
                Console.WriteLine($"The {Name} has been fully consumed.");
                // TODO: remove from inventory here
                return true;  // Use succeeded, item consumed
            }
            return false;
        }
    }
}