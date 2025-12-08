namespace WorldOfZuul.Items
{
public class GenericItem : Item
    {
        
        public GenericItem(string id, string name, string description, string? useText = null)
            : base(id, name, description, useText)
        {
            
        }

        public override bool Use()
        {
            Console.WriteLine($"Can't use {Name}.");
            return false; // Use failed - item cannot be used
        }


    }
}